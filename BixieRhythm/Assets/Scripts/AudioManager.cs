using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;


public class AudioManager: MonoBehaviour
{
    public static AudioManager instance = null;

    public AudioClip[] musicList;
    public AudioClip[] sfxList;

    public AudioMixerGroup musicMixerGroup;
    public AudioMixerGroup soundMixerGroup;

    public AudioSource musicSource;
    public AudioSource soundSource;

    // Initialize the singleton instance.
	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		//If instance exists, destroy this object is for enforce singleton. 
		else if (instance != this)
		{
			Destroy(gameObject);
            return;
		}
		//Set AudioManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
		DontDestroyOnLoad(gameObject);
	}

    // Play a single clip through the sound effects source.
	public void PlaySound(AudioSource sound)
	{

        if(sound == null)
        {
            Debug.Log("SoundFX Source cannot be found.");
        }
        else
        {
            //soundSource.clip = sfx.clip;
		    soundSource.Play();
        }
	}
	// Play a single clip through the music source.
	public void PlayMusic(AudioSource music)
	{

        if(music == null)
        {
            Debug.Log("Music Source cannot be found.");
        }
        else
        {
            //musicSource.clip = music;
		    musicSource.Play();
        }
	}

    private void Start()
    {
       //    
    }

    public void UpdateMixerVolume()
    {
        musicMixerGroup.audioMixer.SetFloat("MusicMixerVolume", Mathf.Log10(UIManager.musicVolume) * 20);
        soundMixerGroup.audioMixer.SetFloat("SoundFXMixerVolume", Mathf.Log10(UIManager.soundFXVolume) * 20);

    }

    /*public void setMusicVolume(float volume)
    {
        audioMixerMain.SetFloat("Volume", volume);
        //audioMixerSFX.SetFloat("Volume", volume);
        if(volume >-49){
            audioMixerMain.SetFloat("Volume", volume);

        }
        else if(volume <= -50){
            audioMixerMain.SetFloat("Volume", -80);
        }
        
        //Debug.Log(volume);
    }

    public void setSFXVolume(float volumeSFX)
    {
        audioMixerSFX.SetFloat("Volume", volumeSFX);
        
        if(volumeSFX >-40){
            audioMixerSFX.SetFloat("Volume", volumeSFX);

        }
        else if(volumeSFX <= -40){
            audioMixerSFX.SetFloat("Volume", -80);
        }
    }
    */
    
}
