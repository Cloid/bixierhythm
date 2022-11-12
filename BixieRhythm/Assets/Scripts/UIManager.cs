using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    //public Slider _musicSlider;
    //public Slider _sfxSlider;

    public static float musicVolume {get; private set;}
    public static float soundFXVolume {get; private set;}

    //change volume for music on slider value change
    public void onMusicSliderVolumeChange(float value)
    {
        musicVolume = value;
        //AudioManager.Instance.UpdateMixerVolume();
    }

    //change volume for sfx on slider value change
    public void onSoundSliderVolumeChange(float value)
    {
        soundFXVolume = value;
    }
}
