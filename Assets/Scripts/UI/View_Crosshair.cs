using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View_Crosshair : View_Base
{
    [SerializeField] private Image crosshairSprite;
    [SerializeField] private Color m_baseColor;

    public void OnHoverItemOn()
    {
        crosshairSprite.color = Color.white;
        crosshairSprite.rectTransform.localScale = Vector3.one * 1.5f;
        crosshairSprite.rectTransform.rotation = Quaternion.Euler(0, 0, 45);
    }

    public void OnHoverItemOut()
    {
        crosshairSprite.color = m_baseColor;
        crosshairSprite.rectTransform.localScale = Vector3.one;
        crosshairSprite.rectTransform.rotation = Quaternion.Euler(0, 0, 0);
    }
}
