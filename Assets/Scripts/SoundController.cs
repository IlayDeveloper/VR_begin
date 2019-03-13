using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {

	public Station state = Station.Play;
	public enum Station
	{
		Play,
		Stop
	}

	public bool canInteract = false;

	public AudioClip button;

	public void PlayOrStop ()
	{
		if (this.canInteract)
		{
			Debug.Log("Play");
			AudioSource.PlayClipAtPoint(this.button, transform.position);
		}	
	}

	public void PlayNext ()
	{
		if (this.canInteract)
		{
			Debug.Log("Next");
			AudioSource.PlayClipAtPoint(this.button, transform.position);
		}
	}
}
