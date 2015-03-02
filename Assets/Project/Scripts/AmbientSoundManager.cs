using UnityEngine;
using System.Collections;

public class AmbientSoundManager : MonoBehaviour {

    //public AudioListener ambientSound;

    public AudioClip wind;
    public AudioClip birdSinging;


	// Use this for initialization
	void Start () {

        AudioListener.volume = 0.5f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
