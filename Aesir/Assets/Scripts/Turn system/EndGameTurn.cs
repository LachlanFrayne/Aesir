using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameTurn : MonoBehaviour {

    public List<GameObject> m_enemies;

    public List<GameObject> m_heroes;

	public Node m_exitNode;

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
		if (m_heroes.Count < 3)
		{
			SceneManager.LoadScene(0);
			Debug.Log("thanks for playin");
		}

		if (m_exitNode.contain)
		{
			if (m_enemies.Count <= 0 && m_exitNode.contain.GetComponent<Hero>() != null)
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
				Debug.Log("thanks for playin");
			}
		}
	}
}
