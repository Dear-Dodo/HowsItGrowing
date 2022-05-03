using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
/// <summary>
/// The UI managment script
/// </summary>
public class SettingsMenu : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown;    //graphichs quality
   // public TMP_Dropdown resolutionDropdown; //screen resolution

    public AudioMixer audioMixer;           //audio mixer

    public Slider MasterVolume;
    public Slider MusicVolume;
    public Slider SFXVolume;

    Resolution[] resolutions;               //possible resolutions
    bool pause;                             //pause state

    private void Start()
    {
        //gets possible resolutions
        resolutions = Screen.resolutions;
       // resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

       // resolutionDropdown.AddOptions(options);
       // resolutionDropdown.value = currentResolutionIndex;
       // resolutionDropdown.RefreshShownValue();

        //current quality
        qualityDropdown.value = QualitySettings.GetQualityLevel();
        qualityDropdown.RefreshShownValue();

        audioMixer.GetFloat("masterVolume", out float masterValue);
        MasterVolume.value = Mathf.Pow((float)System.Math.E, masterValue / 20);

        audioMixer.GetFloat("musicVolume", out float musicValue);
        MusicVolume.value = Mathf.Pow((float)System.Math.E, musicValue / 20);

        audioMixer.GetFloat("sfxVolume", out float sfxValue);
        SFXVolume.value = Mathf.Pow((float)System.Math.E, sfxValue / 20);
    }

    /// <summary>
    /// quits the game
    /// </summary>
    public void quit()
    {
        Application.Quit();
    }

    /// <summary>
    /// set the game graphics quality
    /// </summary>
    /// <param name="index">graphics quality index</param>
    public void setQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }


    /// <summary>
    /// set fullscreen
    /// </summary>
    /// <param name="isFullscreen">fullscreen state</param>
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    /// <summary>
    /// set game resolution
    /// </summary>
    /// <param name="ResolutionIndex">game resolution</param>
    public void setResolution(int ResolutionIndex)
    {
        Resolution resolution = resolutions[ResolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    public void setMasterVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", Mathf.Log(volume) * 20);
    }

    public void setMusicVolume(float volume)
    {
        audioMixer.SetFloat("musicVolume", Mathf.Log(volume) * 20);
    }

    public void setSFXVolume(float volume)
    {
        audioMixer.SetFloat("sfxVolume", Mathf.Log(volume) * 20);
    }
}
