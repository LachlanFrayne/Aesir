using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLinker : MonoBehaviour
{
    public List<Enemy> GetEnemies()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");

        List<Enemy> m_enemyList = new List<Enemy>();

        foreach (GameObject g in temp)
        {
            m_enemyList.Add(g.GetComponent<Enemy>());
        }

        return m_enemyList;
    }
    public List<GameObject> GetEnemiesObject()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");

        List<GameObject> m_enemyList = new List<GameObject>();

        foreach (GameObject g in temp)
        {
            m_enemyList.Add(g);
        }

        return m_enemyList;
    }

	public Transform GetUI()
	{
		Transform canvas = GameObject.Find("Canvas").transform;

		return canvas;
	}

	public IEnumerator EnemyDecisionManager(Animator animator)
	{
		foreach (Enemy e in GetComponent<Animator>().GetBehaviour<EnemyTurn>().m_enemy)
		{
			e.m_calcTargetDecision.MakeDecision();
			while (e.m_nActionPoints > 0)
			{
				//e.m_inRangeDecision.MakeDecision();
				yield return e.m_inRangeDecision.StartCoroutine(e.m_inRangeDecision.StartDecision());
			}
		}

		animator.SetBool("PlayerTurn", true);
	}
}
