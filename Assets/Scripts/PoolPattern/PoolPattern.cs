using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Component pool pattern only | Gameobject case is still to be implemented
/// </summary>
[Serializable]
public class PoolPattern<T>
{
    [Header("Default values")]
    [SerializeField] private int _defaultQuantity = 5;

    [Header("Prefab reference")]
    [SerializeField] private GameObject _elementPrefab;

    [Header("Desired Parents")]
    [SerializeField] private Transform _availableParent;
    [SerializeField] private Transform _currentlyUsedParent;

    private Queue<T> m_availablePool;
    private Queue<T> m_currentlyUsedPool;

    /// <summary>
    /// Initialise the class<br/>
    /// Use this instead of new to avoid clearing out this.script's variables
    /// </summary>
    public void Init()
    {
        m_currentlyUsedPool = new Queue<T>();
        m_availablePool = new Queue<T>();

        for (int i = 0; i < _defaultQuantity; i++)
        {
            m_availablePool.Enqueue(GameObject.Instantiate(_elementPrefab, _availableParent).GetComponent<T>());
        }
    }

    /// <summary>
    /// Force a parent set for a specific element
    /// </summary>
    public void SetParent(T _element, Transform _newParent)
    {
        (_element as Component).transform.SetParent(_newParent);
    }

    /// <summary>
    /// Will transfer the element into the m_currentlyUsedPool pool
    /// </summary>
    public T DequeueFromAvailable()
    {
        T _element;
        if (!m_availablePool.TryDequeue(out _element))
        {
            _element = GameObject.Instantiate(_elementPrefab, _availableParent).GetComponent<T>();
        }
        m_currentlyUsedPool.Enqueue(_element);
        SetParent(_element, _currentlyUsedParent);

        return _element;
    }

    /// <summary>
    /// Will transfer the element into the m_availablePool pool
    /// </summary>
    public T DequeueFromCurrentlyUsed()
    {
        T _element = m_currentlyUsedPool.Dequeue();
        m_availablePool.Enqueue(_element);
        SetParent(_element, _availableParent);

        return _element;
    }
}
