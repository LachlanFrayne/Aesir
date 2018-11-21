using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct AnimationPreset
{
	public int _animation;
	public int _frameRate;
	public int _frames;
	public int animationDuration;
}

public abstract class Hero : Entity      
{
    public bool bThorSelected;
    public bool bFreyaSelected;
    public bool bLokiSelected;
    public bool bMove;
	public bool bAttack;
	private bool bFinished = false;

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
	public Button cancelButton;

    public Material movementHighlight;
    public Material removeHighlight;
	public Material selectedHeroMat;

	public Sprite moveEnableImage;
	public Sprite moveDisableImage;
	public Sprite attackEnableImage;
	public Sprite attackDisableImage;
	public Sprite ability1EnableImage;
	public Sprite ability1DisableImage;
	public Sprite ability2EnableImage;
	public Sprite ability2DisableImage;


	public WorldSpaceUI worldSpaceUI;

	protected LinkedList<Node> path = new LinkedList<Node>();

    protected Collider a;
	protected Collider b;

	public float speed = 1f;

	public List<AnimationPreset> m_animPresets;
	private Animator m_turnChecker;

    public void Start()
    {
        moveSetButtons = GameObject.Find("MoveSet");
        moveButton = GameObject.Find("Move").GetComponent<Button>();
        basicAttackButton = GameObject.Find("Basic Attack").GetComponent<Button>();
        ability1Button = GameObject.Find("Ability 1").GetComponent<Button>();
        ability2Button = GameObject.Find("Ability 2").GetComponent<Button>();
		cancelButton = GameObject.Find("Cancel").GetComponent<Button>();
        m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grida>();        //Reference to Grida

		m_turnChecker = GameObject.Find("TurnManager").GetComponent<Animator>();
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
		if (m_nHealth > m_nHealthMax)
		{
			m_nHealth = m_nHealthMax;
		}

        if (m_nHealth <= 0)
        {
            GameObject.Find("TurnManager").GetComponent<EndGameTurn>().m_heroes.Remove(this.gameObject);
            Destroy(this.gameObject);
        }

		if (bFinished == true)
		{
			path.Clear();       //Clears the path list
			bFinished = false;
		}

		if (!(m_turnChecker.GetCurrentAnimatorStateInfo(0).IsName("PlayerTurn")))
		{
			return;
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
                                                path.AddFirst(temp);        //Adds node to path
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
					cancelButton.onClick.RemoveAllListeners();
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
					cancelButton.onClick.RemoveAllListeners();
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
					cancelButton.onClick.RemoveAllListeners();
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
								//transform.position = new Vector3(hit.collider.GetComponent<MeshRenderer>().bounds.center.x, 0.5f, hit.collider.GetComponent<MeshRenderer>().bounds.center.z);       //Moves player to hit tile
								StartCoroutine(RunAnim(m_animPresets[0]));

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

											StartCoroutine(Wait());

											
												a = null;       //Resets a
												b = null;       //Resets b
												m_grid.ClearBoardData();
												bMove = false;
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

	IEnumerator Wait()
	{
		m_currentNode.GetComponent<Renderer>().material = removeHighlight;
		m_currentNode = null;
		foreach (Node tile in path)     
		{
			Vector3 playerPosition = transform.position;
			Vector3 tilePosition = tile.transform.position;
			float startTime = Time.time;
			float journeyLength = Vector3.Distance(playerPosition, tilePosition);
			StartCoroutine(Lerp(playerPosition, tilePosition, startTime, journeyLength));
			yield return new WaitForSeconds(journeyLength / speed);
		}

		bFinished = true;
		SetTile();
	}
	//Check if first tile is being added to list
	IEnumerator Lerp(Vector3 playerPosition, Vector3 tilePosition, float startTime, float journeyLength)
	{
		while (playerPosition != tilePosition)
		{
			float distCovered = ((Time.time - startTime) * speed);
			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp(playerPosition, tilePosition, fracJourney);
			yield return null;
		}
	}

	IEnumerator RunAnim(AnimationPreset anim)
	{
		Material mat = new Material(GetComponentInChildren<Renderer>().material);

		SetAnim(anim);

		yield return new WaitForSeconds(anim.animationDuration);

		GetComponentInChildren<Renderer>().material = mat;
	}

	public void SetAnim(AnimationPreset anim)
	{
		//Material temp = new Material(Shader.Find("Shader Forge/FrameAnimTest2Normalised"));

		Material temp = GetComponentInChildren<Renderer>().material;

		temp.SetFloat("_Animation", anim._animation);
		temp.SetFloat("_FrameRate", anim._frameRate);
		temp.SetFloat("_Frames", anim._frames);
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
                if (currentNode.contain != null && (currentNode.contain.GetComponent<Enemy>() == null && currentNode.contain.GetComponent<DestructibleObject>() == null))
                    continue;
			}

            currentNode.GetComponent<Renderer>().material = attackMaterial;
            for (int i = 0; i < currentNode.neighbours.Length; i++)
            {
                if (!closedList.Contains(currentNode.neighbours[i]))
                {
                    if (openList.m_tHeap.Contains(currentNode.neighbours[i]))
                    {
                        if (currentNode.neighbours[i].contain != null && (currentNode.neighbours[i].contain.GetComponent<Enemy>() == null && currentNode.neighbours[i].contain.GetComponent<DestructibleObject>() == null) || currentNode.neighbours[i].bBlocked)
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
                            if (currentNode.neighbours[i].contain != null && (currentNode.neighbours[i].contain.GetComponent<Enemy>() == null && currentNode.neighbours[i].contain.GetComponent<DestructibleObject>() == null) || currentNode.neighbours[i].bBlocked)
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
