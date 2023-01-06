using System;
using UnityEngine;

public class View_Alarm : View_Base
{
    [SerializeField] private AudioSource m_audioSource;
    private AudioSource AudioScource { get => m_audioSource; }

    protected override void Init()
    {
        m_audioSource = GetComponent<AudioSource>();
        base.Init();
    }

    public override void OnShow(Action callback = null)
    {
        AudioScource.PlayOneShot(AudioScource.clip);
        base.OnShow(callback);
    }
}
