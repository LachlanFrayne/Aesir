﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
	public Entity m_targetedHero;

	public Grida m_grid;

	public BridalThor m_thor;
	public Loki m_loki;
	public Freya m_freya;

	public TargetInRangeDecision m_inRangeDecision;
	public CalculateTargetDecision m_calcTargetDecision;

	private Animator m_turnChecker;
	public Material m_enemyRangeHighlightMaterial;

	public bool m_bStunned = false;

	bool m_currentlySelected = false;

	void Start()
	{
		m_thor = GameObject.Find("Thor").GetComponent<BridalThor>();
		m_loki = GameObject.Find("Loki").GetComponent<Loki>();
		m_freya = GameObject.Find("Freya").GetComponent<Freya>();

		m_inRangeDecision = gameObject.GetComponent<TargetInRangeDecision>();
		m_calcTargetDecision = gameObject.GetComponent<CalculateTargetDecision>();
		m_turnChecker = GameObject.Find("TurnManager").GetComponent<Animator>();

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

		///////////////////////////////////////////////////////////enemy attack range visualizer////////////////////////////////////////////////////////////

		if (!m_thor.enabled)
		{
			m_thor = m_thor.gameObject.GetComponent<Thor>();
		}

		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100))
			{
				if (hit.transform.gameObject == this.gameObject && m_currentNode.prev == null)
				{
					//if (m_thor.GetType() == typeof(Thor))
					//{
					//	if (!m_loki.bMove || !m_loki.bBasicAttack || !m_loki.bAbility1Attack ||
					//		!m_freya.bMove || !m_freya.bBasicAttack || !m_freya.bAbility1Attack ||
					//		!m_thor.bMove || !m_thor.bBasicAttack || !m_thor.bAbility1Attack || !((Thor)m_thor).bAbility2Attack)
					//	{
					//		m_currentlySelected = true;
					//	}
					//}
					//else if (!m_loki.bMove || !m_loki.bBasicAttack || !m_loki.bAbility1Attack ||
					//		 !m_freya.bMove || !m_freya.bBasicAttack || !m_freya.bAbility1Attack ||
					//		 !m_thor.bMove || !m_thor.bBasicAttack || !m_thor.bAbility1Attack)
					//{


						m_currentlySelected = true;

					//}
				}
			}
		}
		else
		{
			m_currentlySelected = false;
		}


		if (m_turnChecker.GetCurrentAnimatorStateInfo(0).IsName("PlayerTurn") && m_currentlySelected)
		{
			m_grid.ClearBoardData();

			Heap openList = new Heap(false);
			List<Node> closedList = new List<Node>();

			Node currentNode = m_currentNode;

			openList.Add(currentNode);

			while (openList.m_tHeap.Count > 0)      //loop through openlist to get path
			{
				currentNode = openList.Pop();       //update current node with openlist
													//closedList.Add(currentNode);        //add current node to closed list

				//if (currentNode.contain != gameObject)       //if tile's contain isn't self
				//{
				//	if (currentNode.contain != null)        //skip over tiles with objects in them
				//	{
				//		if (!currentNode.bObstacle)      //if tile isn't an obstacle
				//		{
				//			continue;
				//		}
				//	}
				//}

				if (currentNode.m_gScore >= m_nBasicAttackRange)
				{
					continue;
				}

				foreach (Node n in currentNode.neighbours)
				{
					if (!n)     //if node doesnt exist
					{
						continue;
					}

					if (n.bBlocked || n.contain)        //if the node is blocked
					{
						continue;
					}


					if (openList.m_tHeap.Contains(n))       //if in openlist
					{
						if (currentNode.m_gScore + 1 < n.m_gScore)     //if better path
						{
							n.m_gScore = currentNode.m_gScore + 1;
						}
					}
					else        //if not in closed list
					{
						if (!(openList.m_tHeap.Contains(n)))       //if not in openlist
						{
							n.m_gScore = currentNode.m_gScore + 1;
							n.GetComponent<Renderer>().material = m_enemyRangeHighlightMaterial;
							openList.Add(n);        //add to openlist
						}
					}
				}
			}
		}
	}
}

