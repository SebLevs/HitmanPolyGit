using UnityEngine;

public class MasterOfState<T>
{
    private State_Origin<T> m_currentState;

    public State_Origin<T> CurrentState { get => m_currentState; }

    public MasterOfState(State_Origin<T> m_initialState)
    {
        m_currentState = m_initialState;
        m_currentState.OnEnter();
    }

    /// <summary>
    /// 1 - Exit current state<br/>
    /// 2 - Set and enter parameter state
    /// </summary>
    public void OnTransitionState(State_Origin<T> state)
    {
        m_currentState.OnExit();
        m_currentState = state;
        m_currentState.OnEnter();
    }

    public void OnUpdate()
    {
        m_currentState.OnUpdate();
        m_currentState.HandleStateTransition();
    }
}
