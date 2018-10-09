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

            if (!closedList.Contains(currentNode.left))
            {
                if (openList.m_tHeap.Contains(currentNode.left))
                {
                    int tempGScore = currentNode.gScore + gScore;

                    if (tempGScore < currentNode.left.gScore)
                    {
                        currentNode.left.prev = currentNode;
                        currentNode.left.gScore = tempGScore;
                    }
                }
                else
                {
                    if(currentNode.left != null)
                    {
                        currentNode.left.prev = currentNode;
                        currentNode.left.gScore = currentNode.gScore + gScore;
                        openList.Add(currentNode.left);
                    }
                    
                }
            }

            if (!closedList.Contains(currentNode.right))
            {
                if (openList.m_tHeap.Contains(currentNode.right))
                {
                    int tempGScore = currentNode.gScore + gScore;

                    if (tempGScore < currentNode.right.gScore)
                    {
                        currentNode.right.prev = currentNode;

                        currentNode.right.gScore = tempGScore;
                    }
                }
                else
                {
                    if (currentNode.right != null)
                    {
                        currentNode.right.prev = currentNode;
                        currentNode.right.gScore = currentNode.gScore + gScore;
                        openList.Add(currentNode.right);
                    }
                }
            }

            if (!closedList.Contains(currentNode.up))
            {
                if (openList.m_tHeap.Contains(currentNode.up))
                {
                    int tempGScore = currentNode.gScore + gScore;

                    if (tempGScore < currentNode.up.gScore)
                    {
                        currentNode.up.prev = currentNode;

                        currentNode.up.gScore = tempGScore;
                    }
                }
                else
                {
                    if (currentNode.up != null)
                    {
                        currentNode.up.prev = currentNode;
                        currentNode.up.gScore = currentNode.gScore + gScore;
                        openList.Add(currentNode.up);
                    }
                }
            }

            if (!closedList.Contains(currentNode.down))
            {
                if (openList.m_tHeap.Contains(currentNode.down))
                {
                    int tempGScore = currentNode.gScore + gScore;

                    if (tempGScore < currentNode.down.gScore)
                    {
                        currentNode.down.prev = currentNode;

                        currentNode.down.gScore = tempGScore;
                    }
                }
                else
                {
                    if (currentNode.down != null)
                    {
                        currentNode.down.prev = currentNode;
                        currentNode.down.gScore = currentNode.gScore + gScore;
                        openList.Add(currentNode.down);
                    }
                }
            }
        }
    }
}
