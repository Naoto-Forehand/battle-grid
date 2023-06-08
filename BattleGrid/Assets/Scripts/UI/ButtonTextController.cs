using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonTextController : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _objectText;
    [SerializeField]
    private string[] _objectTexts;
    [SerializeField]
    private InfoPanel _infoPanel;

    private PanelState _panelState;

    protected void Awake()
    {
        _objectText = (_objectText == null) ? gameObject.GetComponentInChildren<TextMeshPro>(true) : _objectText;
        _panelState = (_infoPanel != null) ? _infoPanel.State : PanelState.NONE;
        var buttonText = (_infoPanel != null) ? GetTextFromPanelState(_panelState) : "";
        _objectText?.SetText(buttonText);
    }

    protected void Update()
    {
        if (_panelState != _infoPanel.State)
        {
            _panelState = _infoPanel.State;
            _objectText?.SetText(GetTextFromPanelState(_panelState));
        }
    }

    private string GetTextFromPanelState(PanelState state)
    {
        if (_objectTexts.Length > 0 && (int)state < _objectTexts.Length && (int)state > -1)
        {
            return _objectTexts[(int)state];
        }
        else
        {
            return "";
        }
    }
}
