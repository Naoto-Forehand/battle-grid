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

    private void Awake()
    {
        Initialize();
    }

    private void Start()
    {
        StartRound(_currentRound);
    }

    private void Initialize()
    {
        _charactersInScene = _characterContainerHelper?.CharactersInScene;
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

    private void StartRound(int round)
    {
        round++;
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
        for (int index = 0; index < _roundActions.Count; ++index)
        {
            yield return StartCoroutine(ProcessAction(_roundActions[index]));
        }

        StartRound(_currentRound);
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
                _roundActions.Add(_currentRoundOrder[_turnSelectionIndex].Attack);
                AdvanceThroughOrder();
            }
        }
    }
}
