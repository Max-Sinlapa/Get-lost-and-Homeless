using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{

    [SerializeField] protected SoundSettings m_SoundSettings;

    //public Slider m_SliderMasterVolume;
    public Slider m_SliderMusicVolume;
    public Slider m_SliderSFXVolume;
    public GameObject ObjectMusic;
    private AudioSource AudioSource;
    void Start()
    {
        ObjectMusic = GameObject.FindWithTag("BGM");
        AudioSource = ObjectMusic.GetComponent<AudioSource>();
        InitialiseVolumes();
    }

    private void InitialiseVolumes()
    {
        //SetMasterVolume(m_SoundSettings.MasterVolume);
        SetMusicVolume(m_SoundSettings.MusicVolume);
        SetSFXVolume(m_SoundSettings.SFXVolume);
    }

    /*
    public void SetMasterVolume(float vol)
    {
        //Set float to the audiomixer
        m_SoundSettings.AudioMixer.SetFloat(m_SoundSettings.MasterVolumeName, vol);
        //Set float to the scriptable object to persist the value although the game is closed
        m_SoundSettings.MasterVolume = vol;
        //Set the slider bar's value
        m_SliderMasterVolume.value = m_SoundSettings.MasterVolume;
    }
    */

    public void SetMusicVolume(float vol)
    {
        //Set float to the audiomixer
        m_SoundSettings.AudioMixer.SetFloat(m_SoundSettings.MusicVolumeName, vol);
        //Set float to the scriptable object to persist the value although the game is closed
        m_SoundSettings.MusicVolume = vol;
        //Set the slider bar's value
        m_SliderMusicVolume.value = m_SoundSettings.MusicVolume;

    }
    public void SetSFXVolume(float vol)
    {
        //Set float to the audiomixer
        m_SoundSettings.AudioMixer.SetFloat(m_SoundSettings.SFXVolumeName, vol);
        //Set float to the scriptable object to persist the value although the game is closed
        m_SoundSettings.SFXVolume = vol;
        //Set the slider bar's value
        m_SliderSFXVolume.value = m_SoundSettings.SFXVolume;
    }

}