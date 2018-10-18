﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDecision : BaseDecision
{
    public int m_nMoveCostPerTile;
    public Thor m_thor;

	public List<Node> m_path;

    public new void Start()
    {
        m_path = new List<Node>();

        GameObject temp = GameObject.Find("Thor");
        m_thor = temp.transform.GetComponent<Thor>();

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
            
            if (m_thor.m_currentNode == currentNode)        //if at end of path
            {
                break;
            }
            if (currentNode.m_gScore > m_self.m_nActionPoints)      //if run out of actionpoints
            {
                continue;
            }



            foreach(Node n in currentNode.neighbours)
            {
                if (!closedList.Contains(n))        //if not in closed list
                {
                    if (openList.m_tHeap.Contains(n))       //if in openlist
                    {
                        if (currentNode.m_gScore + m_self.m_nMovementActionPointCostPerTile < n.m_gScore)     //if better path
                        {
                            n.m_gScore = currentNode.m_gScore + m_self.m_nMovementActionPointCostPerTile;
                            n.m_hScore = Vector3.Distance(currentNode.transform.position, m_thor.transform.position);
                            n.m_fScore = n.m_hScore + n.m_gScore;
                            n.prev = currentNode;
                        }
                    }
					else		//if not in openlist
                    {		//update neighbors info
                        n.m_gScore = currentNode.m_gScore + m_nMoveCostPerTile;
                        n.m_hScore = Vector3.Distance(currentNode.transform.position, m_thor.transform.position);
                        n.m_fScore = n.m_hScore + n.m_gScore;
                        n.prev = currentNode;

                        openList.Add(n);		//add to openlist
                    }
                }
                
            }
        }


		Node currentPathNode = m_thor.m_currentNode.prev;
		while(currentPathNode.prev != null)
		{
			m_path.Add(currentPathNode);
			currentPathNode = currentPathNode.prev;
		}

        if (m_path.Count >= 1)
        {
            //JM:needs improvement, need to update current node.contain
            //m_currentEnemyNode.tag = "Tile";
            m_self.m_currentNode = m_path[m_path.Count - 1];
            //m_currentEnemyNode.tag = "CurrentEnemyTile";
            m_self.m_currentNode.contain = null;
            transform.position = new Vector3(m_path[m_path.Count - 1].transform.position.x, transform.position.y, m_path[m_path.Count - 1].transform.position.z);
            m_self.m_currentNode = m_path[m_path.Count - 1];
            m_self.m_currentNode.contain = this.gameObject;
        }
    }
}