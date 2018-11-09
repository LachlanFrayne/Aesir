using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Thor : BridalThor
{
     
    bool bAbility2Attack = false;

	public Node[,] list;

	List<Node> ability2List = new List<Node>();

     void Start()
    {
		list = new Node[4, m_nAbility2AttackRange];

		bBasicAttack = false;
		bAbility1Attack = false;
		
        actionPointCostLabel.SetActive(false);
		
        healthLabel.text = m_nHealth.ToString();
        healthMaxLabel.text = m_nHealthMax.ToString();
		worldSpaceUI.thorHealthMaxOverheadLabel.text = healthMaxLabel.text;

		actionPointLabel.text = m_nActionPoints.ToString();
        actionPointMaxLabel.text = m_nActionPointMax.ToString();

        base.Start();

		SetTile();
    }

    void Update()
    {
		if (bThorSelected)
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

			if (bThorSelected)
				cancelButton.onClick.AddListener(Cancel);
			else
				cancelButton.onClick.RemoveAllListeners();
		}
       
        actionPointsBarImage.fillAmount = (1f / m_nActionPointMax) * m_nActionPoints;       //Sets the amount of the actionPointsBar
        actionPointLabel.text = m_nActionPoints.ToString();      //Sets the ActionPoint text to the amount of actionPoints

        healthBarImage.fillAmount = (1f / m_nHealthMax) * m_nHealth;
		worldSpaceUI.thorHealthBarOverheadImage.fillAmount = healthBarImage.fillAmount;

		healthLabel.text = m_nHealth.ToString();      //Sets the health text to the amount of health left
		worldSpaceUI.thorHealthOverheadLabel.text = healthLabel.text;

		if (bThorSelected)
        {
            backgroundThorImage.GetComponent<Image>().color = new Color32(255, 0, 0, 150);
        }
        if (!bThorSelected)
        {
            backgroundThorImage.GetComponent<Image>().color = new Color32(255, 0, 0, 55);
            actionPointCostLabel.SetActive(false);
			bMove = false;
			bBasicAttack = false;
			bAbility1Attack = false;
			bAbility2Attack = false;
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

                        if (bBasicAttack == true)
                        {
                            m_nActionPoints = m_nActionPoints - m_nBasicAttackCost;
                            m_grid.ClearBoardData();
                            hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nBasicAttackDamage;
                            bBasicAttack = false;
							actionPointCostLabel.SetActive(false);
						}
                    }
                }
				else if (hit.collider.GetComponent<DestructibleObject>() != null)
				{
					if (bBasicAttack == true)
					{
						m_nActionPoints = m_nActionPoints - m_nBasicAttackCost;
						m_grid.ClearBoardData();
						hit.collider.GetComponent<DestructibleObject>().m_nHealth = hit.collider.GetComponent<DestructibleObject>().m_nHealth - m_nBasicAttackDamage;
						bBasicAttack = false;
					}
				}

				if (bAbility1Attack)
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

										for (int i = 0; i < m_currentNode.neighbours.Length; i++)
										{
											tempNode1 = m_currentNode.neighbours[i];
											tempNode2 = tempNode1.neighbours[(i + 1) % 4];

											if (tempNode1.contain != null && tempNode1.contain.GetComponent<Enemy>() != null)
											{
												tempNode1.contain.GetComponent<Enemy>().m_nHealth = tempNode1.contain.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
											}
											else if (tempNode1.contain != null && tempNode1.contain.GetComponent<DestructibleObject>() != null)
											{
												tempNode1.contain.GetComponent<DestructibleObject>().m_nHealth = tempNode1.contain.GetComponent<DestructibleObject>().m_nHealth - m_nAbility1Attack;
											}
											if (tempNode2.contain != null && tempNode2.contain.GetComponent<Enemy>() != null)
											{
												tempNode2.contain.GetComponent<Enemy>().m_nHealth = tempNode2.contain.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
											}
											else if (tempNode2.contain != null && tempNode2.contain.GetComponent<DestructibleObject>() != null)
											{
												tempNode2.contain.GetComponent<DestructibleObject>().m_nHealth = tempNode2.contain.GetComponent<DestructibleObject>().m_nHealth - m_nAbility1Attack;
											}

										}
										bAbility1Attack = false;
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
				if (bAbility2Attack && bThorSelected)
				{
					for (int b = 0; b < m_currentNode.neighbours.Length; b++)
					{
						Node temp = m_currentNode.neighbours[b];
						for (int c = 0; c < m_nAbility2AttackRange; c++)
						{
							temp.GetComponent<Renderer>().material = AttackHighlight;
							temp = temp.neighbours[b];
						}
					}
						
					for (int i = 0; i < 4; i++)
					{
						for(int j = 0; j < m_nAbility2AttackRange; j++)
						{
							if(hit1.collider.GetComponent<Node>() == list[i,j])
							{
								for (int a = 0; a < m_nAbility2AttackRange; a++)
								{
									list[i, a].GetComponent<Renderer>().material.color = Color.green;
									if (Input.GetMouseButtonUp(0))
									{
										if (list[i, a].contain != null)
										{
											if (list[i, a].contain.GetComponent<Enemy>() != null)
											{
												list[i, a].contain.GetComponent<Enemy>().m_nHealth = list[i, a].contain.GetComponent<Enemy>().m_nHealth - m_nAbility2Attack;
												m_nActionPoints = m_nActionPoints - m_nAbility2AttackCost;
												m_grid.ClearBoardData();
												bAbility2Attack = false;
												dijkstrasSearchAbility2(list[i, a].contain.GetComponent<Enemy>().m_currentNode, 3, AttackHighlight, 1);
											
												foreach(Node tile in ability2List)
												{
													if (tile.contain != null)
													{
														if (tile.contain.GetComponent<Enemy>() != null)
														{
															tile.contain.GetComponent<Enemy>().m_nHealth = tile.contain.GetComponent<Enemy>().m_nHealth - m_nAbility2Attack;
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
				}
				else if ((gameObject.tag == "Thor" && bThorSelected) || (gameObject.tag == "Loki" && bLokiSelected) || (gameObject.tag == "Freya" && bFreyaSelected))
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

										for (int i = 0; i < temp.neighbours.Length; i++)
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
	void ThorBasicAttack()
    {
		m_grid.ClearBoardData();
		bMove = false;
		bAbility1Attack = false;
		bAbility2Attack = false;
		actionPointCostLabel.SetActive(true);
		bBasicAttack = true;
		actionPointsMoveCostLabel.text = m_nBasicAttackCost.ToString();
		Node tempNode;
		Node tempNode2;

		for (int i = 0; i < m_currentNode.neighbours.Length; i++)
		{
			tempNode = m_currentNode.neighbours[i];
			tempNode2 = tempNode.neighbours[(i + 1) % 4];


			tempNode.GetComponent<Renderer>().material = AttackHighlight;
			tempNode2.GetComponent<Renderer>().material = AttackHighlight;

			tempNode.prev = m_currentNode;
			tempNode2.prev = m_currentNode.neighbours[i];

			if (tempNode.contain != null && (tempNode.contain.GetComponent<Enemy>() == null && tempNode.contain != null && tempNode.contain.GetComponent<DestructibleObject>() == null))
			{
				tempNode.prev = null;
				tempNode.GetComponent<Renderer>().material = removeHighlight;
			}
			if (tempNode2.contain != null && (tempNode2.contain.GetComponent<Enemy>() == null && tempNode2.contain != null && tempNode2.contain.GetComponent<DestructibleObject>() == null))
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
		bBasicAttack = false;
		bAbility2Attack = false;
		bAttack = true;
		actionPointCostLabel.SetActive(true);
		actionPointsMoveCostLabel.text = m_nAbility1AttackCost.ToString();
		bAbility1Attack = true;

		dijkstrasSearchAttack(m_currentNode, m_nAbility1AttackRange, movementHighlight, 1);
    }
    void ThorAbility2()
    {
		m_grid.ClearBoardData();
		bMove = false;
		bBasicAttack = false;
		bAbility1Attack = false;
		bAttack = true;
		actionPointCostLabel.SetActive(true);
		actionPointsMoveCostLabel.text = m_nAbility2AttackCost.ToString();
		bAbility2Attack = true;

		for(int i = 0; i < m_currentNode.neighbours.Length; i++)
		{
			Node temp = m_currentNode.neighbours[i];
			for (int j = 0; j < m_nAbility2AttackRange; j++)
			{ 
				temp.GetComponent<Renderer>().material = AttackHighlight;
				list[i, j] = temp;
				temp = temp.neighbours[i];
			}
		}
    }

	public void dijkstrasSearchAbility2(Node startNode, int actionPointAvailable, Material healingHighlight, int MoveCostPerTile)
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

			if (currentNode.m_gScore > actionPointAvailable)
			{
				continue;
			}
			if(a !=0)
				ability2List.Add(currentNode);

			for (int i = 0; i < currentNode.neighbours.Length; i++)
			{
				if (!closedList.Contains(currentNode.neighbours[i]))
				{
					if (openList.m_tHeap.Contains(currentNode.neighbours[i]))
					{
						int tempGScore = currentNode.m_gScore + gScore;

						if (tempGScore < currentNode.neighbours[i].m_gScore)
						{ 
							currentNode.neighbours[i].m_gScore = tempGScore;
						}
					}
					else
					{
						if (currentNode.neighbours[i] != null)
						{
							currentNode.neighbours[i].m_gScore = currentNode.m_gScore + gScore;
							openList.Add(currentNode.neighbours[i]);
						}
					}
				}
			}
			a++;
		}
	}
	void Cancel()
	{
		m_grid.ClearBoardData();
		bMove = false;
		bBasicAttack = false;
		bAbility1Attack = false;
		bAbility2Attack = false;
		actionPointCostLabel.SetActive(false);
	}
}
        
    

