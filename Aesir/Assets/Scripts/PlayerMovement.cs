using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public void dijkstrasSearch(Node startNode, int actionPointAvailable, Material movementHighlight)
    {
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
            currentNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
            currentNode.self.tag = "ThorWalkableTile";

            if (!closedList.Contains(currentNode.left))
            {
                if (openList.m_tHeap.Contains(currentNode.left))
                {
                    int tempGScore = currentNode.gScore + 1;

                    if (tempGScore < currentNode.left.gScore)
                    {
                        currentNode.left.prev = currentNode;
                        currentNode.left.gScore = tempGScore;
                    }
                }
                else
                {

                    currentNode.left.prev = currentNode;
                    currentNode.left.gScore = currentNode.gScore + 1;
                    openList.Add(currentNode.left);
                }
            }

            if (!closedList.Contains(currentNode.right))
            {
                if (openList.m_tHeap.Contains(currentNode.right))
                {
                    int tempGScore = currentNode.gScore + 1;

                    if (tempGScore < currentNode.right.gScore)
                    {
                        currentNode.right.prev = currentNode;

                        currentNode.right.gScore = tempGScore;
                    }
                }
                else
                {

                    currentNode.right.prev = currentNode;
                    currentNode.right.gScore = currentNode.gScore + 1;
                    openList.Add(currentNode.right);
                }
            }

            if (!closedList.Contains(currentNode.up))
            {
                if (openList.m_tHeap.Contains(currentNode.up))
                {
                    int tempGScore = currentNode.gScore + 1;

                    if (tempGScore < currentNode.up.gScore)
                    {
                        currentNode.up.prev = currentNode;

                        currentNode.up.gScore = tempGScore;
                    }
                }
                else
                {

                    currentNode.up.prev = currentNode;
                    currentNode.up.gScore = currentNode.gScore + 1;
                    openList.Add(currentNode.up);
                }
            }

            if (!closedList.Contains(currentNode.down))
            {
                if (openList.m_tHeap.Contains(currentNode.down))
                {
                    int tempGScore = currentNode.gScore + 1;

                    if (tempGScore < currentNode.down.gScore)
                    {
                        currentNode.down.prev = currentNode;

                        currentNode.down.gScore = tempGScore;
                    }
                }
                else
                {

                    currentNode.down.prev = currentNode;
                    currentNode.down.gScore = currentNode.gScore + 1;
                    openList.Add(currentNode.down);
                }
            }
        }
    }
}
