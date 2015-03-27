using UnityEngine;
using System.Collections;
using Utils.Audio;

public class RandomSoundSelection : MonoBehaviour 
{
	public AudioClip[] sounds;

	void Awake()
	{
		AudioHelper.MasterVolume = 1.0f;
		AudioHelper.EffectVolume = 1.0f;
		AudioHelper.MusicVolume = 0.6f;
		AudioHelper.VoiceVolume = 1.0f;
	}

	void Update () 
	{
		if( Input.GetKeyDown(KeyCode.Space) )
		{
			PlaySound();
		}
	}

	void PlaySound()
	{
		sounds = ArrayTools.Shuffle< AudioClip >( sounds );
		AudioHelper.PlayClipAtPoint( sounds[0], Vector3.zero, 0.75f, SoundType.Effect );
	}
}
