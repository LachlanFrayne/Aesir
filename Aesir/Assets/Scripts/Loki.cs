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



	public bool bBasicAttack = false;
	public bool bAbility1Attack = false;

	[Header("Material")]
	public Material AttackHighlight;
	public Material EnemyHighlight;

	void Start()
	{
		worldSpaceUI = GameObject.Find("GameObject").GetComponent<WorldSpaceUI>();

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

			m_currentNode.gameObject.GetComponent<Renderer>().material = selectedHeroMat;
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
							m_nActionPoints = m_nActionPoints - m_nBasicAttackCost;
							m_grid.ClearBoardData();
							hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nBasicAttackDamage;
							bBasicAttack = false;
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
		bMove = false;
		bBasicAttack = false;
		actionPointCostLabel.SetActive(true);
		bAbility1Attack = true;
		actionPointsMoveCostLabel.text = m_nAbility1AttackCost.ToString();

		dijkstrasSearchAttack(m_currentNode, m_nAbility1AttackRange, AttackHighlight, 1);
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
