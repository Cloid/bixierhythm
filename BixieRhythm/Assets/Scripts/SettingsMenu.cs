using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    
    public AudioMixer audioMixerMain;
    public AudioMixer audioMixerSFX;

    public void setMusicVolume(float volume)
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
}
