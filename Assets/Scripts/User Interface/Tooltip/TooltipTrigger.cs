/*
 *  Author: James Greensill
 *  Date:   29/10/2021
 *  Folder Location: Assets/Scripts/User Interface/Tooltip
 */

using System.Collections;
using UnityEngine;

public class TooltipTrigger : MonoBehaviour
{
    /// <summary>
    /// Data used to set the Tooltip.
    /// </summary>
    [Header("Data for tooltip.")]
    [SerializeField] public TooltipData Data;

    /// <summary>
    /// Tooltip delay, this is used to determine how much time passes before the tooltip appears.
    /// </summary>
    [Header("How long before the tooltip appears. (default = 0.5 seconds)")]
    [Range(0, 15)]
    [SerializeField] public float ShowDelay = 0.5f;

    /// <summary>
    /// Show Enumeration.
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowEnumerator()
    {
        yield return new WaitForSeconds(ShowDelay);
        // Shows the tooltip.
        TooltipSystem.Show(Data);
    }

    /// <summary>
    /// Starts the Show Enumeration.
    /// </summary>
    protected void Show()
    {
        StartCoroutine(ShowEnumerator());
    }

    /// <summary>
    /// Stops the Show Enumeration.
    /// </summary>
    protected void Hide()
    {
        StopAllCoroutines();
        // Hides the tooltip.
        TooltipSystem.Hide();
    }
}