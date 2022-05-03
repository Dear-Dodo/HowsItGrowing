using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Temporary class to store data for "smallJournal" vs "OnScreenInformation"
/// </summary>
public class TempSettingsContainer : MonoBehaviour
{
    public static bool IsUsingJournal = true;

    public static void SetJournal(bool status)
    {
        IsUsingJournal = status;
    }
}
