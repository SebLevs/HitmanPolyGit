using UnityEngine;

public class View_Infos : View_Base
{
    [Header("Infos")]
    [SerializeField] private UIElement_Counter _playerHP;
    [SerializeField] private UIElement_Time _currentTimer;

    public UIElement_Counter PlayerHP { get => _playerHP; }
    public UIElement_Time CurrentTimer { get => _currentTimer; }
}
