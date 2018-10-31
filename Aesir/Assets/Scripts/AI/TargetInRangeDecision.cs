using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetInRangeDecision : ABDecision
{
	public List<Node> m_path;

	public new void Start()
	{
		A = GetComponent<AttackDecision>();

		m_path = new List<Node>();

		base.Start();
	}

	public override void MakeDecision()
    {
		m_path.Clear();
		m_self.m_grid.ClearBoardData();

		Heap openList = new Heap(true);
		List<Node> closedList = new List<Node>();

		Node currentNode = m_self.m_currentNode;

		openList.Add(currentNode);

		while (openList.m_tHeap.Count > 0)      //loop through openlist to get path
		{
			currentNode = openList.Pop();       //update current node with openlist
			closedList.Add(currentNode);        //add current node to closed list

			if (m_self.m_targetedHero.m_currentNode == currentNode)        //if at end of path
			{
				break;
			}

			if (currentNode.contain != m_self.gameObject)		//if tile's contain isn't self
			{
				if (currentNode.contain != null)        //skip over tiles with objects in them
				{
					if (currentNode.contain.tag != "Obstacle")      //if tile isn't an obstacle
					{
						continue;
					}
				}
			}

			foreach (Node n in currentNode.neighbours)
			{
				if (!closedList.Contains(n))        //if not in closed list
				{
					if (openList.m_tHeap.Contains(n))       //if in openlist
					{
						if (currentNode.m_gScore + m_self.m_nMovementActionPointCostPerTile < n.m_gScore)     //if better path
						{
							n.m_gScore = currentNode.m_gScore + m_self.m_nMovementActionPointCostPerTile;
							n.m_hScore = Vector3.Distance(currentNode.transform.position, m_self.m_targetedHero.transform.position);
							n.m_fScore = n.m_hScore + n.m_gScore;
							n.prev = currentNode;
						}
					}
					else        //if not in openlist
					{       //update neighbors info
						n.m_gScore = currentNode.m_gScore + m_self.m_nMovementActionPointCostPerTile;
						n.m_hScore = Vector3.Distance(currentNode.transform.position, m_self.m_targetedHero.transform.position);
						n.m_fScore = n.m_hScore + n.m_gScore;
						n.prev = currentNode;

						openList.Add(n);        //add to openlist
					}
				}
			}
		}


		Node currentPathNode = m_self.m_targetedHero.m_currentNode.prev;
		while (currentPathNode.prev != null)
		{
			m_path.Add(currentPathNode);
			currentPathNode = currentPathNode.prev;
		}

		if (m_self.m_targetedHero.m_currentNode.m_gScore <= m_self.m_nBasicAttackRange)		//hero is in enemy's range 
		{
			((AttackDecision)A).m_path = m_path;
			A.MakeDecision();
			m_self.m_grid.ClearBoardData();
		}
		else
		{
			B.MakeDecision();
		}
	}
}
