using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUI : MonoBehaviour {

	public Camera camera;
	public List<GameObject> m_heroes;
	public GameObject uiHealth;

	void Start ()
	{
		camera = Camera.main;
		m_heroes = new List<GameObject>();

		m_heroes.Add(GameObject.Find("Thor"));
		m_heroes.Add(GameObject.Find("Freya"));
		m_heroes.Add(GameObject.Find("Loki"));

		test();
	}
	
	void test()
	{
		foreach (GameObject hero in m_heroes)
		{
			Instantiate(uiHealth, new Vector3(hero.transform.position.x, hero.transform.position.y, hero.transform.position.z), hero.transform.rotation,transform);
		}
	}
	void Update ()
	{
		//transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
		//gameObject.transform.position = new Vector3(target.transform.position.x, target.transform.position.y + 1, target.transform.position.z);
	}
}
