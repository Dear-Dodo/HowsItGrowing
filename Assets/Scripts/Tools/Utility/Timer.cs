using System.Collections;
using UnityEngine;
using UnityEngine.Events;


/// <summary>
/// Timer class that calls a callback at via delay.
/// </summary>
public class Timer
{
    // these variables will show in the unity inspector.

    [SerializeField] internal float TimeToWait;
    [SerializeField] internal bool PlayOnStart;
    [SerializeField] internal UnityEvent OnTimerFinished;
    [SerializeField] internal bool Running;

    // these variables will not show in the unity inspector but can be accessed through code.

    [HideInInspector] internal UnityEvent OnPlay;
    [HideInInspector] internal UnityEvent OnStop;

    #region Unity Functions

    // called before anything.
    private void Init()
    {
        InitializeEvents();
    }

    private void InitializeEvents()
    {
        if (OnPlay == null)
            OnPlay = new UnityEvent();
        if (OnStop == null)
            OnStop = new UnityEvent();
        if (OnTimerFinished == null)
            OnTimerFinished = new UnityEvent();

        AddListeners();
    }

    private void AddListeners()
    {
        OnPlay.AddListener(() => Running = true);
    }

    // called only on the first frame of instantiation.

    #endregion Unity Functions

    #region Contructors

    public Timer() : this(0.0f, false)
    {
    }

    public Timer(float time) : this(time, false)
    {
    }

    public Timer(bool playOnStart) : this(0.0f, playOnStart)
    {
    }

    public Timer(float time, bool playOnStart)
    {
        TimeToWait = time;
        PlayOnStart = playOnStart;
        Init();
    }

    #endregion Contructors

    public void SetNewTime(float time)
    {
        TimeToWait = time;
    }

    public void Stop()
    {
        Running = false;
    }

    public void Abandon()
    {
        Running = false;
    }

    public IEnumerator Action()
    {
        OnPlay?.Invoke();
        yield return new WaitForSeconds(TimeToWait);
        OnTimerFinished?.Invoke();
    }
}