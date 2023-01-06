using System;
using UnityEngine;

public class View_Extraction : View_Base
{
    [SerializeField] private UIElement_Extraction m_extractionElement;

    public UIElement_Extraction ExtractionElement { get => m_extractionElement; }

    public override void OnShow(Action callback = null)
    {
        base.OnShow(callback);
        m_extractionElement.ResetObject();
    }

    public void OnCompletion(string _completionText, float _hideAfterNSeconds = 1.0f)
    {
        m_extractionElement.SetTitle(_completionText);
        Invoke(nameof(OnHide), _hideAfterNSeconds);
    }

    protected override void OnStart()
    {
        base.OnStart();
        ExtractionElement.ResetObject(true);
    }
}
