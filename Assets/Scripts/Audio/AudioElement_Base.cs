using System;
using UnityEngine;

[Serializable]
public abstract class AudioElement_Base
{
    public abstract AudioClip GetClip();

    public virtual void Init() { }
}
