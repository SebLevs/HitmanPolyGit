using System;
using System.Collections.Generic;
using System.Linq;

public class TimerManager : Manager<TimerManager>
{
    private List<SequentialTimer> m_timers;
    public int Count => m_timers.Count;

    protected override void Init()
    {
        base.Init();
        m_timers = new List<SequentialTimer>();
    }

    private void Update()
    {
        // if (!GameManager.Instance.IsGamePaused)
        if (m_timers.Any())
        {
            for (int i = 0; i < m_timers.Count; i++)
            {
                if (m_timers[i].OnUpdateTime())
                {
                    m_timers.RemoveAt(i);
                }
            }
        }
    }

    public SequentialTimer AddSequentialTimer(float _desiredTime, Action _callback)
    {
        SequentialTimer _timer = new SequentialTimer(_desiredTime, _callback);
        m_timers.Add(_timer);
        return _timer;
    }

    public void RemoveTimer(SequentialTimer _callbackTimer)
    {
        m_timers.Remove(_callbackTimer);
    }
}
