using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using XInputDotNetPure;

public class Options : MonoBehaviour
{
    public AudioMixerGroup master;
    public AudioMixerGroup sfx;
    public AudioMixerGroup music;

    [SerializeField] Transform optionsGroup;
    [SerializeField] Dropdown qualityDropdown;
    [SerializeField] Dropdown resDropdown;
    [SerializeField] Toggle fullScrToggle;
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Button saveButton;

    float volumeValue = 20f;
    
    List<Resolution> resolutions = new List<Resolution>();
    
    Singleton singleton;

    GamePadState[] states = new GamePadState[4];

    soundManager soundManager;


    void Start ()
    {
        singleton = Singleton.instance;

        soundManager = FindObjectOfType<soundManager>();

        Resolution res1 = new Resolution();
        res1.width = 1920;
        res1.height = 1080;
        res1.refreshRate = 60;

        Resolution res2 = new Resolution();
        res2.width = 1600;
        res2.height = 900;
        res2.refreshRate = 60;

        Resolution res3 = new Resolution();
        res3.width = 1366;
        res3.height = 768;
        res3.refreshRate = 60;

        Resolution res4 = new Resolution();
        res4.width = 1280;
        res4.height = 720;
        res4.refreshRate = 60;

        resolutions.Add(res1);
        resolutions.Add(res2);
        resolutions.Add(res3);
        resolutions.Add(res4);

        List<Dropdown.OptionData> resData = new List<Dropdown.OptionData>();

        foreach (Resolution res in resolutions)
        {
            Dropdown.OptionData data = new Dropdown.OptionData(res.width + " x " + res.height);
            resData.Add(data);
        }

        resDropdown.AddOptions(resData);

        singleton.LoadOptions();
        ResetOptions();
        SaveOptions();
        
        saveButton.gameObject.SetActive(false);
	}


    void Update ()
    {
        // Gets states for all game pads
        for (int i = 0; i < states.Length; i++)
        {
            states[i] = GamePad.GetState((PlayerIndex)i);

            if (saveButton.gameObject.activeSelf && states[i].Buttons.Y == ButtonState.Pressed)
            {
                SaveOptions();
            }
        }
    }


    public void SaveOptions()
    {
        // Stores slider, dropdown and toggle values in singleton
        singleton.qualityIndex = qualityDropdown.value;
        singleton.resIndex = resDropdown.value;
        singleton.fullscreen = fullScrToggle.isOn;
        singleton.masterVol = masterSlider.value;
        singleton.sfxVol = sfxSlider.value;
        singleton.musicVol = musicSlider.value;

        // Saves options in a savefile
        singleton.SaveOptions();

        saveButton.gameObject.SetActive(false);

        EventSystem.current.SetSelectedGameObject(optionsGroup.GetComponentInChildren<Selectable>().gameObject);
    }


    public void ChangeQuality()
    {
        QualitySettings.SetQualityLevel(qualityDropdown.value, true);

        saveButton.gameObject.SetActive(true);

        soundManager.PlaySound("ButtonClick");
    }


    public void ChangeResolution()
    {
        Resolution res = resolutions[resDropdown.value];
        Screen.SetResolution(res.width, res.height, fullScrToggle.isOn);

        saveButton.gameObject.SetActive(true);

        soundManager.PlaySound("ButtonClick");
    }


    public void ChangeFullScreen()
    {
        Screen.fullScreen = fullScrToggle.isOn;

        saveButton.gameObject.SetActive(true);

        soundManager.PlaySound("ButtonClick");
    }


    public void ChangeMaster()
    {
        master.audioMixer.SetFloat("MasterVolume", CalculateMixerVol(masterSlider.value));

        saveButton.gameObject.SetActive(true);

        soundManager.PlaySound("ButtonClick");
    }


    public void ChangeSfx()
    {
        sfx.audioMixer.SetFloat("SFXVolume", CalculateMixerVol(sfxSlider.value));

        saveButton.gameObject.SetActive(true);

        soundManager.PlaySound("ButtonClick");
    }


    public void ChangeMusic()
    {
        music.audioMixer.SetFloat("MusicVolume", CalculateMixerVol(musicSlider.value));

        saveButton.gameObject.SetActive(true);
    }


    public void ResetOptions()
    {
        if (singleton.OptionsFileExists())
        {
            // Sets video and volume settings to mathc the values in the save file
            Screen.SetResolution(resolutions[singleton.resIndex].width, resolutions[singleton.resIndex].height, singleton.fullscreen);
            QualitySettings.SetQualityLevel(qualityDropdown.value, true);
            master.audioMixer.SetFloat("MasterVolume", CalculateMixerVol(singleton.masterVol));
            sfx.audioMixer.SetFloat("SFXVolume", CalculateMixerVol(singleton.sfxVol));
            music.audioMixer.SetFloat("MusicVolume", CalculateMixerVol(singleton.musicVol));

            // Sets options UI to match the values in save file
            int index = resolutions.FindIndex(x => x.height == resolutions[singleton.resIndex].height && x.width == resolutions[singleton.resIndex].width);
            resDropdown.value = index;
            qualityDropdown.value = singleton.qualityIndex;
            fullScrToggle.isOn = singleton.fullscreen;
            masterSlider.value = singleton.masterVol;
            sfxSlider.value = singleton.sfxVol;
            musicSlider.value = singleton.musicVol;

            resDropdown.RefreshShownValue();
            qualityDropdown.RefreshShownValue();
        }
        else
        {
            // Sets options UI to match the current settings values
            int index = resolutions.FindIndex(x => x.height == Screen.currentResolution.height && x.width == Screen.currentResolution.width);
            resDropdown.value = index;
            qualityDropdown.value = QualitySettings.GetQualityLevel();
            fullScrToggle.isOn = Screen.fullScreen;
            master.audioMixer.SetFloat("MasterVolume", CalculateMixerVol(masterSlider.value));
            sfx.audioMixer.SetFloat("SFXVolume", CalculateMixerVol(sfxSlider.value));
            music.audioMixer.SetFloat("MusicVolume", CalculateMixerVol(musicSlider.value));

            qualityDropdown.RefreshShownValue();
            resDropdown.RefreshShownValue();
        }
    }


    float CalculateMixerVol(float value)
    {
        if (value == 0f)
            return -80f;
        else
            return value * volumeValue - volumeValue;
    }
}
