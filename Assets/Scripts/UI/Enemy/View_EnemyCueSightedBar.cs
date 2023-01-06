using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class View_EnemyCueSightedBar : View_EnemyCueBase
{
    [SerializeField] private UIElement_EnemyCueSighted m_cueSightedElement;

    public UIElement_EnemyCueSighted CueSightedElement { get => m_cueSightedElement; }

    public override void OnShow(Action callback = null)
    {
        m_cueSightedElement.ResetFilling();
        base.OnShow(callback);
    }
}
