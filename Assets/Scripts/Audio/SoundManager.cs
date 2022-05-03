/*
 * Author: Ronan Richardson
 * Date: 29/10/2021
 * Folder Location: Assets/Scripts/Audio
 */

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

// Disable the "never assigned to, default value of null" warning as it is bugged (all members are assigned a value)
#pragma warning disable 0649

/// <summary>
/// <para>
/// SoundManager is a singleton manager class that is used to collate and keep track of all SFX
/// clips used within the game as Sound Objects. It is the central interface for playing these
/// SFX clips. On Awake() the class will collect and store all pre-created Sound Object assets
/// in an internal dictionary keyed by the string name of it's corresponding audio clip (for this
/// reason each Sound must hold a unique audio clip), and will generate default Sound Objects for
/// any audio clips in the assets folder that do not have corresponding Sounds for them.
/// </para>
/// <para>
/// <b>To Function: it is important that all Audio Clips used in the game are stored in Assets/Resources/Audio/SFX,
/// and that all Sound Object assets you create are stored in Assets/Resources/Audio/Sounds, if either of these
/// paths are not adhered to, the manager will not function as intended.</b>
/// </para>
/// <b>How to Use: All you need to play any SFX within the game is a reference to an instantiation of
/// this Sound Manager, and the name of the audio clip you wish to play. With that, simply call
/// soundManager.Play(audioClipName). There are also functions for stopping and getting sounds which
/// use the same string input.</b>
/// </summary>
/// 
public class SoundManager : Singleton<SoundManager>
{
    // A dictionary of every sound in the game, keyed by their audio clip's name
    private Dictionary<string, Sound> m_sounds;
    // An audio source prefab used to instantiate audio sources for each sound object at runtime
    [SerializeField] private AudioSource m_audioSourcePrefab;
    // The desired mixer group to play the sounds through (should be set to SFX)
    [SerializeField] private AudioMixerGroup m_audioMixerGroup;

    /// <summary>
    /// Awake() first ensures that only one instance of SoundManager ever exists by enforcing a singleton pattern,
    /// and will then ensure that this gameObject is not destroyed on load of it is the original instance of SoundManager.
    /// Then, the Awake() function goes through all audio clips stored in Assets/Resources/Audio/SFX and makes sure that
    /// each clip has a Sound object generated for it, and generates them for clips that are missing Sounds. These Sounds
    /// are then stored in the m_sounds dictionary for fast look up, keyed using the string name of each Sounds audio clip.
    /// </summary>
    public override void Awake()
    {
        // Singleton pattern, ensure that only one instance of Sound Manager ever exists
        base.Awake();

        // First make sure that the audio source prefab has been properly attached in the inspector
        if (Instance.m_audioSourcePrefab == null)
        {
            throw new ArgumentNullException("Audio Source prefab is missing! Please attach it in the inspector.");
        }

        // Initialise the sounds dictionary
        Instance.m_sounds = new Dictionary<string, Sound>();

        // Add all sounds that have been created as assets prior to runtime to the m_sounds dict
        Instance.AddExistingSounds();
        // Create and add any Sounds for audio clips that don't already have Sounds for them
        Instance.CreateMissingSounds();
    }

    /// <summary>
    /// <para>
    /// AddExistingSounds() simply collates all the user-generated sounds that have been created prior to runtime,
    /// and adds them each to the m_sounds dictionary, using the audio clip name as their key.
    /// </para>
    /// <b>Please Note: This function uses Resources.LoadAll() to load sounds, and assumes that all sounds in the
    /// project are stored within Assets/Resources/Audio/Sounds.</b>
    /// </summary>
    private void AddExistingSounds()
    {
        // Load all the pre-created sound scriptables in the project
        List<Sound> precreatedSounds = new List<Sound>(Resources.LoadAll<Sound>("Audio/Sounds"));
        // Foreach pre-created Sound Object, instantiate an audio source for it, initialise it and add it to the m_sounds dictionary
        foreach (var sound in precreatedSounds)
        {
            sound.Init(Instantiate(Instance.m_audioSourcePrefab, Instance.transform));
            Instance.m_sounds.TryAdd(sound.m_audioClip.name, sound);
        }
    }

  

    /// <summary>
    /// <para>
    /// CreateMissingSounds() loads all of the audio clips stored in the project, and goes through them to check
    /// if any are missing Sounds and have not been entered into the m_sounds dictionary. If any clip is missing a
    /// sound, the function will create an instance of a new sound and call Init(audioSource, audioClip) on it, which
    /// sets all of the Sound's audio source wrapper variables to their default values. The function makes sure that
    /// any new sounds generated for audio clips are added to the m_sounds dictionary, using the audio clip's name
    /// as it's key.
    /// </para>
    /// <b>Please Note: This function uses Resources.LoadAll() to load audio clips, and assumes that all clips in the
    /// project are stored within Assets/Resources/Audio/SFX.</b>
    /// </summary>
    private void CreateMissingSounds()
    {
        // Load all the audio clips in the project and store them
        AudioClip[] audioClips = Resources.LoadAll<AudioClip>("Audio/SFX");

        // Loop through all audio clips stored in assets and check for any that don't have sounds pre-created for them
        foreach (var clip in audioClips)
        {
            // If the clip does not already have a sound in the m_sounds dictionary, create a new Sound instance for it and initialise it with default values
            if (!Instance.m_sounds.TryGetValue(clip.name, out Sound placeholder))
            {
                Sound newSound = ScriptableObject.CreateInstance<Sound>();
                newSound.Init(Instantiate(Instance.m_audioSourcePrefab, Instance.transform), clip, m_audioMixerGroup);
                // Add the runtime generated sound to the m_sounds dict
                Instance.m_sounds.TryAdd(newSound.m_audioClip.name, newSound);
            }
        }
    }

    /// <summary>
    /// Play() takes an input of a string denoting the name of the desired audio clip to play, and will search through the dictionary m_sounds
    /// of Sound objects to find and play the Sound with the the matching audio clip.
    /// </summary>
    /// <param name="audioName">The name of the desired audio clip to play.</param>
    public void Play(string audioName)
    {
        Sound sound;
        // If audioName is a valid key in the dict, TryGetValue will return true and set sound with the matching dict value
        if (Instance.m_sounds.TryGetValue(audioName, out sound))
        {
            sound.Play();
        }
    }

    /// <summary>
    /// Play() takes an input of the desired audio clip to play, and will search through the dictionary
    /// of Sound objects (m_sounds) to find and play the Sound with the the matching audio clip.
    /// </summary>
    /// <param name="audioClip">The desired audio clip to play.</param>
    public void Play(AudioClip audioClip)
    {
        Sound sound;
        if (Instance.m_sounds.TryGetValue(audioClip.name, out sound))
        {
            sound.Play();
            return;
        }

        // If no Sound is found that matches the passed audio clip, notify in the console that either the clip is passed incorrectly or stored incorrectly
        Debug.LogError("The audio clip you passed has not been generated as a Sound, please recheck that the clip you are passing is correct and that it is stored in the specified filepath.");
    }

    /// <summary>
    /// Stop() takes an input of a string denoting the name of the desired audio clip to stop playing, and will search through the dictionary
    /// of Sound objects (m_sounds) to find and stop the Sound with the the matching audio clip.
    /// </summary>
    /// <param name="audioName">The name of the audio clip corresponding to the desired Sound to stop.</param>
    public void Stop(string audioName)
    {
        Sound sound;
        if (Instance.m_sounds.TryGetValue(audioName, out sound))
        {
            sound.Stop();
            return;
        }

        // If no Sound is found that matches the passed audioName, notify in the console that either the name is incorrect or the clip is stored incorrectly
        Debug.LogError("The audio name you passed could not be found, please recheck that the name you are passing is correct and that the audio clip is stored in the specified filepath.");
    }

    /// <summary>
    /// GetSound() takes an input of a string denoting the name of the audio clip to get the corresponding Sound of,
    /// and will search through the dictionary of Sound objects (m_sounds) to find and return the Sound with the
    /// matching audio clip.
    /// </summary>
    /// <param name="audioName">The name of the audio clip corresponding to the desired Sound to get.</param>
    public Sound GetSound(string audioName)
    {
        Sound sound;
        if (Instance.m_sounds.TryGetValue(audioName, out sound))
        {
            return sound;
        }

        // If no Sound is found that matches the passed audioName, notify in the console that either the name is incorrect or the clip is stored incorrectly
        Debug.LogError("The audio name you passed could not be found, please recheck that the name you are passing is correct and that the audio clip is stored in the specified filepath.");
        return null;
    }
}