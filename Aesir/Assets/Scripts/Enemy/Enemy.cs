using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Grida m_grid;
    public Node m_currentEnemyNode;
    bool turn = false;

    public int m_meleeDamage;
    public int m_rangeDamage;

	MoveDecision m_move;

    void Start()
    {
		
		m_move = gameObject.AddComponent<MoveDecision>();

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

		if (Input.GetKeyDown(KeyCode.P))
		{
			m_move.MakeDecision();

			if(m_move.m_path.Count >= 1)
			{
			m_currentEnemyNode.self.tag = "Tile";
			m_currentEnemyNode = m_move.m_path[m_move.m_path.Count - 1];
			m_currentEnemyNode.self.tag = "CurrentEnemyTile";
			transform.position = new Vector3( m_move.m_path[m_move.m_path.Count - 1].self.transform.position.x, transform.position.y, transform.position.z);
			transform.position = new Vector3(transform.position.x, transform.position.y, m_move.m_path[m_move.m_path.Count - 1].self.transform.position.z);
			}
		}
    }
}
