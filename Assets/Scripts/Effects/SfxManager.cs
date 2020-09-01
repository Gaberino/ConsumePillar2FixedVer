using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class SfxManager : Singleton<SfxManager>
{

	public AudioClip bleepSound;
	public AudioClip boopSound;

	private AudioSource audioSource;


	void Awake()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void TryPlay(AudioClip sound, string soundName = "<Unknown>")
    {
		if (sound == null)
        {
			Debug.LogWarning($"Sound not initialized: { soundName }");
        }
        else
        {
			audioSource.PlayOneShot(sound);
        }
    }

	public void PlayBleepSound()
    {
		TryPlay(bleepSound, nameof(bleepSound));
	}
	
	public void PlayBoopSound()
	{
		TryPlay(boopSound, nameof(boopSound));
	}
}
