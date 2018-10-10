using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementRemove : MonoBehaviour {

    public void dijkstrasSearch(Node startNode, int actionPointAvailable, Material removeHighlight)
    {
        Heap openList = new Heap();
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);


        while (openList.m_tHeap.Count > 0)
        {
            Node currentNode = openList.Pop();

            closedList.Add(currentNode);

            if (currentNode.gScore > actionPointAvailable)
            {
                continue;
            }
            if (currentNode.self.tag == "CurrentTile" || currentNode.self.tag == "CurrentEnemyTile")
                continue;

            currentNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
            currentNode.self.tag = "Tile";
            currentNode.gScore = 0;
            currentNode.prev = null;

            for (int i = 0; i < currentNode.neighbours.Length; i++)
            {
                if (!closedList.Contains(currentNode.neighbours[i]))
                {
                    if (openList.m_tHeap.Contains(currentNode.neighbours[i]))
                    {
                        int tempGScore = currentNode.gScore + 1;

                        if (tempGScore < currentNode.neighbours[i].gScore)
                        {
                            currentNode.neighbours[i].prev = currentNode;
                            currentNode.neighbours[i].gScore = tempGScore;
                        }
                        currentNode.neighbours[i].self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                        currentNode.neighbours[i].self.tag = "Tile";
                        currentNode.neighbours[i].gScore = 0;
                        currentNode.neighbours[i].prev = null;
                    }
                    else
                    {
                        if (currentNode.neighbours[i] != null)
                        {
                            currentNode.neighbours[i].prev = currentNode;
                            currentNode.neighbours[i].gScore = currentNode.gScore + 1;
                            openList.Add(currentNode.neighbours[i]);
                        }
                    }
                }
            }            
        }
    }
}
