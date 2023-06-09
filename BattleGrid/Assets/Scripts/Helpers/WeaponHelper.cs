using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponHelper : MonoBehaviour
{
    [SerializeField]
    private CircleCollider2D _circleCollider;
    [SerializeField]
    private CapsuleCollider2D _capsuleCollider;

    private Vector2 _defaultPosition;
    private const float EXTEND_DISTANCE = -3.5f;
    private const float SWING_TIME = 0.1f;

    private void Awake()
    {
        _circleCollider = (_circleCollider == null) ? gameObject.GetComponentInChildren<CircleCollider2D>() : _circleCollider;
        _defaultPosition = (_circleCollider) ? _circleCollider.offset : Vector2.zero;
        if (_circleCollider != null && _circleCollider.GetComponent<ColliderHelper>() != null)
        {
            ToggleListeners(true, _circleCollider.GetComponent<ColliderHelper>());
        }
        _capsuleCollider = (_capsuleCollider == null) ? gameObject.transform.parent.GetComponentInChildren<CapsuleCollider2D>() : _capsuleCollider;
    }

    private void ToggleListeners(bool enable, ColliderHelper helper)
    {
        if (enable)
        {
            helper.OnTriggerEnter += OnTriggerEnter2D;
            helper.OnTriggerStay += OnTriggerStay2D;
            helper.OnTriggerExit += OnTriggerExit2D;
        }
        else
        {
            helper.OnTriggerEnter -= OnTriggerEnter2D;
            helper.OnTriggerStay -= OnTriggerStay2D;
            helper.OnTriggerExit -= OnTriggerExit2D;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision != _capsuleCollider && collision.tag == "Receiver")
        {
            Debug.LogWarning($"Enter {collision}");
            if (collision.transform.parent.GetComponent<SimpleCharacterView>() != null)
            {
                collision.transform.parent.GetComponent<SimpleCharacterView>().TriggerHit();
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision != _capsuleCollider && collision.tag == "Receiver")
        {
            Debug.LogWarning($"Stay {collision}");
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision != _capsuleCollider && collision.tag == "Receiver")
        {
            Debug.LogWarning($"Exit {collision}");
        }
    }

    public void OnSwing()
    {
        if (_circleCollider != null)
        {
            StartCoroutine(ExtendColliderRange());
        }
    }

    private IEnumerator ExtendColliderRange()
    {
        var currentPosition = _circleCollider.offset;
        var targetPosition = new Vector2(EXTEND_DISTANCE, currentPosition.y);
        //var elapsed = 0f;

        //yield return new WaitForSecondsRealtime(0.02f);

        _circleCollider.offset = targetPosition;

        yield return new WaitForSecondsRealtime(0.35f);
        //while (elapsed < SWING_TIME)
        //{
        //    _circleCollider.offset = Vector2.Lerp(currentPosition, targetPosition, (elapsed / SWING_TIME));
        //    currentPosition = _circleCollider.offset;
        //    elapsed += Time.fixedDeltaTime;
        //    yield return null;
        //}

        //yield return new WaitForSecondsRealtime(0.02f);
        _circleCollider.offset = _defaultPosition;

        yield return null;
    }
}
