using System;
using TMPro;
using Tools;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : Singleton<MusicManager>
{
    [SerializeField] public TextMeshProUGUI MusicText;
    [SerializeField] public AudioClip[] MusicTracks;

    [SerializeField] public AudioClip DefaultMusicTrack;

    [SerializeField] private AudioClip m_CurrentClip;

    [SerializeField] private AudioSource m_AudioSource;

    [SerializeField] public bool PlayOnStart = false;

    [SerializeField] public bool Shuffle = false;
    [SerializeField] public bool Looping = false;

    [SerializeField] public UnityEvent OnClipStart;
    [SerializeField] public UnityEvent OnClipEnd;

    //[SerializeField] public bool PlayOneTrack = false;

    private int m_PreviousTrackIndex = -1;

    /// <summary>
    /// Initializes all the events.
    /// </summary>
    public override void Awake()
    {
        base.Awake();
        if (!m_AudioSource)
            m_AudioSource = this.GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (PlayOnStart)
        {
            var handler = FindObjectOfType<MusicManagerHandler>();

            if (handler.PlayOnStart)
            {
                PlayHandler(handler);
                OnClipStart?.Invoke();
            };
        }
    }

    public void PlayHandler(MusicManagerHandler handler)
    {
        if (handler)
            handler.Play();
    }

    /// <summary>
    /// Does time related stuff.
    /// </summary>
    private void Update()
    {
        if (m_CurrentClip == null) return;

        if (!m_AudioSource.isPlaying && Shuffle)
            Play();
    }

    /// <summary>
    /// Only updated the ui on render passes.
    /// </summary>
    private void OnRenderObject()
    {
        if (MusicText)
        {
            TimeSpan currentTime = TimeSpan.FromSeconds(m_AudioSource.time);
            TimeSpan clipLength = TimeSpan.FromSeconds(m_CurrentClip.length);
            MusicText.text = $"{m_CurrentClip.name}\n{currentTime.Minutes:D2}:{currentTime.Seconds:D2} / {clipLength.Minutes:D2}:{clipLength.Seconds:D2}";
        }
    }

    /// <summary>
    /// Resets the time.
    /// </summary>
    private void OnTimeReset()
    {
        OnClipEnd?.Invoke();

        if (Shuffle)
        {
            Play();
            OnClipStart?.Invoke();
        }
    }

    /// <summary>
    /// Skips the song.
    /// </summary>
    public void Skip()
    {
        m_AudioSource.Stop();

        OnTimeReset();
    }

    public void Stop()
    {
        m_AudioSource.Stop();
        OnClipEnd?.Invoke();
    }

    public void Play(AudioClip track)
    {
        m_AudioSource.clip = m_CurrentClip = track;
        m_AudioSource.Play();
    }

    /// <summary>
    /// Plays the song.
    /// </summary>
    private void Play()
    {
        if (Looping && m_CurrentClip)
        {
            Play(m_CurrentClip);
            return;
        }
        Play(Get());
    }

    /// <summary>
    /// Gets a truly random number.
    /// </summary>
    /// <returns></returns>
    private AudioClip Get()
    {
        if (MusicTracks.Length > 1)
            return MusicTracks[EnsureRandomNumber.Get(ref m_PreviousTrackIndex, 0, MusicTracks.Length)];
        return MusicTracks.Length == 1 ? MusicTracks[0] : DefaultMusicTrack;
    }
}