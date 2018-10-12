using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDecision : BaseDecision
{
    public int m_nMoveCostPerTile;
    public Thor m_thor;

    public override void MakeDecision()
    {
        Heap openList = new Heap(true);
        List<Node> closedList = new List<Node>();

        Node currentNode = m_self.m_currentEnemyNode;

        openList.Add(currentNode);

        //sort by fscore
        //update current node
        //check for end of path
        //remove current from open
        //add current to closed
        
        //for all neighbours
        //hscore = distance to end node
        //fscore = hscore + gscore
        //neighbour.previous = current node

        while (openList.m_tHeap.Count > 0)      //loop through openlist to get path
        {
            currentNode = openList.Pop();
            
            if (m_thor.m_currentNode == currentNode)
            {
                break;
            }
            if (currentNode.m_gScore > m_self.m_nActionPoints)
            {
                continue;
            }
            if (currentNode.self.tag == "CurrentEnemyTile")
                continue;

            openList.Pop();

            closedList.Add(currentNode);

            foreach(Node n in currentNode.neighbours)
            {
                if (!closedList.Contains(n))
                {
                    if (openList.m_tHeap.Contains(n))
                    {
                        if (n.m_gScore < currentNode.m_gScore + m_nMoveCostPerTile)
                        n.m_gScore = currentNode.m_gScore + m_nMoveCostPerTile;
                        n.m_hScore = Vector3.Distance(currentNode.self.transform.position, m_thor.transform.position);
                        n.m_fScore = n.m_hScore + n.m_gScore;
                        n.prev = currentNode;
                    }
                    else
                    {
                        n.m_gScore = currentNode.m_gScore + m_nMoveCostPerTile;
                        n.m_hScore = Vector3.Distance(currentNode.self.transform.position, m_thor.transform.position);
                        n.m_fScore = n.m_hScore + n.m_gScore;
                        n.prev = currentNode;

                        openList.Add(n);
                    }
                }
                
            }
        }
    }
}
