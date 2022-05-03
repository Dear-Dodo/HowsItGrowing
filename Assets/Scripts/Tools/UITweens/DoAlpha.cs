/*
 *  Author: James Greensill
 *  Folder Location: Assets/Scripts/Tools/UITweens
 */

using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Interpolates an Images Alpha over time, with optional callback
/// </summary>
public class DoAlpha : DoAction
{
    [Header("Target Alpha will be interpolated towards by Speed.")] [SerializeField]
    public float TargetAlpha;

    [SerializeField] public float Speed;

    [Tooltip("If this is field is left blank, it will attempt to locate an Image component using GetComponent<TargetImage>().")]
    [Header("Target Graphic")]
    [SerializeField]
    public Image TargetImage;

    private void Awake()
    {
        if (TargetImage == null)
        {
            if (GetComponent<Image>() == null)
                this.enabled = false;
            TargetImage = this.GetComponent<Image>();
        }
    }
    
    // Not Implemented.
    public override void Restore()
    {
        throw new System.NotImplementedException();
    }

    public override void Run() => Run(TargetAlpha, true, null);

    public void Run(float target) => Run(target, true, null);

    /// <summary>
    /// Interpolates current alpha to end alpha over speed.
    /// </summary>
    /// <param name="endAlpha">Target Alpha</param>
    /// <param name="endCallback">End Callback Flag</param>
    /// <param name="action">End Callback</param>
    public void Run(float endAlpha, bool endCallback, UnityAction action)
    {
        if (TargetImage == null)
            return;

        var m_Color = TargetImage.color;
        DOTween.To(() => m_Color.a, x => m_Color.a = x, endAlpha, Speed)
            .OnUpdate(() => { TargetImage.color = m_Color; })
            .OnComplete(() =>
            {
                if (endCallback)
                {
                    OnTweenComplete?.Invoke();
                }

                action?.Invoke();
            });
    }
}