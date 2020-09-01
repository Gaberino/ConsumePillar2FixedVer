using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class SfxManager : Singleton<SfxManager>
{

	public AudioClip moveSound;
	public AudioClip eatSound;
	public AudioClip bumpSound;
	public AudioClip winSound;
	public AudioClip headPopOffSound;
	public AudioClip undoSound;

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

	public void PlayMoveSound()
    {
		TryPlay(moveSound, nameof(moveSound));
	}

	public void PlayEatSound()
	{
		TryPlay(eatSound, nameof(eatSound));
	}

	public void PlayBumpSound()
	{
		TryPlay(bumpSound, nameof(bumpSound));
	}

	public void PlayWinSound()
	{
		TryPlay(winSound, nameof(winSound));
	}

	public void PlayHeadPopOffSound()
	{
		TryPlay(headPopOffSound, nameof(headPopOffSound));
	}

	public void PlayUndoSound()
	{
		TryPlay(undoSound, nameof(undoSound));
	}
}
