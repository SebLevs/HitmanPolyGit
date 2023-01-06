using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;
using System;

/// <summary>
/// The base class for any UIElement on a View (menu)
/// </summary>
public class UIElement_Base : MonoBehaviour//, IPointerEnterHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    [SerializeField] protected TextMeshProUGUI m_title;

    public TextMeshProUGUI MyTitle { get => m_title; }
    public Selectable Selectable { get; private set; } // Reference to this gameObject's Selectable
    public View_Base View { get; private set; }

    private void Awake()
    {
        OnAwake();
    }

    private void Start()
    {
        Init();
        OnStart();
    }

    private void Update()
    {
        OnUpdate();
    }

    protected virtual void OnAwake()
    {
        View = GetComponentInParent<View_Base>();
        Selectable = this.GetComponent<Selectable>();
    }
    
    protected virtual void OnStart() { }

    protected virtual void OnUpdate() { }
    public virtual void Init() { }

    public void SetTitle(string _newTitle)
    {
        m_title.text = _newTitle;
    }

    public void SetTitle(Color _color)
    {
        m_title.color = _color;
    }

    public void Execute(Action callback = null)
    {
        StopAllCoroutines();
        StartCoroutine(OnExecute(callback));
    }

    protected virtual IEnumerator OnExecute(Action callback) { Debug.Log("OnExecute() not implemented for: " + gameObject.name); yield return null; }

    public void ExecuteVerticalBlink(float _timerMax = 0.35f ,Transform _elementToBlink = null, Action callback = null, bool _stopAllCoroutine = false)
    {
        if (_stopAllCoroutine)
        {
            StopAllCoroutines();
        }

        //RectTransform _trueElementToBlink = (_elementToBlink == null) ? GetComponent<RectTransform>() : _elementToBlink;
        Transform _trueElementToBlink = (_elementToBlink == null) ? transform : _elementToBlink;
        StartCoroutine(OnExecuteVerticalBlink(_timerMax, _trueElementToBlink, callback));
    }

    protected virtual IEnumerator OnExecuteVerticalBlink(float _timerMax, Transform _elementToBlink, Action callback)
    {
        float timer = _timerMax;
        // Reduce scale of Y until 0
        while (timer > 0 || _elementToBlink.localScale.y > 0.0f) // TODO: Set as  a method that takes an action  (see TODO bellow) (overridable in children if there<s a desire for it)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timer -= Time.deltaTime;
            float newY = timer / _timerMax; // TODO: Set as an action for this specific implementation 
            _elementToBlink.localScale = new Vector3(1f, newY, 1f);
        }

        timer = 0.0f;
        _elementToBlink.localScale = new Vector3(1f, 0f, 1f);

        yield return new WaitForFixedUpdate();
        if (callback != null)
        {
            callback.Invoke();
        }

        // Grow scale of Y until 1
        while (timer < _timerMax || _elementToBlink.localScale.y < 1.0f) // TODO: Set as  a method that takes an action  (see TODO bellow) (overridable in children if there<s a desire for it)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            timer += Time.deltaTime;
            float newY = timer / _timerMax; // TODO: Set as an action for this specific implementation 
            _elementToBlink.localScale = new Vector3(1f, newY, 1f);
        }

        _elementToBlink.localScale = Vector3.one;
    }

    public void OnSelect()
    {
        if (Selectable)
        {
            Selectable.Select();
        }
    }

    // Pointer to simulate button
/*    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!View.CanvasGroup.interactable) { Debug.Log("Not interactable"); return; }
        Debug.Log("OnPointerEnter()");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (!View.CanvasGroup.interactable) { return; }
        //Debug.Log("OnPointerExit()");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!View.CanvasGroup.interactable) { return; }
        //Debug.Log("OnPointerClick()");
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (!View.CanvasGroup.interactable) { return; }
        //Debug.Log("OnSelected()");
        //m_audioHandler.PlaySpecificSound(UIManager.Instance.BaseSelectClip);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        if (!View.CanvasGroup.interactable) { return; }
        //Debug.Log("OnDeselect()");
    }*/

}
