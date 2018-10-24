using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour {

    public List<GameObject> m_enemies;

    public List<GameObject> heroes; 

	// Use this for initialization
	void Start ()
    {
        m_enemies = GetComponent<EnemyLinker>().GetEnemiesObject();

        heroes = new List<GameObject>();
        heroes.Add(GameObject.Find("Thor"));
        heroes.Add(GameObject.Find("Freya"));
        heroes.Add(GameObject.Find("Loki"));
    }
	
	// Update is called once per frame
	void Update ()
    {
		if (m_enemies.Count <= 0)
        {
            Application.Quit();
            Debug.Log("thanks for playin");
        }
        if (heroes.Count <= 0)
        {
            Application.Quit();
            Debug.Log("thanks for playin");
        }
	}
}
