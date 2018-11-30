using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{

	// Use this for initialization
	void Start () {
		Invoke("Credit", 38);
	}
	
	void Credit()
	{
		SceneManager.LoadScene(0);
	}
}
