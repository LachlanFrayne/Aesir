using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDelay : MonoBehaviour {

    public AudioSource music;
    float timer;
	void Start () {

        music.enabled = false;
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(timer > 15)
        {
            music.enabled = true;
        }
    }

}
