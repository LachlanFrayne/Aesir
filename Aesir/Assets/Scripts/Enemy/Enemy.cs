using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    Grida m_grid;
    public Node m_currentEnemyNode;
    bool turn = false;

    public int m_meleeDamage;
    public int m_rangeDamage;

    void Start()
    {
        m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grida>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 100))
        {
            for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
            {
                for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                {
                    if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile].self)
                    {
                        m_currentEnemyNode = m_grid.boardArray[columnTile, rowTile];
                        m_currentEnemyNode.self.tag = "CurrentEnemyTile";
                        transform.position = new Vector3(m_currentEnemyNode.self.transform.position.x, .5f, m_currentEnemyNode.self.transform.position.z);
                    }
                }
            }
        }
    }

    void Update()
    {
        if(m_nHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
