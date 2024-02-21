using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]

public class BGM : SoundManager<BGM>
{
    #region Set BGM

    public AudioSource AudioSource { get; set; }
    private float volumeScale = 1.0f;
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

    protected override void SetVolume(float volumeScale)
    {
        SetVolume("BGM", volumeScale);
    }

    #endregion

    //예시
    [Header("Game")]
    public AudioClip bgm; //예를 들어 시작 브금


    private void Start()
    {
        Play(bgm, true); //시작 브금 스타트
    }

    public void Play(AudioClip clip, bool isLoop)
    {
        this.AudioSource.loop = isLoop;
        this.AudioSource.clip = clip;
        this.AudioSource.Play();
    }

    public void Stop()
    {
        this.AudioSource.Stop();
    }
}
