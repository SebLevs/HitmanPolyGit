using UnityEngine;

public class View_TextPrinter : View_Base
{
    [SerializeField] private UIElement_TextPrinter m_textPrinterElement;

    public UIElement_TextPrinter TextPrinterElement { get => m_textPrinterElement; }
}
