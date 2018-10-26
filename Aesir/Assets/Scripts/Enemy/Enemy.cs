using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
	public Hero m_targetedHero;

    public Grida m_grid;

	public TargetInRangeDecision m_inRangeDecision;
	public CalculateTargetDecision m_calcTargetDecision;


	public bool bStunned = false;
    void Start()
    {
		
		m_inRangeDecision = gameObject.GetComponent<TargetInRangeDecision>();
		m_calcTargetDecision = gameObject.GetComponent<CalculateTargetDecision>();

		m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grida>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 100))
        {
            for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
            {
                for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                {
                    if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile])
                    {
                        m_currentNode = m_grid.nodeBoardArray[columnTile, rowTile];
                        m_currentNode.contain = this.gameObject;
                        transform.position = new Vector3(m_currentNode.transform.position.x, 0.0f, m_currentNode.transform.position.z);
                    }
                }
            }
        }
    }

    void Update()
    {
		if (m_nHealth <= 0)
		{
			GameObject.Find("TurnManager").GetComponent<EndGameTurn>().m_enemies.Remove(this.gameObject);
			Destroy(this.gameObject);
		}
    }
}
