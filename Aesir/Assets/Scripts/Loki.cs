using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loki : Hero
{
	public Node m_tempNode;

	public Image actionPointsBarImage;
	public Image healthBarImage;
	public Image backgroundLokiImage;

	[Header("Material")]
	public Material AttackHighlight;
	public Material EnemyHighlight;

	void Start()
	{
		worldSpaceUI = GameObject.Find("WorldSpaceUI").GetComponent<WorldSpaceUI>();

		actionPointCostLabel = GameObject.Find("Action Points Cost Loki");
		actionPointLabel = GameObject.Find("Action Points Loki").GetComponent<Text>();
		actionPointMaxLabel = GameObject.Find("Action Points Max Loki").GetComponent<Text>();
		actionPointsMoveCostLabel = GameObject.Find("Action Points Move Cost Loki").GetComponent<Text>();
		healthLabel = GameObject.Find("Health Loki").GetComponent<Text>();
		healthMaxLabel = GameObject.Find("Health Max Loki").GetComponent<Text>();
		actionPointsBarImage = GameObject.Find("Action Points Bar Loki").GetComponent<Image>();
		healthBarImage = GameObject.Find("Health Bar Loki").GetComponent<Image>();
		backgroundLokiImage = GameObject.Find("BackgroundLoki").GetComponent<Image>();

		actionPointCostLabel.SetActive(false);

		healthLabel.text = m_nHealth.ToString();
		healthMaxLabel.text = m_nHealthMax.ToString();
		worldSpaceUI.lokiHealthMaxOverheadLabel.text = healthMaxLabel.text;

		actionPointLabel.text = m_nActionPoints.ToString();
		actionPointMaxLabel.text = m_nActionPointMax.ToString();

		base.Start();

		SetTile();
	}

	void Update()
	{
		if (bLokiSelected)
		{
			if (m_nActionPoints > 0)        //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
			{
				moveButton.onClick.AddListener(HighlightMovement);
				moveButton.image.sprite = moveEnableImage;
			}
			else
			{
				moveButton.onClick.RemoveAllListeners();
				moveButton.image.sprite = moveDisableImage;
			}

			if (m_nActionPoints >= m_nBasicAttackCost)      //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
			{
				basicAttackButton.onClick.AddListener(BasicAttack);
				basicAttackButton.image.sprite = attackEnableImage;
			}
			else
			{
				basicAttackButton.onClick.RemoveAllListeners();
				basicAttackButton.image.sprite = attackDisableImage;
			}

			if (m_nActionPoints >= m_nAbility1AttackCost)       //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
			{
				ability1Button.onClick.AddListener(Ability1);
				ability1Button.image.sprite = ability1EnableImage;
			}
			else
			{
				ability1Button.onClick.RemoveAllListeners();
				ability1Button.image.sprite = ability1DisableImage;
			}

			if (bLokiSelected)
			{
				cancelButton.onClick.AddListener(Cancel);

			}
			else
			{
				cancelButton.onClick.RemoveAllListeners();

			}

			if (m_currentNode != null)
			{
				m_currentNode.gameObject.GetComponent<Renderer>().material = selectedHeroMat;
			}
		}

		

		actionPointsBarImage.fillAmount = (1f / m_nActionPointMax) * m_nActionPoints;       //Sets the amount of the actionPointsBar		
		actionPointLabel.text = m_nActionPoints.ToString();      //Sets the ActionPoint text to the amount of actionPoints

		healthBarImage.fillAmount = (1f / m_nHealthMax) * m_nHealth;
		worldSpaceUI.lokiHealthBarOverheadImage.fillAmount = healthBarImage.fillAmount;

		healthLabel.text = m_nHealth.ToString();      //Sets the health text to the amount of health left
		worldSpaceUI.lokiHealthOverheadLabel.text = healthLabel.text;

		if (bLokiSelected)
		{
			backgroundLokiImage.GetComponent<Image>().color = new Color32(0, 0, 255, 150);
		}
		if (!bLokiSelected)
		{
			backgroundLokiImage.GetComponent<Image>().color = new Color32(0, 0, 255, 55);
			actionPointCostLabel.SetActive(false);
			bMove = false;
			bBasicAttack = false;
			bAbility1Attack = false;
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
							Node node = hit.collider.GetComponent<Enemy>().m_currentNode.prev;
							
							StartCoroutine(basicAttack(m_animPresets[2], hit.collider.GetComponent<Enemy>()));	
						}
					}
					if (bAbility1Attack == true)
					{
						m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
						m_grid.ClearBoardData();

						Node temp = m_currentNode;      //Sets the current nodes for all
						m_currentNode = hit.collider.GetComponent<Enemy>().m_currentNode;
						hit.collider.GetComponent<Enemy>().m_currentNode = temp;

						m_currentNode.contain = this.gameObject;
						hit.collider.GetComponent<Enemy>().m_currentNode.contain = hit.collider.gameObject;

						Vector3 temp1 = transform.position;
						transform.position = hit.collider.GetComponent<Collider>().transform.position;
						hit.collider.GetComponent<Collider>().transform.position = temp1;
						hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
						bAbility1Attack = false;
					}
				}
				else if (hit.collider.GetComponentInParent<Freya>() != null)
				{
					if (bAbility1Attack == true)
					{
						StartCoroutine(ability1Attack(m_animPresets[5], hit.collider.GetComponentInParent<Freya>()));
					}
				}
				else if (hit.collider.GetComponentInParent<BridalThor>() != null)
				{
					if (bAbility1Attack == true)
					{
						StartCoroutine(ability1Attack(m_animPresets[5], hit.collider.GetComponentInParent<BridalThor>()));
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

	void BasicAttack()
	{
		m_grid.ClearBoardData();
		bMove = false;
		bAttacking = true;
		bAbility1Attack = false;
		actionPointCostLabel.SetActive(true);
		bBasicAttack = true;
		actionPointsMoveCostLabel.text = m_nAbility1AttackCost.ToString();
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

	

	void Ability1()
	{
		m_grid.ClearBoardData();
		bAttacking = true;
		freya.bAttacking = true;
		if(bridalThor != null)
		{
			if (bridalThor.enabled == false)
			{
				bridalThor = GameObject.Find("Thor").GetComponent<Thor>();
				bridalThor.bAttacking = true;
			}
			else
				bridalThor.bAttacking = true;
		}
		bMove = false;
		bBasicAttack = false;
		actionPointCostLabel.SetActive(true);
		bAbility1Attack = true;
		actionPointsMoveCostLabel.text = m_nAbility1AttackCost.ToString();

		dijkstrasSearchTeleport(m_currentNode, m_nAbility1AttackRange, AttackHighlight, 1);
	}

	IEnumerator ability1Attack(AnimationPreset anim, Entity entity)
	{
		StartCoroutine(RunAnim(anim));
		yield return new WaitForSeconds(anim.animationDuration);

		m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
		m_grid.ClearBoardData();

		Node temp = m_currentNode;      //Sets the current nodes for all
		m_currentNode = entity.m_currentNode;
		entity.m_currentNode = temp;

		m_currentNode.contain = this.gameObject;
		entity.m_currentNode.contain = entity.gameObject;

		Vector3 temp1 = transform.position;
		transform.position = entity.transform.position;
		entity.transform.position = temp1;
		bAbility1Attack = false;
		bAttacking = false;
		bridalThor.bAttacking = false;
		loki.bAttacking = false;
		freya.bAttacking = false;
	}

	public void dijkstrasSearchTeleport(Node startNode, int actionPointAvailable, Material healingHighlight, int MoveCostPerTile)
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

			currentNode.GetComponent<Renderer>().material = healingHighlight;
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
	void Cancel()
	{
		m_grid.ClearBoardData();
		bMove = false;
		bBasicAttack = false;
		bAbility1Attack = false;
		actionPointCostLabel.SetActive(false);
	}
}
