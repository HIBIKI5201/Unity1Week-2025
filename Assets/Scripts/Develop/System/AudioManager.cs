using System;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("BGM")]
    [SerializeField] private List<Sound> bgmList = new();

    [Header("SE")]
    [SerializeField] private List<Sound> seList = new();

    private AudioSource _bgmSource;
    private AudioSource _seSource;

    private void Awake()
    {
        _bgmSource = gameObject.AddComponent<AudioSource>();
        _bgmSource.loop = true;
        _bgmSource.playOnAwake = true;

        _seSource = gameObject.AddComponent<AudioSource>();
        _seSource.loop = false;
        _seSource.playOnAwake = false;

        if (bgmList.Count > 0)
        {
            PlayBGM(bgmList[0].name);
        }
    }

    public void PlayBGM(string name)
    {
        var sound = bgmList.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"BGMが見つかりません: {name}");
            return;
        }

        if (_bgmSource.clip == sound.clip)
        {
            return;
        }

        _bgmSource.clip = sound.clip;
        _bgmSource.volume = sound.volume;
        _bgmSource.Play();
    }

    public void StopBGM()
    {
        StopIfPlaying(_bgmSource);
    }

    public void PlaySE(string name)
    {
        var sound = seList.Find(s => s.name == name);
        if (sound == null)
        {
            Debug.LogWarning($"SEが見つかりません: {name}");
            return;
        }

        _seSource.PlayOneShot(sound.clip, sound.volume);
    }

    public void StopAllAudioIfPlaying()
    {
        StopIfPlaying(_bgmSource);
        StopIfPlaying(_seSource);
    }

    private static void StopIfPlaying(AudioSource source)
    {
        if (source != null && source.isPlaying)
        {
            source.Stop();
        }
    }
}
