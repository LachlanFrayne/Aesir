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

            //////////////////////////////////////////////////////////////////////////////////////////
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
            //if (!closedList.Contains(currentNode.left))
            //{
            //    if (openList.m_tHeap.Contains(currentNode.left))
            //    {
            //        int tempGScore = currentNode.gScore + 1;
            //
            //        if (tempGScore < currentNode.left.gScore)
            //        {
            //            currentNode.left.prev = currentNode;
            //            currentNode.left.gScore = tempGScore;
            //        }
            //        currentNode.left.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
            //        currentNode.left.self.tag = "Tile";
            //        currentNode.left.gScore = 0;
            //        currentNode.left.prev = null;
            //    }
            //    else
            //    {
            //        if (currentNode.left != null)
            //        {
            //            currentNode.left.prev = currentNode;
            //            currentNode.left.gScore = currentNode.gScore + 1;
            //            openList.Add(currentNode.left);
            //        }
            //
            //    }
            //}
            //
            //if (!closedList.Contains(currentNode.right))
            //{
            //    if (openList.m_tHeap.Contains(currentNode.right))
            //    {
            //        int tempGScore = currentNode.gScore + 1;
            //
            //        if (tempGScore < currentNode.right.gScore)
            //        {
            //            currentNode.right.prev = currentNode;
            //
            //            currentNode.right.gScore = tempGScore;
            //            currentNode.right.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
            //            currentNode.right.self.tag = "Tile";
            //            currentNode.right.gScore = 0;
            //            currentNode.right.prev = null;
            //        }
            //    }
            //    else
            //    {
            //        if (currentNode.right != null)
            //        {
            //            currentNode.right.prev = currentNode;
            //            currentNode.right.gScore = currentNode.gScore + 1;
            //            openList.Add(currentNode.right);
            //        }
            //    }
            //}
            //
            //if (!closedList.Contains(currentNode.up))
            //{
            //    if (openList.m_tHeap.Contains(currentNode.up))
            //    {
            //        int tempGScore = currentNode.gScore + 1;
            //
            //        if (tempGScore < currentNode.up.gScore)
            //        {
            //            currentNode.up.prev = currentNode;
            //
            //            currentNode.up.gScore = tempGScore;
            //            currentNode.up.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
            //            currentNode.up.self.tag = "Tile";
            //            currentNode.up.gScore = 0;
            //            currentNode.up.prev = null;
            //        }
            //    }
            //    else
            //    {
            //        if (currentNode.up != null)
            //        {
            //            currentNode.up.prev = currentNode;
            //            currentNode.up.gScore = currentNode.gScore + 1;
            //            openList.Add(currentNode.up);
            //        }
            //    }
            //}
            //
            //if (!closedList.Contains(currentNode.down))
            //{
            //    if (openList.m_tHeap.Contains(currentNode.down))
            //    {
            //        int tempGScore = currentNode.gScore + 1;
            //
            //        if (tempGScore < currentNode.down.gScore)
            //        {
            //            currentNode.down.prev = currentNode;
            //
            //            currentNode.down.gScore = tempGScore;
            //            currentNode.down.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
            //            currentNode.down.self.tag = "Tile";
            //            currentNode.down.gScore = 0;
            //            currentNode.down.prev = null;
            //        }
            //    }
            //    else
            //    {
            //        if (currentNode.down != null)
            //        {
            //            currentNode.down.prev = currentNode;
            //            currentNode.down.gScore = currentNode.gScore + 1;
            //            openList.Add(currentNode.down);
            //        }
            //    }
            //}
        }
    }
}
