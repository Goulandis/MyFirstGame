using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngineInternal;

//音效管理脚本
public class AudioManager : BaseManager
{
    private const string sound_Path = "Sounds/";
    public const string sound_Alert = "Alert";
    public const string sound_ArrowShoot = "ArrowShoot";
    public const string sound_BgFast = "Bg(fast)";
    public const string sound_BgModerate = "Bg(moderate)";
    public const string sound_ButtonClick = "ButtonClick";
    public const string sound_Miss = "Miss";
    public const string sound_ShootPerson = "ShootPerson";
    public const string sound_Timer = "Timer";

    private AudioSource bgmAudioSource;//负责背景音乐的声音源组件
    private AudioSource nomalAudioSource;//负责其他音效的声音源组件

    public AudioManager(GameFocade facade) : base(facade)
    {

    }

    public override void OnInit()
    {
        GameObject audioGameObject = new GameObject("AudioGameObject");//创造声音源物体
        //将声音源组件挂载到声音源物体上
        bgmAudioSource = audioGameObject.AddComponent<AudioSource>();
        nomalAudioSource = audioGameObject.AddComponent<AudioSource>();
        //默认播放背景音乐
        PlaySound(bgmAudioSource,LoadSound(sound_BgModerate),0.25f,true);
    }

    //播放声音
    private void PlaySound(AudioSource audioSource, AudioClip audioClip, float volume = 0.5f, bool isLoop = false)
    {
        audioSource.clip = audioClip;//声音源
        audioSource.volume = volume;//音量
        audioSource.loop = isLoop;//循环播放
        audioSource.Play();//播放
    }

    //加载声音源到声音源列表
    private AudioClip LoadSound(string soundName)
    {
        return Resources.Load<AudioClip>(sound_Path + soundName);
    }

    //播放背景音乐
    public void PlayBgmSound(string soundName)
    {
        PlaySound(bgmAudioSource,LoadSound(soundName),0.25f,true);
    }

    //播放其他音效
    public void PlayNomalSound(string soundName)
    {
        PlaySound(nomalAudioSource,LoadSound(soundName),1f);
    }
}
