/*
 *  Author: James Greensill
 *  Date:   29/10/2021
 *  Folder Location: Assets/Scripts/User Interface/Tooltip
 */

using UnityEngine;

[System.Serializable]
public struct TooltipData
{
    // Title of the Tooltip Box.
    public string Header;

    // Content of the Tooltip Box.
    public string Content;
}

public class TooltipSystem : Singleton<TooltipSystem>
{
    // Singleton reference.
    // Tooltip Object (template).
    [Header("Tooltip Template.")]
    public Tooltip TooltipObject;

    /// <summary>
    /// Unity's Default Awake: Used for singleton.
    /// </summary>

    /// <summary>
    /// Sets the Tooltip object to active and sets the Data using the parsed data variable.
    /// </summary>
    /// <param name="data">Holds tooltip Data.</param>
    public static void Show(TooltipData data)
    {
        if (Instance == null || Instance.TooltipObject == null)
            return;
        // Sets the tooltip object to active.
        Instance.TooltipObject.gameObject.SetActive(true);
        // Sets the tooltip data with parsed data variable.
        Instance.TooltipObject.Set(data);

        Instance.TooltipObject.GetComponent<DoScale>().Run();
    }

    /// <summary>
    /// Hides the Tooltip Object.
    /// </summary>
    public static void Hide()
    {
        // Checks for null reference errors.
        if (Instance == null || Instance.TooltipObject == null)
            return;
        // Sets the tooltip object to inactive.

        Instance.TooltipObject.GetComponent<DoScale>().Run(new Vector3(0, 0, 0), .5f, () =>
        {
            Instance.TooltipObject.gameObject.SetActive(false);
        });
    }
}