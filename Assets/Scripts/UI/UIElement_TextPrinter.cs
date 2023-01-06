using System;
using System.Collections;
using UnityEngine;

public class UIElement_TextPrinter : UIElement_Base
{
    [Header("Texts to print")]
    [TextArea(3, 10)]
    [SerializeField] protected string[] m_texts;

    [Header("Audio")]
    [SerializeField] protected AudioHandler m_audioHandler;

    protected override IEnumerator OnExecute(Action callback)
    {
        yield return null;

        StartCoroutine(OnExecutePrintCharByChar());

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    public void OnPrintCharByChar()
    {
        StopAllCoroutines();
        StartCoroutine(OnExecutePrintCharByChar());
    }

    protected virtual IEnumerator OnExecutePrintCharByChar()
    {
        UIManager _uiManager = UIManager.Instance;

        foreach (string line in m_texts)
        {
            m_title.text = "";
            for (int i = 0; i < line.Length; i++)
            {
                m_title.text += line[i];
                m_audioHandler.PlaySpecificSound("Print");
                yield return new WaitForSeconds(_uiManager.CharacterPrintSpeed.Value);
            }

            for (int i = 0; i < 4; i++)
            {
                m_title.text = (i % 2 == 0) ? line + "_" : line;
                yield return new WaitForSeconds(_uiManager.LinePrintPauseBetween.Value * 0.25f);
            }
        }
    }
}
