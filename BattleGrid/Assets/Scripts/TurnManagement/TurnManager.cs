using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    [SerializeField]
    private UIEventRouter _uiEventRouter;
    [SerializeField]
    private CharacterContainerHelper _characterContainerHelper;

    [SerializeField]
    private int _currentRound = 0;
    private int _turnSelectionIndex = 0;
    private List<Action> _roundActions;
    private SimpleCharacterView[] _charactersInScene;
    private SimpleCharacterView[] _currentRoundOrder;

    private bool _waitingForInput = false;

    public event Action<int> OnRoundUpdate;
    public event Action<string[]> OnRoundReport;

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        StartRound(ref _currentRound);
    }

    private void Initialize()
    {
        _charactersInScene = _characterContainerHelper?.CharactersInScene;
        if (_uiEventRouter != null)
        {
            _uiEventRouter.SubscribePanels(_characterContainerHelper);
            _uiEventRouter.SubscribeRoundInfo(this);
        }
    }

    private SimpleCharacterView[] GetTurnOrder()
    {
        var rand = new System.Random();
        Stack<SimpleCharacterView> turnStack = new Stack<SimpleCharacterView>();
        List<int> addedIndexes = new List<int>();

        while (turnStack.Count < _charactersInScene.Length)
        {
            var index = rand.Next(_charactersInScene.Length);
            if (!addedIndexes.Contains(index))
            {
                turnStack.Push(_charactersInScene[index]);
                addedIndexes.Add(index);
            }
        }

        SimpleCharacterView[] turnOrder = new SimpleCharacterView[turnStack.Count];

        for (int index = turnOrder.Length; turnStack.Count > 0; --index)
        {
            var view = turnStack.Pop();
            turnOrder.SetValue(view, (index - 1));
        }

        return turnOrder;
    }

    private void StartRound(ref int round)
    {
        round += 1;
        OnRoundUpdate?.Invoke(round);
        _roundActions = new List<Action>();
        _turnSelectionIndex = 0;
        _currentRoundOrder = GetTurnOrder();
        _waitingForInput = true;
        _currentRoundOrder[0].ChangeViewState(SimpleCharacterState.SELECTED);
    }

    private void AdvanceThroughOrder()
    {
        _currentRoundOrder[_turnSelectionIndex].ChangeViewState(SimpleCharacterState.UNSELECTED);
        ++_turnSelectionIndex;
        if (_turnSelectionIndex < _currentRoundOrder.Length)
        {
            _currentRoundOrder[_turnSelectionIndex].ChangeViewState(SimpleCharacterState.SELECTED);
        }
        else
        {
            EndRound();
        }
    }

    private void EndRound()
    {
        _currentRoundOrder = new SimpleCharacterView[0];
        _waitingForInput = false;
        StartCoroutine(ProcessActions());
    }

    private IEnumerator ProcessActions()
    {
        string[] roundReport = new string[_roundActions.Count];
        for (int index = 0; index < _roundActions.Count; ++index)
        {
            var action = _roundActions[index];
            var actor = action.Target.ToString();
            var method = action.Method.ToString();
            roundReport.SetValue($"{actor} {method}", index);
            yield return StartCoroutine(ProcessAction(action));
        }

        OnRoundReport?.Invoke(roundReport);
        StartRound(ref _currentRound);
    }

    private IEnumerator ProcessAction(Action action)
    {
        yield return null;
        action();
        yield return new WaitForSecondsRealtime(0.5f);
    }

    private void Update()
    {
        if (_waitingForInput)
        {
            if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                //TODO: Actual target selection, for now, just other available target
                var targetIndex = (_turnSelectionIndex == 0) ? 1 : 0;
                var target = _currentRoundOrder[targetIndex];
                _currentRoundOrder[_turnSelectionIndex].SetTarget(target);
                _roundActions.Add(_currentRoundOrder[_turnSelectionIndex].Attack);
                AdvanceThroughOrder();
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                _currentRoundOrder[_turnSelectionIndex].SetDefend(_currentRoundOrder[_turnSelectionIndex]);
                _roundActions.Add(_currentRoundOrder[_turnSelectionIndex].Defend);
                AdvanceThroughOrder();
            }
        }
    }
}
