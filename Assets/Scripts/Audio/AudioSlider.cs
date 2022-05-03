/*
 * Author: Alex Manning
 * Date: 27/10/2021
 * Folder Location: Assets/Scripts/Audio
 */

using UnityEngine;
using UnityEngine.Audio;
using TMPro;

public class AudioSlider : MonoBehaviour
{
    TextMeshProUGUI label;      //Label for the slider
    public AudioMixer mixer;    //Mixer to affect
    
    public void Awake()
    {
        Init();
    }

    /// <summary>
    /// Used to initalise variables
    /// </summary>
    /// <remarks>
    /// as some variables need to be initalised outside of runtime, Start() and Awake() would not be called
    /// </remarks>
    public void Init()
    {
        label = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    /// <summary>
    /// set the mixer volume
    /// </summary>
    /// <param name="volume">mixer volume</param>
    public void SetVolume(float volume)
    {
        mixer.SetFloat("volume", volume);
    }

    /// <summary>
    /// sets the label text
    /// </summary>
    /// <param name="newLabel">text to label the mixer with</param>
    public void SetLabel(string newLabel)
    {
        label.text = newLabel;
    }
}
