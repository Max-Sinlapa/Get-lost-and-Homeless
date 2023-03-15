using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "SoundSettings/SoundSettingsPreset", fileName = "SoundSettingsPreset")]
public class SoundSettings : ScriptableObject
{
    public AudioMixer AudioMixer;

    [Header("MasterVolume")]
    public string MasterVolumeName = "MasterVolume";
    [Range(-80, 20)]
    public float MasterVolume;

    [Header("MusicVolume")]
    public string MusicVolumeName = "MusicVolume";
    [Range(-80, 20)]
    public float MusicVolume;

    [Header("SFXVolume")]
    public string SFXVolumeName = "SFXVolume";
    [Range(-80, 20)]
    public float SFXVolume;


}