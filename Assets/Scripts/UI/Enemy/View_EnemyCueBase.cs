using UnityEngine;
using System;
using System.Collections;

public class View_EnemyCueBase : MonoBehaviour
{
    [SerializeField] protected float m_fadeInOutTime = 1f;

    private void Awake()
    {
        Init();
        OnAwake();
    }

    private void Start()
    {
        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void Init()
    {
    }

    protected virtual void OnAwake()
    {
    }

    protected virtual void OnStart()
    {
    }

    protected virtual void OnUpdate()
    {
    }

    public virtual void OnShow(Action callback = null)
    {
        StopAllCoroutines();
        gameObject.SetActive(true);
        StartCoroutine(OnShowIE(callback));
    }

    protected virtual IEnumerator OnShowIE(Action callback = null)
    {
        float timer = 0.0f;
        while (timer < m_fadeInOutTime || transform.localScale.x < 1) // TODO: Set as  a method that takes an action  (see TODO bellow) (overridable in children if there<s a desire for it)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            float sizeX = timer / m_fadeInOutTime;
            float sizeY = timer / m_fadeInOutTime;
            float sizeZ = timer / m_fadeInOutTime;
            transform.localScale = new Vector3(sizeX, sizeY, sizeZ); // TODO: Set as an action for this specific implementation
        }

        transform.localScale = Vector3.one;

        if (callback != null)
        {
            callback.Invoke();
        }

        //UIManager.Instance.CurrentView = this;
        UIManager.Instance.CurrentView.CanvasGroup.interactable = true;
    }

    public virtual void OnHide(Action callback = null)
    {
        StopAllCoroutines();
        StartCoroutine(OnHideIE(callback));
    }

    protected virtual IEnumerator OnHideIE(Action callback = null)
    {
        float timer = m_fadeInOutTime;
        while (timer > 0 || transform.localScale.x > 0) // TODO: Set as  a method that takes an action  (see TODO bellow) (overridable in children if there<s a desire for it)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timer -= Time.deltaTime;
            float sizeX = timer / m_fadeInOutTime;
            float sizeY = timer / m_fadeInOutTime;
            float sizeZ = timer / m_fadeInOutTime;
            transform.localScale = new Vector3(sizeX, sizeY, sizeZ); // TODO: Set as an action for this specific implementation
        }

        transform.localScale = Vector3.zero;

        if (callback != null)
        {
            callback.Invoke();
        }

        gameObject.SetActive(false); // To prevent alpha shenanigans
    }
}
