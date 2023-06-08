using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIEventRouter : MonoBehaviour
{
    [SerializeField]
    private GameObject _uiRoot;
    [SerializeField]
    private InfoPanel _infoPanel;

    private void Awake()
    {
        _uiRoot = (_uiRoot == null) ? this.gameObject : _uiRoot;
    }

    public void HandleInfoButton()
    {
        if (_infoPanel != null && _infoPanel.State != PanelState.MOVING)
        {
            var targetState = (_infoPanel.State == PanelState.HIDDEN) ? PanelState.SHOWING : PanelState.HIDDEN;
            _infoPanel.SetPanelState(targetState);
        }
    }
}
