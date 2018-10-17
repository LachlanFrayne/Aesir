using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Hero : Entity      
{
    public bool ThorSelected;
    public bool FreyaSelected;
    public bool LokiSelected;

    Thor thor;
    Freya freya;
    Loki loki;

    public Grida m_grid;

    public Node m_currentNode;
    public Node m_tempNodeBase;

    public GameObject actionPointCostLabel;
    public GameObject moveSetButtons;

    public Text actionPointLabel;
    public Text actionPointMaxLabel;
    public Text actionPointsMoveCostLabel;
    public Text healthLabel;
    public Text healthMaxLabel;

    public Button moveButton;
    public Button basicAttackButton;
    public Button ability1Button;
    public Button ability2Button;

    public Material movementHighlight;
    public Material removeHighlight;

    List<Node> path = new List<Node>();

    Collider a;
    Collider b;

    public void Start()
    {
        thor = new Thor();
        freya = new Freya();
        loki = new Loki();

        moveSetButtons = GameObject.Find("MoveSet");
        moveButton = GameObject.Find("Move").GetComponent<Button>();
        basicAttackButton = GameObject.Find("Basic Attack").GetComponent<Button>();
        ability1Button = GameObject.Find("Ability 1").GetComponent<Button>();
        ability2Button = GameObject.Find("Ability 2").GetComponent<Button>();
        m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grida>();        //Reference to Grida

    }

    public void Update()
    {
        Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit1;
        if (Physics.Raycast(ray1, out hit1, 100) && ThorSelected)
        {
            if (hit1.collider.GetComponent<Node>() != null)
            {
                if (hit1.collider.GetComponent<Node>().prev != null)
                {
                    for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
                    {
                        for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                        {
                            if (hit1.collider.gameObject == m_grid.boardArray[columnTile, rowTile])
                            {
                                actionPointCostLabel.SetActive(true);
                                actionPointsMoveCostLabel.text = m_grid.nodeBoardArray[columnTile, rowTile].m_gScore.ToString();       //Sets the ActionPointCost to the gScore of the tile

                                if (a == null)
                                    a = hit1.collider;      //a = the first hit
                                else
                                    b = hit1.collider;      //b = the second hit

                                if (a != b)         //if the first hit is different then the second hit
                                {
                                    foreach (Node tile in path)      //Goes through all tiles in the path and sets them back to walkable
                                    {
                                        tile.GetComponent<Renderer>().material = movementHighlight;
                                    }
                                    a = null;       //Resets a 
                                    b = null;       //Resets b
                                    path.Clear();       //Clears the path list
                                }
                                Node temp;      //Creates a temp node
                                temp = m_grid.nodeBoardArray[columnTile, rowTile];      //Sets it to the hit node

                                while (temp.prev != null)
                                {
                                    temp.GetComponent<Renderer>().material.color = Color.green;
                                    path.Add(temp);        //Adds node to path
                                    temp = temp.prev;       //Set temp to temp.prev 
                                }
                            }
                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.GetComponent<Node>() != null)
                {
                    if (hit.collider.GetComponent<Node>().prev != null && ThorSelected == true)        //Used for when you are moving
                    {
                        transform.position = new Vector3(hit.collider.GetComponent<MeshRenderer>().bounds.center.x, 0.5f, hit.collider.GetComponent<MeshRenderer>().bounds.center.z);       //Moves player to hit tile
                        for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
                        {
                            for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                            {
                                if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile])
                                {
                                    Node tempNode = m_currentNode;      //Creates a tempNode and sets it to currentNode
                                    int tempActionPoints = m_nActionPoints;         //Creates a tempActionPoints and sets it to ActionPoints
                                    m_nActionPoints = m_nActionPoints - m_grid.nodeBoardArray[columnTile, rowTile].m_gScore;        //Sets ActionPoints to ActionPoints - hit tile gscore

                                    dijkstrasSearchRemove(tempNode, tempActionPoints, removeHighlight, m_nMovementActionPointCostPerTile);        //Removes the highlight
                                    m_currentNode = m_grid.nodeBoardArray[columnTile, rowTile];        //Sets currentNode to the hit tile
                                    m_currentNode.tag = "CurrentTile";        //Sets currentNode tag = "CurrentTIle"
                                    m_tempNodeBase = m_currentNode;        //Sets tempNodeBase to new currentNode
                                    actionPointCostLabel.SetActive(false);
                                    foreach (Node tile in path)      //Goes through all tiles in the path and removes the material and tag 
                                    {
                                        tile.GetComponent<Renderer>().material = removeHighlight;
                                    }
                                    a = null;       //Resets a
                                    b = null;       //Resets b
                                    path.Clear();       //Clears the path list
                                    SetTile();
                                }
                            }
                        }
                    }
                }
            }
        }
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

    public void SetTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 100))       //Creates a raycast downwards
        {
            for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)     //Goes through the grid 
            {
                for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                {
                    if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile])        //Goes through the grid until it finds the tiles the character is on         
                    {
                        m_currentNode = m_grid.nodeBoardArray[columnTile, rowTile];        //Sets currentNode to the tile the character is on
                        m_currentNode.tag = "CurrentTile";        //Sets currentNodes tag to "CurrentTile"
                        transform.position = new Vector3(m_currentNode.transform.position.x, 0, m_currentNode.transform.position.z);        //Sets position to the center of the tile
                        m_tempNodeBase = m_currentNode;        //Creates a temp node on the currentNode
                        m_currentNode.contain = this.gameObject;
                    }
                }
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
            if (currentNode.tag == "CurrentEnemyTile")
                continue;

            currentNode.GetComponent<Renderer>().sharedMaterial = movementHighlight;

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
                            if (currentNode.m_gScore + gScore > actionPointAvailable)
                            {
                                continue;
                            }
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
            if (currentNode.tag == "CurrentEnemyTile")
                continue;

            currentNode.GetComponent<Renderer>().sharedMaterial = movementHighlight;
            currentNode.tag = "AttackableTile";

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
            if (currentNode.tag == "CurrentTile" || currentNode.tag == "CurrentEnemyTile")
                continue;

            currentNode.GetComponent<Renderer>().sharedMaterial = removeHighlight;
            currentNode.tag = "Tile";
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
                        currentNode.neighbours[i].GetComponent<Renderer>().sharedMaterial = removeHighlight;
                        currentNode.neighbours[i].tag = "Tile";
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
