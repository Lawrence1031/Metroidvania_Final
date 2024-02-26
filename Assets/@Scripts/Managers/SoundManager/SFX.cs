using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;

[RequireComponent(typeof(AudioSource))]
public class SFX : SoundManager<SFX>
{
    #region Set SFX

    public AudioSource AudioSource { get; set; }
    private float volumeScale = 0.2f;
    public float VolumeScale
    {
        get
        {
            return this.volumeScale;
        }
        set
        {
            this.volumeScale = Mathf.Clamp01(value);
            SetVolume(this.volumeScale);
        }
    }

    protected override void Awake()
    {
        base.Awake();
        this.AudioSource = GetComponent<AudioSource>();
        LoadVolumeSettings();
    }

    protected override void SetVolume(float volumeScale)
    {
        SetVolume("SFX", volumeScale);
    }

    #endregion

    private void Start()
    {
        LoadVolumeSettings();
    }

    private void LoadVolumeSettings()
    {
        float savedVolume = PlayerPrefs.GetFloat("SFXVolume",0.5f);
        VolumeScale = savedVolume;
    }

    public void PlayOneShot(AudioClip clip, float volumeScale = 1.0f)
    {
        this.AudioSource.PlayOneShot(clip, volumeScale);
    }

    public void PlayOneShot(string name, float volumeScale = 1.0f)
    {
        AudioClip clip = ResourceManager.Instance.GetAudioClip(name);
        this.AudioSource.PlayOneShot(clip, volumeScale);
    }
}