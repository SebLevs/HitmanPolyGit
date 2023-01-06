using System;

public class UIElement_Time : UIElement_Base
{
    public void SetTitle(float _time) // Time
    {
        TimeSpan _timeSpan = TimeSpan.FromSeconds(_time);
        m_title.text = _timeSpan.ToString("hh':'mm':'ss':'ff");
    }
}
