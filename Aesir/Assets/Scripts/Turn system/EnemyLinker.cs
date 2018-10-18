using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyLinker : MonoBehaviour
{
    List<Enemy> m_enemyList;

    private void Start()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");

        foreach(GameObject g in temp)
        {
            m_enemyList.Add(g.GetComponent<Enemy>());
        }
    }
}
