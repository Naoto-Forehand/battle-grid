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

    public void SubscribeRoundInfo(TurnManager turnManager)
    {
        turnManager.OnRoundUpdate += _infoPanel.RoundInfoPanel.UpdateRound;
        turnManager.OnRoundReport += _infoPanel.RoundInfoPanel.RoundReport;
    }

    public void SubscribePanels(CharacterContainerHelper containerHelper)
    {
        var characters = containerHelper.CharactersInScene;
        if (characters.Length == _infoPanel.CharacterPanels.Length)
        {
            for (int index = 0; index < characters.Length; ++index)
            {
                var character = characters[index];
                _infoPanel.CharacterPanels[index].Initialize(character.SimpleCharacter);
                character.OnCharacterUpdate += _infoPanel.CharacterPanels[index].OnCharacterUpdate;
            }
        }
        else
        {
            Debug.LogError("Cannot initialize Panels as the array lengths are mismatched");
        }
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
