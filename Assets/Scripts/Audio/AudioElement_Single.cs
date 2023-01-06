using System;
using UnityEngine;

[Serializable]
public class AudioElement_Single : AudioElement_Base
{
    [SerializeField] private AudioClip m_clip;

    public override AudioClip GetClip()
    {
        return m_clip;
    }
}
