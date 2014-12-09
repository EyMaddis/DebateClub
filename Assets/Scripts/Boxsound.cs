using UnityEngine;
using System.Collections;

public class Boxsound : MonoBehaviour {
	public AudioClip clip;

	public void PlayBoxExplosionSound()
	{
		audio.clip = clip;
		audio.Play ();
	}
}
