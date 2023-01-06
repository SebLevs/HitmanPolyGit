using UnityEngine;

public class AudioController_Multiple : AudioController_Base
{
    [SerializeField] private AudioElement_Multiple m_audioMultiple;

    public AudioElement_Multiple AudioMultiple { get => m_audioMultiple; }

    override public void PlayOneShot()
    {
        m_audioSource.PlayOneShot(m_audioMultiple.GetClip());
    }
}
