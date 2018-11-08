using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUI : MonoBehaviour {

	public Camera camera;
	public GameObject target;

	void Start ()
	{
		camera = Camera.main;
		target = GameObject.Find("Thor");
	}
	
	void Update ()
	{
		transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
		gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);
	}
}
