using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pixelplacement;

public class SfxManager : Singleton<SfxManager>
{

	public enum EatSoundStyle
    {
		Wet,
		Dry,
    }

	public AudioClip moveSound;
	public AudioClip eatWetSound;
	public AudioClip eatDrySound;
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

	public void PlayEatSound(EatSoundStyle style)
	{
		switch (style)
        {
			case EatSoundStyle.Wet:
				TryPlay(eatWetSound, nameof(eatWetSound));
				break;
			case EatSoundStyle.Dry:
				TryPlay(eatDrySound, nameof(eatDrySound));
				break;
		}
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
