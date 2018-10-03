using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    void dijkstrasSearch(Node startNode, int actionPointAvailable)
    {
        List<Node> openList = new List<Node>();
        List<Node> closedList = new List<Node>();

        startNode.prev = null;
        openList.Add(startNode);
        while (openList.Count > 0)
        {
            
        }
    }
}
