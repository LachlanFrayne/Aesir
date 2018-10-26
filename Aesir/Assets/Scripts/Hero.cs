using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class Hero : Entity      
{
    public bool bThorSelected;
    public bool bFreyaSelected;
    public bool bLokiSelected;
    public bool bMove;

	public int m_nAbility1Attack;
	public int m_nAbility1AttackRange;
	public int m_nAbility1AttackCost;
	public int m_nAbility2Attack;
	public int m_nAbility2AttackRange;
	public int m_nAbility2AttackCost;

	public Grida m_grid;

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

        moveSetButtons = GameObject.Find("MoveSet");
        moveButton = GameObject.Find("Move").GetComponent<Button>();
        basicAttackButton = GameObject.Find("Basic Attack").GetComponent<Button>();
        ability1Button = GameObject.Find("Ability 1").GetComponent<Button>();
        ability2Button = GameObject.Find("Ability 2").GetComponent<Button>();
        m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grida>();        //Reference to Grida

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

    public void Update()
    {
        if (m_nHealth <= 0)
        {
            GameObject.Find("TurnManager").GetComponent<EndGameTurn>().heroes.Remove(this.gameObject);
            Destroy(this.gameObject);
        }
            if (bMove)
            {
                ////////////////////////////////////Path//////////////////////////////////////////////////////
                Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit1;
                if (Physics.Raycast(ray1, out hit1, 100))
                {
                    if ((gameObject.tag == "Thor" && bThorSelected) || (gameObject.tag == "Loki" && bLokiSelected) || (gameObject.tag == "Freya" && bFreyaSelected))
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
                }
            }

            if (Input.GetMouseButtonUp(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    if (hit.collider.tag == "Thor")
                    {
                        moveButton.onClick.RemoveAllListeners();
                        basicAttackButton.onClick.RemoveAllListeners();
                        ability1Button.onClick.RemoveAllListeners();
                        ability2Button.onClick.RemoveAllListeners();
                        m_grid.ClearBoardData();
                        bThorSelected = true;
                        bLokiSelected = false;
                        bFreyaSelected = false;
                    }
                    if (hit.collider.tag == "Loki")
                    {
                        moveButton.onClick.RemoveAllListeners();
                        basicAttackButton.onClick.RemoveAllListeners();
                        ability1Button.onClick.RemoveAllListeners();
                        ability2Button.onClick.RemoveAllListeners();
                        m_grid.ClearBoardData();
                        bLokiSelected = true;
                        bThorSelected = false;
                        bFreyaSelected = false;
                    }
                    if (hit.collider.tag == "Freya")
                    {
                        moveButton.onClick.RemoveAllListeners();
                        basicAttackButton.onClick.RemoveAllListeners();
                        ability1Button.onClick.RemoveAllListeners();
                        ability2Button.onClick.RemoveAllListeners();
                        m_grid.ClearBoardData();
                        bFreyaSelected = true;
                        bThorSelected = false;
                        bLokiSelected = false;
                    }
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 100))
                {
                    ////////////////////////////////////Deletes path and moves player//////////////////////////////////////////////////////
                    if (bMove)
                    {
                        if ((gameObject.tag == "Thor" && bThorSelected) || (gameObject.tag == "Loki" && bLokiSelected) || (gameObject.tag == "Freya" && bFreyaSelected))
                        {
                            if (hit.collider.GetComponent<Node>() != null)
                            {
                                if (hit.collider.GetComponent<Node>().prev != null)        //Used for when you are moving
                                {
                                    transform.position = new Vector3(hit.collider.GetComponent<MeshRenderer>().bounds.center.x, 0.5f, hit.collider.GetComponent<MeshRenderer>().bounds.center.z);       //Moves player to hit tile

                                    for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
                                    {
                                        for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                                        {
                                            if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile])
                                            {
                                                m_currentNode.tag = "Tile";
                                                m_currentNode.contain = null;

                                                Node tempNode = m_currentNode;      //Creates a tempNode and sets it to currentNode
                                                int tempActionPoints = m_nActionPoints;         //Creates a tempActionPoints and sets it to ActionPoints
                                                m_nActionPoints = m_nActionPoints - m_grid.nodeBoardArray[columnTile, rowTile].m_gScore;        //Sets ActionPoints to ActionPoints - hit tile gscore
                                                actionPointCostLabel.SetActive(false);
                                                foreach (Node tile in path)      //Goes through all tiles in the path and removes the material and tag 
                                                {
                                                    tile.GetComponent<Renderer>().material = removeHighlight;
                                                }
                                                a = null;       //Resets a
                                                b = null;       //Resets b
                                                path.Clear();       //Clears the path list
                                                m_grid.ClearBoardData();
                                                SetTile();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        
    }

    public void dijkstrasSearch(Node startNode, int actionPointAvailable, Material movementHighlight, int MoveCostPerTile)
    {
        int a = 0;
        int gScore = MoveCostPerTile;
        Heap openList = new Heap(false);
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);

        while (openList.m_tHeap.Count > 0)
        {
            Node currentNode = openList.Pop();

            closedList.Add(currentNode);

            if (currentNode.m_gScore > actionPointAvailable || currentNode.bObstacle || currentNode.bBlocked)
            {
                continue;
            }
            if (a != 0)
            {
                if (currentNode.contain != null)
                    continue;
            }

            currentNode.GetComponent<Renderer>().material = movementHighlight;

            for (int i = 0; i < currentNode.neighbours.Length; i++)
            {
                if (!closedList.Contains(currentNode.neighbours[i]))
                {
                    if (openList.m_tHeap.Contains(currentNode.neighbours[i]))
                    {
                        if (currentNode.neighbours[i].contain != null || currentNode.neighbours[i].bBlocked || currentNode.neighbours[i].bObstacle)
                            continue;
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
                            if (currentNode.neighbours[i].contain != null || currentNode.neighbours[i].bBlocked || currentNode.neighbours[i].bObstacle)
                                continue;
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
            a++;
        }
    }

    public void dijkstrasSearchAttack(Node startNode, int actionPointAvailable, Material attackMaterial, int MoveCostPerTile)
    {
        int a = 0;
        int gScore = MoveCostPerTile;
        Heap openList = new Heap(false);
        List<Node> closedList = new List<Node>();

        openList.Add(startNode);

        while (openList.m_tHeap.Count > 0)
        {
            Node currentNode = openList.Pop();

            closedList.Add(currentNode);

            if (currentNode.m_gScore > actionPointAvailable || currentNode.bBlocked)
            {
                continue;
            }
            if (a != 0)
            {
                if (currentNode.contain != null && currentNode.contain.GetComponent<Enemy>() == null)
                    continue;
            }

            currentNode.GetComponent<Renderer>().material = attackMaterial;
            for (int i = 0; i < currentNode.neighbours.Length; i++)
            {
                if (!closedList.Contains(currentNode.neighbours[i]))
                {
                    if (openList.m_tHeap.Contains(currentNode.neighbours[i]))
                    {
                        if (currentNode.neighbours[i].contain != null && currentNode.neighbours[i].contain.GetComponent<Enemy>() == null || currentNode.neighbours[i].bBlocked)
                            continue;
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
                            if (currentNode.neighbours[i].contain != null && currentNode.neighbours[i].contain.GetComponent<Enemy>() == null || currentNode.neighbours[i].bBlocked)
                                continue;
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
            a++;
        }
    } 
}
