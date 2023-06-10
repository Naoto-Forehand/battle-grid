using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SimpleCharacterPanel : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _characterName;
    [SerializeField]
    private TMP_Text _characterStatus;
    [SerializeField]
    private TMP_Text _characterHealth;
    [SerializeField]
    private TMP_Text _characterAttack;
    [SerializeField]
    private TMP_Text _characterDefense;


    public void Initialize(SimpleCharacter simpleCharacter)
    {
        UpdateTextFields(simpleCharacter);
    }

    public void OnCharacterUpdate(SimpleCharacter simpleCharacter)
    {
        UpdateTextFields(simpleCharacter);
    }

    private void UpdateTextFields(SimpleCharacter simpleCharacter)
    {
        _characterName.SetText(simpleCharacter.Name);
        _characterStatus.SetText(simpleCharacter.Status.ToString());
        _characterHealth.SetText($"{simpleCharacter.Health}");
        _characterAttack.SetText($"{simpleCharacter.Attack}");
        _characterDefense.SetText($"{simpleCharacter.Defense}");
    }
}
