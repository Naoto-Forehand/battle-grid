using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class InfoPanel : MonoBehaviour
{
    [SerializeField]
    private PanelState _state;
    public PanelState State { get { return _state; } }
    private PanelState _nextState = PanelState.NONE;

    [SerializeField]
    private Vector3 _hiddenPosition;
    [SerializeField]
    private Vector3 _shownPosition;
    [SerializeField]
    private RectTransform _rectTransform;

    [SerializeField]
    private float _movingSpeed = 1.5f;

    private Vector3 _currentPosition;

    protected void Awake()
    {
        _state = PanelState.HIDDEN;
        _rectTransform = (_rectTransform == null) ? this.GetComponent<RectTransform>() : _rectTransform;
        if (_rectTransform != null)
        {
            _rectTransform.anchoredPosition = _hiddenPosition;
        }
        _currentPosition = (_rectTransform != null) ? _rectTransform.anchoredPosition : Vector3.zero;
    }

    protected void Update()
    {
        if (_nextState != PanelState.NONE)
        {
            GetNextState();
        }
    }

    private void GetNextState()
    {
        _state = PanelState.MOVING;
        var moveState = _nextState;
        _nextState = PanelState.NONE;
        StartCoroutine(MovePosition(moveState));
    }

    protected virtual IEnumerator MovePosition(PanelState targetState)
    {
        var targetPosition = (targetState == PanelState.SHOWING) ? _shownPosition : _hiddenPosition;
        var elapsedTime = 0f;
        var currentPosition = _rectTransform.anchoredPosition;
        if (_rectTransform != null)
        {
            while (elapsedTime < _movingSpeed)
            {
                _rectTransform.anchoredPosition = Vector3.Lerp(currentPosition, targetPosition, (elapsedTime / _movingSpeed));
                elapsedTime += Time.fixedDeltaTime;

                yield return null;
            }

            _rectTransform.anchoredPosition = targetPosition;
            _state = targetState;
            yield return null;
        }

        yield return null;
    }

    public virtual void SetPanelState(PanelState targetState)
    {
        _nextState = targetState;
    }
}
