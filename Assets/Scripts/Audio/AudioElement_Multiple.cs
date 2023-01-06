using System;
using System.Linq;
using UnityEngine;

[Serializable]
public class AudioElement_Multiple : AudioElement_Base
{
    [SerializeField] private AudioClip[] m_clips;

    /// <summary>
    /// Get a random clip from an array of AudioClip
    /// </summary>
    public override AudioClip GetClip()
    {
        return m_clips[UnityEngine.Random.Range(0, m_clips.Length)];
    }

    public AudioClip GetClip(int _index)
    {
        if (_index > m_clips.Length) { return null; }
        return m_clips[_index];
    }

    public AudioClip GetClip(string _name)
    {
        return m_clips.First(x => x.name.ToLower().Contains(_name.ToLower()));
    }
}
