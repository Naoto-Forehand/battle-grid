using UnityEngine;
using TMPro;

public class RoundInfoPanel : MonoBehaviour
{
    [SerializeField]
    TMP_Text _currentRound;
    [SerializeField]
    TMP_Text _roundInfo;

    const string ROUND_FORMAT = @"ROUND: {0}";

    private void Awake()
    {
        _currentRound?.SetText(string.Empty);
        _roundInfo?.SetText(string.Empty);
    }

    public void UpdateRound(int round)
    {
        _currentRound.SetText(string.Format(ROUND_FORMAT, round));
    }

    public void RoundReport(string[] actions)
    {
        _roundInfo.SetText(string.Join("\n", actions));
    }
}
