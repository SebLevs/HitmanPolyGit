using UnityEngine;
using System;

public class SequentialTimer
{
    private Action m_callback;
    private float m_targetTime;
    public float CurrentTime { get; private set; }
    public bool IsPaused { get; private set; }
    public bool HasReachedTargetTime => CurrentTime >= m_targetTime;

    public SequentialTimer(float _desiredTime, Action _callback)
    {
        m_targetTime = _desiredTime;
        m_callback = _callback;
        IsPaused = false;
    }

    /// <summary>
    /// Will not update if IsPaused
    /// </summary>
    /// <returns>HasReachedTargetTime</returns>
    public bool OnUpdateTime()
    {
        if (IsPaused) { return HasReachedTargetTime; }

        CurrentTime += Time.deltaTime;
        if (HasReachedTargetTime)
        {
            PauseTimer();
            m_callback();
            return true;
        }

        return false;
    }

    public void StartTimer()
    {
        IsPaused = false;
    }

    public void PauseTimer()
    {
        IsPaused = true;
    }

    public void ResetTimer(bool _isPaused = false)
    {
        IsPaused = _isPaused;
        CurrentTime = 0;
    }
}
