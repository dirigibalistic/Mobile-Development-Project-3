using UnityEngine;

public abstract class StateMachineMB : MonoBehaviour
{
    public State CurrentState {  get; private set; }
    private State _previousState;

    private bool _inTransition = false;

    public void ChangeState(State newState)
    {
        if (CurrentState == newState || _inTransition)
            return;
        ChangeStateSequence(newState);
    }

    private void ChangeStateSequence(State newState)
    {
        _inTransition = true;
        //exit old state
        CurrentState?.Exit();
        StoreStateAsPrevious(newState);

        CurrentState = newState;

        //enter new state
        CurrentState?.Enter();
        _inTransition = false;
    }

    private void StoreStateAsPrevious(State newState)
    {
        //if no previous state, this is the first
        if(_previousState == null && newState != null)
            _previousState = newState;
        //else, store current as previous
        else if (_previousState != null && CurrentState != null)
            _previousState = CurrentState;
    }

    public void ChangeStateToPrevious()
    {
        if (_previousState != null)
            ChangeState(_previousState);
        else
            Debug.LogWarning("No previous state to change to");
    }

    protected virtual void Update()
    {
        if (CurrentState != null && !_inTransition)
            CurrentState.Tick();
    }

    protected virtual void FixedUpdate()
    {
        if (CurrentState != null && !_inTransition)
            CurrentState.FixedTick();
    }

    protected virtual void OnDestroy()
    {
        CurrentState?.Exit();
    }
}