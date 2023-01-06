using System.Collections;
using TMPro;
using UnityEngine;
using System;

public class UIElement_QuestBase : UIElement_Base
{
    [SerializeField] protected TextMeshProUGUI m_body;

    public void InitQuestTexts(string _title, string _body)
    {
        SetTitle(_title);
        SetBody(_body);
    }

    public void SetBody(string _newBody)
    {
        m_body.text = _newBody;
    }

    public void SetBody(Color _color)
    {
        m_body.color = _color;
    }

    public void OnPopup(float _popupTime = 0.75f, Action callback = null)
    {
        StopAllCoroutines();
        StartCoroutine(PopupQuestIE(_popupTime, callback));
    }

    public void OnPopout(float _popupTime = 0.75f, Action callback = null)
    {
        StopAllCoroutines();
        StartCoroutine(PopoutQuestIE(_popupTime, callback));
    }

    public virtual IEnumerator PopupQuestIE(float _popupTime = 0.75f, Action callback = null)
    {
        float timer = 0.0f;
        while (timer < _popupTime || transform.localScale.y < 1.0f)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;

            float newX = timer / _popupTime;
            float newY = timer / _popupTime;
            transform.localScale = new Vector3(newX, newY, 1);
        }

        transform.localScale = Vector3.one; // In case of overflow

        if (callback != null)
        {
            callback.Invoke();
        }
    }

    public virtual IEnumerator PopoutQuestIE(float _popoutTime = 0.75f, Action callback = null)
    {
        float timer = _popoutTime;
        while (timer > _popoutTime || transform.localScale.y > 0)
        {
            yield return new WaitForFixedUpdate();
            timer -= Time.deltaTime;

            float newX = timer / _popoutTime;
            float newY = timer / _popoutTime;
            transform.localScale = new Vector3(newX, newY, 1);
        }

        transform.localScale = Vector3.zero; // In case of overflow

        if (callback != null)
        {
            callback.Invoke();
        }
    }
}
