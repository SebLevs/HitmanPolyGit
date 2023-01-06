using System;
using UnityEngine;

public abstract class AudioController_Base : MonoBehaviour
{
    [SerializeField] protected AudioSource m_audioSource;

    public AudioSource AudioSource { get => m_audioSource; }

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

    private void FixedUpdate()
    {
        OnFixedUpdate();
    }

    protected virtual void Init() { }

    protected virtual void OnAwake() { }

    protected virtual void OnStart() { }

    protected virtual void OnUpdate() { }

    protected virtual void OnFixedUpdate() { }

    public virtual void PlayOneShot() { throw new NotImplementedException(); }

    public void PlayOneShot(AudioClip _clip)
    {
        m_audioSource.PlayOneShot(_clip);
    }

    // TODO: Set AudioSource variable handling methods here
}
