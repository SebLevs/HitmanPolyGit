using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class State_Origin<T>
{
    protected T m_context;

    public State_Origin(T context)
    {
        m_context = context;
    }

    public abstract void OnEnter();
    public abstract void OnUpdate();
    public abstract void OnExit();

    public abstract void HandleStateTransition();
}
