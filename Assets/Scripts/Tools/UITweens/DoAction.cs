/*
 *  Author: James Greensill
 *  Folder Location: Assets/Scripts/UITweens
 */

using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/// <summary>
/// Base Tween action class. Implements base functionality.
/// </summary>
public abstract class DoAction : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
{
    [SerializeField] public bool PlayOnEnable;
    [SerializeField] public bool PlayOnMouseEnter;
    [SerializeField] public bool ReturnToNormalOnMouseExit;
    [SerializeField] public bool ReturnToNormalOnClick = true;

    [SerializeField] public AudioClip HoverAudioClip;
    [SerializeField] public AudioClip TweenCompleteClip;

    [SerializeField] public UnityEvent OnTweenComplete;

    public void OnPointerExit(PointerEventData eventData)
    {
        if (ReturnToNormalOnMouseExit)
            Restore();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (PlayOnMouseEnter)
            Run();
        if (HoverAudioClip)
            SoundManager.Instance.Play(HoverAudioClip);
    }

    private void OnEnable()
    {
        if (PlayOnEnable)
            Run();
    }

    public abstract void Restore();

    public abstract void Run();
}