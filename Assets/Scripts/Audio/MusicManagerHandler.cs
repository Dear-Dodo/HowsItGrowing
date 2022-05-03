using UnityEngine;

public class MusicManagerHandler : MonoBehaviour
{
    [SerializeField] public AudioClip PlayOnStartClip;

    [SerializeField] public bool PlayOnStart = true;

    public void Play() => Play(PlayOnStartClip);

    public void Play(AudioClip clip)
    {
        MusicManager.Instance.Play(clip);
    }

    public void Stop()
    {
        MusicManager.Instance.Stop();
    }
}