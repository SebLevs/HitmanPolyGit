using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AudioPair
{
    [SerializeField] private string _key;
    [SerializeField] private AudioClip _value;

    public string Key { get => _key; }
    public AudioClip Value { get => _value; }
}

[Serializable]
public class AudioContainer
{
    [SerializeField] private string _key;
    [SerializeField] private AudioClip[] _value;

    public string Key { get => _key; }
    public AudioClip[] Value { get => _value; }
}

[Serializable]
public class AudioHandler
{
    [Header("Source")]
    [SerializeField] private AudioSource m_Source;

    // TODO: Try to find a way to transition into drag&drop "Packages" without having to create a hierarchy
    [Header("Clips")]
    [Tooltip("one key per audio clip")]
    [SerializeField] private List<AudioPair> m_audioPairs = new List<AudioPair>();
    [Tooltip("one key per array of audio clips")]
    [SerializeField] private List<AudioContainer> m_audioContainers = new List<AudioContainer>();

    public AudioSource AudioSource { get => m_Source; private set => m_Source = value; }

    /// <summary>
    /// Use this instead of constructor to avoid emptying inspector based variable assignment</br>
    /// Will override the variables passed as parameter
    /// </summary>
    public void Init(AudioSource source)
    {
        AudioSource = source;
    }

    /// <summary>
    /// Plays a random sound from a base array of AudioClips<br/>
    /// If parameter is left to null, will take this.class m_baseClips as array
    /// </summary>
    public void PlayRandomSound(string _key)
    {
        try
        {
            AudioClip[] _clips = GetAudioClips(_key);
            int index = UnityEngine.Random.Range(0, _clips.Length);
            AudioClip sound = _clips[index];
            AudioSource.PlayOneShot(sound);
        }
        catch (Exception exception)
        {
            Debug.LogError($"And error has occured at {nameof(PlayRandomSound)} of {nameof(AudioHandler)}: <b>{exception.Message}</b>");
        }
    }

    public void PlayRandomSound(AudioClip[] _clips)
    {
        int index = UnityEngine.Random.Range(0, _clips.Length);
        AudioClip sound = _clips[index];
        AudioSource.PlayOneShot(sound);
    }

    /// <summary>
    /// Try to fetch an audio clip with the specified key value from this.audioPair list
    /// </summary>
    public void PlaySpecificSound(string _key)
    {
        try
        {
            AudioClip _clip = GetAudioClip(_key);
            PlaySpecificSound(_clip);
        }
        catch (Exception exception)
        {
            Debug.LogError($"And error has occured at {nameof(PlaySpecificSound)} of {nameof(AudioHandler)}: <b>{exception.Message}</b>");
        }
    }

    public void PlaySpecificSound(AudioClip _clip)
    {
        AudioSource.PlayOneShot(_clip);
    }

    /// <summary>
    /// Will convert ToLower() to try and avoid the string based typo hellscape
    /// </summary>
    public AudioClip GetAudioClip(string _key)
    {
        return m_audioPairs.Find(x => x.Key.ToLower() == _key.ToLower()).Value;
    }

    public AudioClip[] GetAudioClips(string _key)
    {
        return m_audioContainers.Find(x => x.Key.ToLower() == _key.ToLower()).Value;
    }

    // TODO: Sort for same key values both in pairs and clips: Print warning message if it happens?
}
