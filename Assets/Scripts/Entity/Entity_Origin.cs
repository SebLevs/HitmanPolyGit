using System;
using UnityEngine;
public abstract class Entity_Origin : MonoBehaviour
{
    private void Awake()
    {
        OnAwake();
    }

    private void Start()
    {
        Init();
        OnStart();
    }

    private void FixedUpdate()
    {
        //  TODO: delete?
        if (GameManager.Instance.IsPaused) { return; }
        OnFixedUpdate();
    }

    private void Update()
    {
        // TODO: delete?
        if (GameManager.Instance.IsPaused) { return; }
        OnUpdate();
    }

    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void OnFixedUpdate() { }
    protected virtual void Init() { }
    protected virtual void OnUpdate() { }
}
