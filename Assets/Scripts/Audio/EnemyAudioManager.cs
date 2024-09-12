using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioManager : MonoBehaviour
{
    public static EnemyAudioManager Instance;


    [Header("Sound")]
    public List<AudioClip> soundEffects;
    public List<AudioClip> oneShotSoundEffects; 

    private AudioSource musicSource;
    private List<AudioSource> sfxSources;
    private AudioSource oneShotSource; 
    private Dictionary<string, AudioClip> sfxDictionary;
    private Dictionary<string, AudioClip> oneShotSfxDictionary;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return; 
        }
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.playOnAwake = false; 
        musicSource.volume = 1.0f;
        sfxSources = new List<AudioSource>();
        for (int i = 0; i < 10; i++)
        {
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSources.Add(sfxSource);
        }
        oneShotSource = gameObject.AddComponent<AudioSource>();
        sfxDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in soundEffects)
        {
            sfxDictionary[clip.name] = clip;
        }
        oneShotSfxDictionary = new Dictionary<string, AudioClip>();
        foreach (AudioClip clip in oneShotSoundEffects)
        {
            oneShotSfxDictionary[clip.name] = clip;
        }

    }
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip == null)
        {
            return;
        }

        musicSource.clip = musicClip;
        musicSource.Play();
    }
    public void PlaySoundEffect(string soundName)
    {
        if (sfxDictionary.ContainsKey(soundName))
        {
            foreach (var sfxSource in sfxSources)
            {
                if (!sfxSource.isPlaying)
                {
                    sfxSource.PlayOneShot(sfxDictionary[soundName]);
                    return;
                }
            }
            sfxSources[0].Stop();
            sfxSources[0].PlayOneShot(sfxDictionary[soundName]);
        }
    }
    public void PlayOneShotSoundEffect(string soundName)
    {
        if (oneShotSfxDictionary.ContainsKey(soundName))
        {
            if (oneShotSource.isPlaying)
            {
                oneShotSource.Stop();
            }
            oneShotSource.PlayOneShot(oneShotSfxDictionary[soundName]);
        }
    }
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }
    public void SetSFXVolume(float volume)
    {
        foreach (var sfxSource in sfxSources)
        {
            sfxSource.volume = volume;
        }
    }
    public void StopMusic()
    {
        musicSource.Stop();
    }
    public void StopSoundEffects()
    {
        foreach (var sfxSource in sfxSources)
        {
            sfxSource.Stop();
        }
    }
    public void StopOneShotSoundEffect()
    {
        oneShotSource.Stop();
    }
}
