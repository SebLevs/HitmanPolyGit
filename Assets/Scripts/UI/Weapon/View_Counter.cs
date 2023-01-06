using UnityEngine;

public class View_Counter : View_Base
{
    [SerializeField] private UIElement_Counter m_counterElement;

    public UIElement_Counter CounterElement { get => m_counterElement; }
}
