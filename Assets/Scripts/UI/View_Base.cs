using System.Collections;
using UnityEngine;
using System;

public class View_Base : MonoBehaviour
{
    [SerializeField] protected float m_fadeInOutTime = 1f;
    [SerializeField] protected UIElement_Base m_defaultSelectedElement;
    private const float quickHideTime = 0.25f;

    protected UIElement_Base m_lastSelectedElement;

    [SerializeField] protected CanvasGroup m_canvasGroup;

    public UIElement_Base DefaultSelectedElement { get => m_defaultSelectedElement; }
    public CanvasGroup CanvasGroup { get => m_canvasGroup; }

    private void Awake()
    {
        OnAwake();
        Init();
    }

    private void Start()
    {
        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnAwake()
    {
        if (!m_canvasGroup)
        {
            m_canvasGroup = GetComponent<CanvasGroup>();
            Debug.LogWarning($"m_cavasGroup of {this.name} was set in Awake(). Please set the canvasGroup reference in the gameObject");
        }
    }

    /// <summary>
    /// Called in Awake(), after OnAWake()
    /// </summary>
    protected virtual void Init() { }

    protected virtual void OnStart()
    {
        if (m_defaultSelectedElement)
        {
            m_defaultSelectedElement.OnSelect();
        }
    }

    protected virtual void OnUpdate()
    {
    }

    public virtual void OnShow(Action callback = null)
    {
        StopAllCoroutines();
        //m_canvasGroup.alpha = 0; // Make sure it's hidden
        gameObject.SetActive(true);
        //UIManager.Instance.CurrentView = this;
        
        StartCoroutine(OnShowIE(callback));
    }

    protected virtual IEnumerator OnShowIE(Action callback = null)
    {
        float timer = 0.0f;
        while (timer < m_fadeInOutTime || m_canvasGroup.alpha != 1) // TODO: Set as  a method that takes an action  (see TODO bellow) (overridable in children if there<s a desire for it)
        {
            yield return new WaitForFixedUpdate();
            timer += Time.deltaTime;
            m_canvasGroup.alpha = timer / m_fadeInOutTime; // TODO: Set as an action for this specific implementation
        }

        m_canvasGroup.interactable = true;
        m_canvasGroup.alpha = 1;

        if (callback != null)
        {
            callback.Invoke();
        }

        // If no selected element, select default element (if any)
        SetAndSelectLastElement();

        //UIManager.Instance.CurrentView = this;
        UIManager.Instance.CurrentView.CanvasGroup.interactable = true;
    }

    public virtual void OnHide(Action callback = null)
    {
        StopAllCoroutines();
        m_canvasGroup.interactable = false; // Prevent spaming
        StartCoroutine(OnHideIE(callback));
    }

    public void OnHideWithoutAction()
    {
        StopAllCoroutines();
        m_canvasGroup.interactable = false; // Prevent spaming
        StartCoroutine(OnHideIE());
    }

    protected virtual IEnumerator OnHideIE(Action callback = null)
    {
        m_canvasGroup.interactable = false;
        float timer = m_fadeInOutTime;
        while (timer > 0 || m_canvasGroup.alpha != 0) // TODO: Set as  a method that takes an action  (see TODO bellow) (overridable in children if there<s a desire for it)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timer -= Time.deltaTime;
            m_canvasGroup.alpha = timer / m_fadeInOutTime; // TODO: Set as an action for this specific implementation 
        }

        m_canvasGroup.alpha = 0;

        if (callback != null)
        {
            callback.Invoke();
        }

        gameObject.SetActive(false); // To prevent alpha shenanigans
    }

    /// <summary>
    /// Always hide in quickHideTime value
    /// </summary>
    public void OnQuickHide()
    {
        StartCoroutine(OnQuickHideIE());
    }

    protected virtual IEnumerator OnQuickHideIE()
    {
        float timer = quickHideTime;
        while (timer > 0 || m_canvasGroup.alpha != 0) // TODO: Set as  a method that takes an action  (see TODO bellow) (overridable in children if there<s a desire for it)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timer -= Time.deltaTime;
            m_canvasGroup.alpha = timer / m_fadeInOutTime; // TODO: Set as an action for this specific implementation 
        }

        m_canvasGroup.alpha = 0;

        gameObject.SetActive(false); // To prevent alpha shenanigans
    }

    protected void SetAndSelectLastElement()
    {
        if (!m_lastSelectedElement)
        {
            m_lastSelectedElement = m_defaultSelectedElement;
        }

        if (m_lastSelectedElement)
        {
            m_lastSelectedElement.OnSelect();
        }
    }
}
