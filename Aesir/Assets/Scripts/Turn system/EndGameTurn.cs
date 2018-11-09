using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTurn : MonoBehaviour {

    public List<GameObject> m_enemies;

    public List<GameObject> m_heroes;

	void Start ()
    {
        m_enemies = GetComponent<EnemyLinker>().GetEnemiesObject();

        m_heroes = new List<GameObject>();
        m_heroes.Add(GameObject.Find("Thor"));
        m_heroes.Add(GameObject.Find("Freya"));
        m_heroes.Add(GameObject.Find("Loki"));
    }
	
	void Update ()
    {
		if (m_heroes.Count <= 0)
		{
			SceneManager.LoadScene(0);
			Debug.Log("thanks for playin");
		}


		if (m_enemies.Count <= 0)
		{
			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
			Debug.Log("thanks for playin");
		}
	}
}
