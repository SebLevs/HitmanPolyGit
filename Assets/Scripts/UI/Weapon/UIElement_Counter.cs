using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;

public class UIElement_Counter : UIElement_Base
{
    [SerializeField] private Image m_backgroundImage;
    [SerializeField] private Color m_emptyColor;
    private Color m_baseColor;

    [Header("Wiggle")]
    [SerializeField] private float _wiggleDisplacement;
    [SerializeField] private float _quantityOfWiggles = 4;

    public Image BackgroundImage { get => m_backgroundImage; }
    public Color EmptyColor { get => m_emptyColor; }
    protected override void OnAwake()
    {
        base.OnAwake();
        m_baseColor = m_backgroundImage.color;
    }

    public override void Init()
    {
        base.Init();
    }

    public void OnWiggleNegative(bool isEmpty = false, Action callback = null)
    {
        if (!gameObject.activeSelf) { return; } 
        m_backgroundImage.color = m_emptyColor;
        if (isEmpty)
        {
            SetTitle("---");
        }
        WiggleLeftRight(() =>
        {
            if (!isEmpty) { m_backgroundImage.color = m_baseColor; }
            if (callback != null) { callback.Invoke(); }
        });
    }

    // TODO: Might want to extract this into a reusable method for any UI
    public void WiggleLeftRight(Action callback = null)
    {
        //if (!gameObject.activeSelf) { return; }
        StopAllCoroutines();
        StartCoroutine(OnWiggleLeftRight(callback));
    }

    private IEnumerator OnWiggleLeftRight(Action callback = null)
    {
        Vector3 m_defaultPosition = transform.position;
        for (int i = 0; i < _quantityOfWiggles; i++)
        {
            if (i % 2 == 0)
            {
                yield return new WaitForSeconds(0.14f);
                transform.localPosition = new Vector3(_wiggleDisplacement, transform.localPosition.y, transform.localPosition.z);
            }
            else
            {
                yield return new WaitForSeconds(0.14f);
                transform.localPosition = new Vector3(-_wiggleDisplacement, transform.localPosition.y, transform.localPosition.z);
            }
        }

        if (callback != null)
        {
            callback.Invoke();
        }

        transform.position = m_defaultPosition;
    }

    override protected IEnumerator OnExecute(Action callback = null)
    {
        m_backgroundImage.color = m_emptyColor;
        yield return new WaitForSeconds(0.14f);
        m_backgroundImage.color = m_baseColor;

        if (callback != null)
        {
            callback.Invoke();
        }
    }
}
