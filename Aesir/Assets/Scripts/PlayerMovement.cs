using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public void dijkstrasSearch(Node startNode, int actionPointAvailable, Material movementHighlight, int MoveCostPerTile)
    {
        int gScore = MoveCostPerTile;
        Heap openList = new Heap();
        List<Node> closedList = new List<Node>();

        //startNode.prev = null;
        openList.Add(startNode);


        while (openList.m_tHeap.Count > 0)
        {
            Node currentNode = openList.Pop();

            closedList.Add(currentNode);

            if (currentNode.gScore > actionPointAvailable)
            {
                continue;
            }
            if (currentNode.self.tag == "CurrentEnemyTile")
                continue;

            currentNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
            currentNode.self.tag = "ThorWalkableTile";

            for (int i = 0; i < currentNode.neighbours.Length; i++)
            {
                if (!closedList.Contains(currentNode.neighbours[i]))
                {
                    if (openList.m_tHeap.Contains(currentNode.neighbours[i]))
                    {
                        int tempGScore = currentNode.gScore + gScore;

                        if (tempGScore < currentNode.neighbours[i].gScore)
                        {
                            currentNode.neighbours[i].prev = currentNode;
                            currentNode.neighbours[i].gScore = tempGScore;
                        }
                    }
                    else
                    {
                        if (currentNode.neighbours[i] != null)
                        {
                            currentNode.neighbours[i].prev = currentNode;
                            currentNode.neighbours[i].gScore = currentNode.gScore + gScore;
                            openList.Add(currentNode.neighbours[i]);
                        }
                    }
                }
            }
        }
    }
}
