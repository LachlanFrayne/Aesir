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
}
