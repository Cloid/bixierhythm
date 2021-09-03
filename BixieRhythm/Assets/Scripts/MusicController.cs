using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

using UnityEngine;

public class MusicController : MonoBehaviour
{
    public AudioSource musicAudio;
    public string clipName;

	// Use this for initialization
	void Start ()
    {
        LoadMusic();
	}

    public void PlayAudio()
    {
        if (musicAudio.clip != null)
        {
            musicAudio.Play();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void LoadMusic()
    {
        AudioClip clip = Resources.Load(clipName) as AudioClip;
        if(clip != null)
        {
            this.musicAudio.clip = clip;
        }
    }
}
