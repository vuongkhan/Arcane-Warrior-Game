using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    private Dictionary<EState, BaseState<EState>> states = new Dictionary<EState, BaseState<EState>>();
    private BaseState<EState> currentState;
    private EState queuedState;

    public void RegisterState(EState key, BaseState<EState> state)
    {
        states[key] = state;
    }

    public void SetInitialState(EState key)
    {
        currentState = states[key];
        currentState.EnterState();
    }

    void Update()
    {
        if (currentState == null) return;

        EState nextStateKey = currentState.GetNextState();
        if (nextStateKey.Equals(currentState.StateKey))
        {
            currentState.UpdateState();
        }
        else if (states.ContainsKey(nextStateKey))
        {
            Debug.Log($"Transitioning to state: {nextStateKey}");
            TransitionToState(nextStateKey);
        }
    }
    public EState GetQueuedState()
    {
        return queuedState;
    }

    public void QueueNextState(EState stateKey)
    {
        queuedState = stateKey;
    }

    private void TransitionToState(EState stateKey)
    {
        if (!states.TryGetValue(stateKey, out var nextState)) return;

        currentState.ExitState();
        currentState = nextState;
        currentState.EnterState();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        currentState?.OnTriggerEnter2D(other);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        currentState?.OnTriggerStay2D(other);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        currentState?.OnTriggerExit2D(other);
    }
}
