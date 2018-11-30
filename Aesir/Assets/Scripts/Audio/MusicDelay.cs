using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDelay : MonoBehaviour {

    public AudioSource music;
    public float timer;
	

    private void Update()
    {
        timer -= Time.deltaTime;

        if(timer < 0)
        {
			music.Play();
			Destroy(this);
        }
    }

}
