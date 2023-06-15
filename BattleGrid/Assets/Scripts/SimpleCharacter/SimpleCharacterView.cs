using UnityEngine;
using System;
using System.Collections;
using System.Linq;

public class SimpleCharacterView : MonoBehaviour
{
    [SerializeField]
    private SimpleCharacter _simpleCharacter;
    public SimpleCharacter SimpleCharacter { get { return _simpleCharacter; } }

    [SerializeField]
    private GameObject _characterModel;
    [SerializeField]
    private GameObject _characterBackground;

    [SerializeField]
    private int _defaultHealth = 100;
    [SerializeField]
    private int _defaultAttack = 10;
    [SerializeField]
    private int _defaultDefense = 10;

    public SimpleCharacterState SimpleCharacterState { get { return _simpleCharacterState; } }
    private SimpleCharacterState _simpleCharacterState = SimpleCharacterState.NONE;

    [SerializeField]
    private Color[] _stateColors = new Color[] { Color.white, Color.green, Color.red };

    private enum _colors
    {
        DEFAULT = 0,
        SELECTED = 1,
        DEFENDING = 2
    }

    private bool _isUpdatingViewState = false;
    private Color _defaultColor;
    private const float DEFAULT_ANIMATION_TIME = 1.5f;

    private SimpleCharacterView _target;

    public event Action<SimpleCharacter> OnCharacterUpdate;

    private void Awake()
    {
        SetDefaults();
    }

    protected virtual void SetDefaults()
    {
        if (_simpleCharacter == null)
        {
            SimpleCharacterFacade facade = new SimpleCharacterFacade { Name = gameObject.name, Attack = _defaultAttack, Defense = _defaultDefense, Health = _defaultHealth };
            _simpleCharacter = new SimpleCharacter(facade);
        }

        _simpleCharacterState = SimpleCharacterState.UNSELECTED;

        if (_characterBackground != null)
        {
            var spriteRenderer = GetSpriteRenderer(_characterBackground);
            _defaultColor = spriteRenderer.color;
        }
    }

    public void ChangeViewState(SimpleCharacterState nextState)
    {
        if (!_isUpdatingViewState)
        {
            switch (nextState)
            {
                case SimpleCharacterState.UNSELECTED:
                    StartCoroutine(SetUnselected());
                    break;
                case SimpleCharacterState.SELECTED:
                    StartCoroutine(SetSelected());
                    break;
                case SimpleCharacterState.NONE:
                default:
                    break;
            }
        }
    }

    public void SetTarget(SimpleCharacterView target)
    {
        _target = target;
    }

    public void Attack()
    {
        if (!_simpleCharacter.Status.HasFlag(SimpleCharacterStatus.DEAD))
        {
            Debug.Log($"{name} is Attacking");
            _characterModel.GetComponent<Animator>().SetTrigger("attack");
            //TODO: For now send damage here
            _target.SimpleCharacter.CacheDamage(_simpleCharacter.Attack);
        }
        
        _target = null;
    }

    public void Parry()
    {
        Debug.Log($"{name} is trying to Parry");
        // TODO: Initiate some lazy element to receive then Trigger Attack animation
    }

    public void Defend()
    {
        Debug.Log($"{name} is on Defense");
    }

    public void SetDefend(SimpleCharacterView target)
    {
        Debug.Log($"{name} is Defending");
        // TODO: Initiate something to indicate defending
        target.SimpleCharacter.CacheDefense(_simpleCharacter.Defense);
        StartCoroutine(SetDefending());
    }

    public void TriggerHit()
    {
        Debug.Log($"{name} has been hit");
        var renderer = GetSpriteRenderer(_characterBackground);
        if (renderer.color != _stateColors[(int)_colors.DEFENDING])
        {
            _characterModel.GetComponent<Animator>().SetTrigger("hit");
        }
        
        _simpleCharacter.HandleReceivingDamage();

        if (_simpleCharacter.Health <= 0)
        {
            _characterModel.GetComponent<Animator>().SetTrigger("death");
        }

        if (renderer.color != _defaultColor)
        {
            renderer.color = _defaultColor;
        }

        OnCharacterUpdate?.Invoke(_simpleCharacter);
    }

    private IEnumerator SetSelected()
    {
        _isUpdatingViewState = true;
        var spriteRenderer = GetSpriteRenderer(_characterBackground);
        var animationTime = DEFAULT_ANIMATION_TIME;
        var targetColor = _stateColors[(int)_colors.SELECTED];

        yield return StartCoroutine(LerpSpriteColor(spriteRenderer, animationTime, targetColor));

        _isUpdatingViewState = false;
        yield return null;
    }

    private IEnumerator SetUnselected()
    {
        _isUpdatingViewState = true;
        var spriteRenderer = GetSpriteRenderer(_characterBackground);
        var animationTime = DEFAULT_ANIMATION_TIME;
        var targetColor = _defaultColor;

        yield return StartCoroutine(LerpSpriteColor(spriteRenderer, animationTime, targetColor));

        _isUpdatingViewState = false;
        yield return null;
    }

    private IEnumerator SetDefending()
    {
        _isUpdatingViewState = true;
        var spriteRenderer = GetSpriteRenderer(_characterBackground);
        var animationTime = DEFAULT_ANIMATION_TIME;
        var targetColor = _stateColors[(int)_colors.DEFENDING];

        yield return StartCoroutine(LerpSpriteColor(spriteRenderer, animationTime, targetColor));

        _isUpdatingViewState = false;
        yield return null;
    }

    private IEnumerator LerpSpriteColor(SpriteRenderer renderer, float time, Color targetColor)
    {
        var elapsed = 0f;
        var currentColor = renderer.color;

        while (elapsed < time)
        {
            renderer.color = Color.Lerp(currentColor, targetColor, (elapsed / time));
            currentColor = renderer.color;
            elapsed += Time.fixedDeltaTime;
            yield return null;
        }

        if (renderer.color != targetColor)
        {
            renderer.color = targetColor;
        }

        yield return null;
    }

    private SpriteRenderer GetSpriteRenderer(GameObject target, string objectName = "")
    {
        if (target != null && target.GetComponent<SpriteRenderer>() != null)
        {
            return target.GetComponent<SpriteRenderer>();
        }
        else if (target != null && !string.IsNullOrEmpty(objectName))
        {
            var renderers = target.GetComponentsInChildren<SpriteRenderer>();
            var renderer = renderers.FirstOrDefault(rend => rend.name.Contains(objectName));
            if (renderer)
            {
                return renderer;
            }
            else
            {
                throw new MissingReferenceException($"{objectName} has no matching renderer in the target's children");
            }
        }
        else
        {
            throw new MissingReferenceException("Missing Target or invalid Target object");
        }
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.A) && name.Contains("01"))
        {
            _characterModel.GetComponent<Animator>().SetTrigger("attack");
        }
    }
}
