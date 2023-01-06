using UnityEngine;

public class View_FillingBar : View_Base
{
    [SerializeField] private UIElement_FillingBar m_fillingBarElement;

    public UIElement_FillingBar FillingBarElement { get => m_fillingBarElement; }
}
