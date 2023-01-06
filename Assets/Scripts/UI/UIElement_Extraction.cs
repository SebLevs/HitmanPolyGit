using System.Collections;
using UnityEngine;

public class UIElement_Extraction : UIElement_FillingBar
{
    [Header("Popup Cue")]
    [SerializeField] private float m_waitTime_ToggleUpDown;
    [SerializeField] private float m_fontSizeModifier;
    [SerializeField] private float minTextSize = 34;
    [SerializeField] private float maxTextSize = 36;

    /// <summary>
    /// Will set active the object<br/>
    /// Can specify if title behaviour's coroutine should restart<br/>
    /// </summary>
    public void ResetObject(bool restartCoroutine = false)
    {
        m_title.gameObject.SetActive(true);
        gameObject.SetActive(true);
        m_imageForeground.fillAmount = 0.01f;
        ResetTitle(restartCoroutine);
    }

    /// <summary>
    /// Will also stop the coroutine used for visual cue
    /// </summary>
    public void ResetTitle(bool restartCoroutine = false)
    {
        StopAllCoroutines();
        SetTitle("Extraction in progress");
        m_title.fontSize = maxTextSize;
        m_title.color = Color.white;

        if (restartCoroutine)
        {
            ActivateTitleCue();
        }
    }

    public void SetTitleOutsideExtractionPoint(bool restartCoroutine = false)
    {
        StopAllCoroutines();
        SetTitle("Return to Extraction!");
        m_title.fontSize = maxTextSize;
        m_title.color = Color.red;

        if (restartCoroutine)
        {
            ActivateTitleCue();
        }
    }

    public void ActivateTitleCue()
    {
        StartCoroutine(TitleCueUp());
    }

    private IEnumerator TitleCueUp()
    {
        while (!(m_title.fontSize >= maxTextSize))
        {
            yield return new WaitForFixedUpdate();
            m_title.fontSize += m_fontSizeModifier;
        }
        yield return new WaitForSeconds(m_waitTime_ToggleUpDown);
        StartCoroutine(TitleCueDown());
    }

    private IEnumerator TitleCueDown()
    {
        while (!(m_title.fontSize <= minTextSize))
        {
            yield return new WaitForFixedUpdate();
            m_title.fontSize -= m_fontSizeModifier;
        }
        yield return new WaitForSeconds(m_waitTime_ToggleUpDown);

        StartCoroutine(TitleCueUp());
    }
}
