using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thor : Hero
{
    [Header("Thor")]
    public int m_nThorHealth;
    public int m_nThorHealthMax;
    public int m_nThorActionPoints;
    public int m_nThorActionPointMax;
    public int m_nThorBasicAttack;
    public int m_nThorBasicAttackRange;
    public int m_nThorBasicAttackCost;
    public int m_nThorAbility1Attack;
    public int m_nThorAbility1AttackCost;
    public int m_nThorAbility2Attack;
    public int m_nThorAbility2AttackCost;
    public int m_nThorMovementActionPointCostPerTile;
    int i = 0;
    int j =0;

    public Node m_tempNode;
    
    public Image actionPointsBarImage;
    public Image healthBarImage;
    public Image backgroundThorImage;

	List<Node> path2 = new List<Node>();
	List<Node> path3 = new List<Node>();
	List<Node> path4 = new List<Node>();

	bool bBridalBasicAttack = false;
    bool bBridalAbility1Attack = false;
    bool bThorBasicAttack = false;
    bool bThorAbility1Attack = false;
    bool bThorAbility2Attack = false;
    public bool bBridal = true;
	bool bJump = false;


    [Header("Material")]
    public Material AttackHighlight;
    public Material EnemyHighlight;


    void Start()
    {
        
        actionPointCostLabel = GameObject.Find("Action Points Cost Thor");
        
        actionPointLabel = GameObject.Find("Action Points Thor").GetComponent<Text>();
        actionPointMaxLabel = GameObject.Find("Action Points Max Thor").GetComponent<Text>();
        actionPointsMoveCostLabel = GameObject.Find("Action Points Move Cost Thor").GetComponent<Text>();
        healthLabel = GameObject.Find("Health Thor").GetComponent<Text>();
        healthMaxLabel = GameObject.Find("Health Max Thor").GetComponent<Text>();
        actionPointsBarImage = GameObject.Find("Action Points Bar Thor").GetComponent<Image>();
        healthBarImage = GameObject.Find("Health Bar Thor").GetComponent<Image>();
        backgroundThorImage = GameObject.Find("BackgroundThor").GetComponent<Image>();

        actionPointCostLabel.SetActive(false);

        healthLabel.text = m_nHealth.ToString();
        healthMaxLabel.text = m_nHealthMax.ToString();
        actionPointLabel.text = m_nActionPoints.ToString();
        actionPointMaxLabel.text = m_nActionPointMax.ToString();

        base.Start();

        SetTile();
    }


    void Update()
    {
        

        if (bThorSelected)
        {
            if (bBridal)
            {
                if (m_nActionPoints > 1)        //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
                    moveButton.onClick.AddListener(HighlightMovement);
                else
                    moveButton.onClick.RemoveAllListeners();

                if (m_nActionPoints >= m_nBasicAttackCost)      //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
                    basicAttackButton.onClick.AddListener(BridalBasicAttack);
                else
                    basicAttackButton.onClick.RemoveAllListeners();

                if (m_nActionPoints >= m_nAbility1AttackCost)       //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
                    ability1Button.onClick.AddListener(BridalAbility1);
                else
                    ability1Button.onClick.RemoveAllListeners();
            }
            else
            {
                if (m_nActionPoints > 0)        //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
                    moveButton.onClick.AddListener(HighlightMovement);
                else
                    moveButton.onClick.RemoveAllListeners();

                if (m_nActionPoints >= m_nBasicAttackCost)      //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
                    basicAttackButton.onClick.AddListener(ThorBasicAttack);
                else
                    basicAttackButton.onClick.RemoveAllListeners();

                if (m_nActionPoints >= m_nAbility1AttackCost)       //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
                    ability1Button.onClick.AddListener(ThorAbility1);
                else
                    ability1Button.onClick.RemoveAllListeners();

                if (m_nActionPoints >= m_nAbility2AttackCost)
                    ability2Button.onClick.AddListener(ThorAbility2);
                else
                    ability2Button.onClick.RemoveAllListeners();


                if (i == 0)        //Changes all varibles once thor is upgraded
                {
                    m_nHealth = m_nThorHealth;
                    m_nHealthMax = m_nThorHealthMax;
                    m_nActionPoints = m_nThorActionPoints;
                    m_nActionPointMax = m_nThorActionPointMax;
                    m_nBasicAttackDamage = m_nThorBasicAttack;
                    m_nBasicAttackRange = m_nThorBasicAttackRange;
                    m_nBasicAttackCost = m_nThorBasicAttackCost;
                    m_nAbility1Attack = m_nThorAbility1AttackCost;
                    m_nAbility1AttackCost = m_nThorAbility1AttackCost;
                    m_nAbility2Attack = m_nThorAbility2Attack;
                    m_nAbility2AttackCost = m_nThorAbility2AttackCost;
                    m_nMovementActionPointCostPerTile = m_nThorMovementActionPointCostPerTile;

                }
            }
        }
       

        actionPointsBarImage.fillAmount = (1f / m_nActionPointMax) * m_nActionPoints;       //Sets the amount of the actionPointsBar
        actionPointLabel.text = m_nActionPoints.ToString();      //Sets the ActionPoint text to the amount of actionPoints

        healthBarImage.fillAmount = (1f / m_nHealthMax) * m_nHealth;
        healthLabel.text = m_nHealth.ToString();      //Sets the health text to the amount of health left

        if (bThorSelected)
        {
            backgroundThorImage.GetComponent<Image>().color = new Color32(255, 0, 0, 150);
        }
        if (!bThorSelected)
        {
            backgroundThorImage.GetComponent<Image>().color = new Color32(255, 0, 0, 55);
            actionPointCostLabel.SetActive(false);
        }



        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.GetComponent<Enemy>() != null)
                {
                    if (hit.collider.GetComponent<Enemy>().m_currentNode.prev != null)
                    {
                        actionPointCostLabel.SetActive(false);

                        if (bBridalBasicAttack == true)
                        {
                            m_nActionPoints = m_nActionPoints - m_nBasicAttackCost;
                            m_grid.ClearBoardData();
                            hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nBasicAttackDamage;
                            bBridalBasicAttack = false;
                        }
                        if (bBridalAbility1Attack == true)
                        {
                            m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
                            m_grid.ClearBoardData();
							hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
							hit.collider.GetComponent<Enemy>().bStunned = true;
							bBridalAbility1Attack = false;
                        }

                        if (bThorBasicAttack == true)
                        {
                            m_nActionPoints = m_nActionPoints - m_nBasicAttackCost;
                            m_grid.ClearBoardData();
                            hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nBasicAttackDamage;
                            bThorBasicAttack = false;
                        }
                        //if (bThorAbility1Attack == true)
                        //{
                        //    m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
                        //    m_grid.ClearBoardData();
                        //    hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
                        //    bThorAbility1Attack = false;
                        //}
                        if (bThorAbility2Attack == true)
                        {
                            m_nActionPoints = m_nActionPoints - m_nAbility2AttackCost;
                            m_grid.ClearBoardData();
                            hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
                            bThorAbility2Attack = false;
                        }
                    }
                }
                else
                {
                    Debug.Log("YEET");
                }
				if (bThorAbility1Attack)
				{
					if (hit.collider.GetComponent<Node>() != null)
					{
						if (hit.collider.GetComponent<Node>().prev != null)        //Used for when you are moving
						{
							transform.position = new Vector3(hit.collider.GetComponent<MeshRenderer>().bounds.center.x, 0, hit.collider.GetComponent<MeshRenderer>().bounds.center.z);       //Moves player to hit tile

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
										m_nActionPoints = m_nActionPoints - m_nAbility1Attack;        //Sets ActionPoints to ActionPoints - hit tile gscore
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

										
										m_grid.ClearBoardData();

										Node tempNode1;
										Node tempNode2;

										for (i = 0; i < m_currentNode.neighbours.Length; i++)
										{
											tempNode1 = m_currentNode.neighbours[i];
											tempNode2 = tempNode1.neighbours[(i + 1) % 4];

											if (tempNode1.contain != null && tempNode1.contain.GetComponent<Enemy>() != null)
											{
												tempNode1.contain.GetComponent<Enemy>().m_nHealth = tempNode1.contain.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
											}
											if (tempNode2.contain != null && tempNode2.contain.GetComponent<Enemy>() != null)
											{
												tempNode2.contain.GetComponent<Enemy>().m_nHealth = tempNode2.contain.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
											}

										}

										bThorAbility1Attack = false;


									}
								}
							}
						}
					}
					
				}
			}
        }

		if (bAttack)
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
												if(tile.prev == null)
													tile.GetComponent<Renderer>().material = removeHighlight;
												else
													tile.GetComponent<Renderer>().material = movementHighlight;
											}
											a = null;       //Resets a 
											b = null;       //Resets b
											path.Clear();       //Clears the path list
										}
										Node temp;      //Creates a temp node
										temp = m_grid.nodeBoardArray[columnTile, rowTile];      //Sets it to the hit node

										Node tempNode;
										Node tempNode2;

										for (i = 0; i < temp.neighbours.Length; i++)
										{
											tempNode = temp.neighbours[i];
											tempNode2 = tempNode.neighbours[(i + 1) % 4];


											tempNode.GetComponent<Renderer>().material = AttackHighlight;
											tempNode2.GetComponent<Renderer>().material = AttackHighlight;

											path.Add(tempNode);
											path.Add(tempNode2);
											if (tempNode.contain != null)// && tempNode.contain.GetComponent<Enemy>() == null)
											{
												tempNode.prev = null;
											}
											if (tempNode2.contain != null)// && tempNode2.contain.GetComponent<Enemy>() == null)
											{
												tempNode2.prev = null;
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
		base.Update();
    }

    void HighlightMovement()
    {
        m_grid.ClearBoardData();
        bMove = true;
        dijkstrasSearch(m_currentNode, m_nActionPoints, movementHighlight, m_nMovementActionPointCostPerTile);
    }


    void BridalBasicAttack()
    {
        m_grid.ClearBoardData();
        bMove = false;
        actionPointCostLabel.SetActive(true);
        actionPointsMoveCostLabel.text = m_nBasicAttackCost.ToString();
        bBridalBasicAttack = true;

        dijkstrasSearchAttack(m_currentNode, m_nBasicAttackRange, AttackHighlight, 1);
    }
    void BridalAbility1()
    {
        m_grid.ClearBoardData();
        bMove = false;
        actionPointCostLabel.SetActive(true);
        bBridalAbility1Attack = true;
        actionPointsMoveCostLabel.text = m_nAbility1AttackCost.ToString();
		Node tempNode;
		Node tempNode2;

		for (i=0;i<m_currentNode.neighbours.Length; i++)
        {
			tempNode = m_currentNode.neighbours[i];
			tempNode2 = tempNode.neighbours[(i + 1) % 4];

			
			tempNode.GetComponent<Renderer>().material = AttackHighlight;
			tempNode2.GetComponent<Renderer>().material = AttackHighlight;

			tempNode.prev = m_currentNode;
            tempNode2.prev = m_currentNode.neighbours[i];

			if (tempNode.contain != null && tempNode.contain.GetComponent<Enemy>() == null)
			{
				tempNode.prev = null;
				tempNode.GetComponent<Renderer>().material = removeHighlight;
			}
			if (tempNode2.contain != null && tempNode2.contain.GetComponent<Enemy>() == null)
			{
				tempNode2.prev = null;
				tempNode2.GetComponent<Renderer>().material = removeHighlight;
			}
		}
		path2 = path;
    }
    void ThorBasicAttack()
    {
		m_grid.ClearBoardData();
		bMove = false;
		actionPointCostLabel.SetActive(true);
		bThorBasicAttack = true;
		actionPointsMoveCostLabel.text = m_nBasicAttackCost.ToString();
		Node tempNode;
		Node tempNode2;

		for (i = 0; i < m_currentNode.neighbours.Length; i++)
		{
			tempNode = m_currentNode.neighbours[i];
			tempNode2 = tempNode.neighbours[(i + 1) % 4];


			tempNode.GetComponent<Renderer>().material = AttackHighlight;
			tempNode2.GetComponent<Renderer>().material = AttackHighlight;

			tempNode.prev = m_currentNode;
			tempNode2.prev = m_currentNode.neighbours[i];

			if (tempNode.contain != null && tempNode.contain.GetComponent<Enemy>() == null)
			{
				tempNode.prev = null;
				tempNode.GetComponent<Renderer>().material = removeHighlight;
			}
			if (tempNode2.contain != null && tempNode2.contain.GetComponent<Enemy>() == null)
			{
				tempNode2.prev = null;
				tempNode2.GetComponent<Renderer>().material = removeHighlight;
			}

		}
	}
    void ThorAbility1()
    {
		m_grid.ClearBoardData();
		bMove = false;
		bAttack = true;
		actionPointCostLabel.SetActive(true);
		actionPointsMoveCostLabel.text = m_nAbility1AttackCost.ToString();
		bThorAbility1Attack = true;

		dijkstrasSearchAttack(m_currentNode, 4, movementHighlight, 1);
    }
    void ThorAbility2()
    {
		m_grid.ClearBoardData();
		bMove = false;
		actionPointCostLabel.SetActive(true);
		actionPointsMoveCostLabel.text = m_nAbility2AttackCost.ToString();
		bThorAbility2Attack = true;

		for(int i = 0; i < m_currentNode.neighbours.Length; i++)
		{
			for(i = 0; i < m_nAbility2AttackRange; i++)
			{

			}
		}
    }
}
        
    

