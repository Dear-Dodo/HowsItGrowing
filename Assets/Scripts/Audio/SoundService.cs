/*
 * Author: Ronan Richardson
 * Date: 30/10/2021
 * Folder Location: Assets/Scripts/Audio
 */

using System;
using UnityEngine;

// Disable the "never assigned to, default value of null" warning as it is bugged (all members are assigned a value)
#pragma warning disable 0649

/// <summary>
/// SoundService is simply a proxy class that allows the SoundManager to be accessed in scenes other than the starting
/// scene during development in Unity. Because the SoundManager is a singleton that starts in the starting scene and is
/// carried across during runtime, it's Play() functions can't be assigned to events in the inspector by designers and
/// other programmers during development, so instead a SoundService is placed in every scene other than the starting
/// one, and it is this that is instead dragged onto events and triggers. SoundService simply gets a reference to the
/// SoundManager singleton once it has been loaded into the relevant scene, and has the proxy function Play(), Stop()
/// and GetSound() that simply call those corresponding functions on the soundManagerInstance.
/// </summary>
public class SoundService : MonoBehaviour
{
    // Uses the lazy-initilisation pattern to only get the SoundManager once it's definitely been loaded
    private SoundManager m_soundManagerInstance;

    public SoundManager SoundManagerInstance
    {
        get
        {
            if (m_soundManagerInstance == null)
            {
                m_soundManagerInstance = FindObjectOfType<SoundManager>();
            }
            return m_soundManagerInstance;
        }
    }

    /// <summary>
    /// Play() is simply a proxy function that attempts to call the SoundManager's Play(string) function.
    /// </summary>
    /// <param name="audioName">The string name of the desired audio clip to play.</param>
    public void Play(string audioName)
    {
        try
        {
            SoundManagerInstance.Play(audioName);
        }
        catch (NullReferenceException e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// Play() is simply a proxy function that attempts to call the SoundManager's Play(AudioClip) function.
    /// </summary>
    /// <param name="audioName">The AudioClip to play.</param>
    public void Play(AudioClip audioClip)
    {
        try
        {
            if (SoundManagerInstance != null)
                SoundManagerInstance.Play(audioClip);
        }
        catch (NullReferenceException e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// Stop() is simply a proxy function that attempts to call the SoundManager's Stop(string) function.
    /// </summary>
    /// <param name="audioName">The string name of the desired audio clip to stop.</param>
    public void Stop(string audioName)
    {
        try
        {
            SoundManagerInstance.Stop(audioName);
        }
        catch (NullReferenceException e)
        {
            Debug.LogError(e);
        }
    }

    /// <summary>
    /// GetSound() is simply a proxy function that attempts to call the SoundManager's GetSound() function.
    /// </summary>
    /// <param name="audioName">The string name of the desired audio clip to find in the SoundManager.</param>
    /// <returns>A reference to the desired Sound if found.</returns>
    public Sound GetSound(string audioName)
    {
        return SoundManagerInstance.GetSound(audioName);
    }
}