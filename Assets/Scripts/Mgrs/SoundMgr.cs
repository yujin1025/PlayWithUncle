using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundMgr : SingletonBehaviour<SoundMgr>
{
    // 음악 기본 볼륨
    const float BGMVOL = 100;
    const float EFFECTVOL = 100;

    // 사운드 소스
    [SerializeField] AudioSource bgmSource;   // 배경음악
    [SerializeField] AudioSource effectSource;    // 효과음악

    // 배경음악 오디오 클립
    [SerializeField] AudioClip mainBGM;
    [SerializeField] AudioClip ingameBGM;
    [SerializeField] AudioClip endingBGM;

    // 효과음악 오디오 클립
    [SerializeField] AudioClip clickEffect;


    private new void Awake()
    {
        base.Awake();
    }

    void Start()
    {
        bgmSource.clip = mainBGM;
        bgmSource.loop = true;

        effectSource.clip = clickEffect;
        effectSource.loop = false;

        MainBGMPlay();
    }

    public void MainBGMPlay()
    {
        bgmSource.clip = mainBGM;       
        bgmSource.Play();       
    }    

    public void ClickEffectPlay()
    {
        effectSource.clip = clickEffect;
        effectSource.Play();
    }


    public void MuteOrPlayAllSound()
    {
        if(bgmSource.volume == 0)
        {
            bgmSource.volume = BGMVOL;
            effectSource.volume = EFFECTVOL;
        }
        else
        {
            bgmSource.volume = 0;
            effectSource.volume = 0;
        }       
    }
}
