using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Hero : Entity      
{
    public bool ThorSelected;
    public bool FreyaSelected;
    public bool LokiSelected;

    Thor thor;
    Freya freya;
    Loki loki;

    void Start()
    {
        thor = new Thor();
        freya = new Freya();
        loki = new Loki();
    }

    public void ChangeCharacters(string selectedCharacter, string characterSwappingWith)
    {
        if (ThorSelected)
        {
            if (selectedCharacter == "Thor" && characterSwappingWith == "Freya")
            {
                dijkstrasSearchRemove(thor.m_currentNode, thor.m_nActionPoints, thor.removeHighlight, thor.m_nMovementActionPointCostPerTile);
            }
            if (selectedCharacter == "Thor" && characterSwappingWith == "Loki")
            {
                dijkstrasSearchRemove(thor.m_currentNode, thor.m_nActionPoints, thor.removeHighlight, thor.m_nMovementActionPointCostPerTile);
            }
        }
        if (FreyaSelected)
        {
            if (selectedCharacter == "Freya" && characterSwappingWith == "Thor")
            {
                dijkstrasSearchRemove(freya.m_currentNode, freya.m_nActionPoints, freya.removeHighlight, freya.m_nMovementActionPointCostPerTile);
            }
            if (selectedCharacter == "Freya" && characterSwappingWith == "Loki")
            {
                dijkstrasSearchRemove(freya.m_currentNode, freya.m_nActionPoints, freya.removeHighlight, freya.m_nMovementActionPointCostPerTile);
            }
        }
        if (LokiSelected)
        {
            if (selectedCharacter == "Loki" && characterSwappingWith == "Thor")
            {
                dijkstrasSearchRemove(loki.m_currentNode, loki.m_nActionPoints, loki.removeHighlight, loki.m_nMovementActionPointCostPerTile);
            }
            if (selectedCharacter == "Loki" && characterSwappingWith == "Freya")
            {
                dijkstrasSearchRemove(loki.m_currentNode, loki.m_nActionPoints, loki.removeHighlight, loki.m_nMovementActionPointCostPerTile);
            }
        }
    }


    public void dijkstrasSearch(Node startNode, int actionPointAvailable, Material movementHighlight, int MoveCostPerTile)
    {
        int gScore = MoveCostPerTile;
        Heap openList = new Heap(false);
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);

        while (openList.m_tHeap.Count > 0)
        {
            Node currentNode = openList.Pop();

            closedList.Add(currentNode);

            if (currentNode.m_gScore > actionPointAvailable)
            {
                continue;
            }
            if (currentNode.self.tag == "CurrentEnemyTile")
                continue;

            currentNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
            currentNode.self.tag = "WalkableTile";

            for (int i = 0; i < currentNode.neighbours.Length; i++)
            {
                if (!closedList.Contains(currentNode.neighbours[i]))
                {
                    if (openList.m_tHeap.Contains(currentNode.neighbours[i]))
                    {
                        int tempGScore = currentNode.m_gScore + gScore;

                        if (tempGScore < currentNode.neighbours[i].m_gScore)
                        {
                            currentNode.neighbours[i].prev = currentNode;
                            currentNode.neighbours[i].m_gScore = tempGScore;
                        }
                    }
                    else
                    {
                        if (currentNode.neighbours[i] != null)
                        {
                            currentNode.neighbours[i].prev = currentNode;
                            currentNode.neighbours[i].m_gScore = currentNode.m_gScore + gScore;
                            openList.Add(currentNode.neighbours[i]);
                        }
                    }
                }
            }
        }
    }

    public void dijkstrasSearchAttack(Node startNode, int actionPointAvailable, Material movementHighlight, int MoveCostPerTile)
    {
        int gScore = MoveCostPerTile;
        Heap openList = new Heap(false);
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);

        while (openList.m_tHeap.Count > 0)
        {
            Node currentNode = openList.Pop();

            closedList.Add(currentNode);

            if (currentNode.m_gScore > actionPointAvailable)
            {
                continue;
            }
            if (currentNode.self.tag == "CurrentEnemyTile")
                continue;

            currentNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
            currentNode.self.tag = "AttackableTile";

            for (int i = 0; i < currentNode.neighbours.Length; i++)
            {
                if (!closedList.Contains(currentNode.neighbours[i]))
                {
                    if (openList.m_tHeap.Contains(currentNode.neighbours[i]))
                    {
                        int tempGScore = currentNode.m_gScore + gScore;

                        if (tempGScore < currentNode.neighbours[i].m_gScore)
                        {
                            currentNode.neighbours[i].prev = currentNode;
                            currentNode.neighbours[i].m_gScore = tempGScore;
                        }
                    }
                    else
                    {
                        if (currentNode.neighbours[i] != null)
                        {
                            currentNode.neighbours[i].prev = currentNode;
                            currentNode.neighbours[i].m_gScore = currentNode.m_gScore + gScore;
                            openList.Add(currentNode.neighbours[i]);
                        }
                    }
                }
            }
        }
    }

    public void dijkstrasSearchRemove(Node startNode, int actionPointAvailable, Material removeHighlight, int MoveCostPerTile)
    {
        int gScore = MoveCostPerTile;
        Heap openList = new Heap(false);
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);


        while (openList.m_tHeap.Count > 0)
        {
            Node currentNode = openList.Pop();

            closedList.Add(currentNode);

            if (currentNode.m_gScore > actionPointAvailable)
            {
                continue;
            }
            if (currentNode.self.tag == "CurrentTile" || currentNode.self.tag == "CurrentEnemyTile")
                continue;

            currentNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
            currentNode.self.tag = "Tile";
            currentNode.m_gScore = 0;
            currentNode.prev = null;

            for (int i = 0; i < currentNode.neighbours.Length; i++)
            {
                if (!closedList.Contains(currentNode.neighbours[i]))
                {
                    if (openList.m_tHeap.Contains(currentNode.neighbours[i]))
                    {
                        int tempGScore = currentNode.m_gScore + gScore;

                        if (tempGScore < currentNode.neighbours[i].m_gScore)
                        {
                            currentNode.neighbours[i].prev = currentNode;
                            currentNode.neighbours[i].m_gScore = tempGScore;
                        }
                        currentNode.neighbours[i].self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                        currentNode.neighbours[i].self.tag = "Tile";
                        currentNode.neighbours[i].m_gScore = 0;
                        currentNode.neighbours[i].prev = null;
                    }
                    else
                    {
                        if (currentNode.neighbours[i] != null)
                        {
                            currentNode.neighbours[i].prev = currentNode;
                            currentNode.neighbours[i].m_gScore = currentNode.m_gScore + gScore;
                            openList.Add(currentNode.neighbours[i]);
                        }
                    }
                }
            }
        }
    }

    



}
