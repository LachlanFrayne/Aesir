﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BridalThor : Hero {

	public Image actionPointsBarImage;
	public Image healthBarImage;
	public Image backgroundThorImage;

	protected bool bBasicAttack = false;
	protected bool bAbility1Attack = false;
	public bool bThor = false;

	[Header("Material")]
	public Material AttackHighlight;
	public Material EnemyHighlight;

	void Start ()
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

		healthLabel.text = m_nHealth.ToString();
		healthMaxLabel.text = m_nHealthMax.ToString();
		actionPointLabel.text = m_nActionPoints.ToString();
		actionPointMaxLabel.text = m_nActionPointMax.ToString();

		base.Start();

		//gameObject.GetComponent<Thor>().enabled = false;

		SetTile();
	}

	void Update()
	{
		if(bThor)
		{			
			gameObject.GetComponent<Thor>().enabled = true;
			Thor thor = gameObject.GetComponent<Thor>();
			thor.actionPointCostLabel = actionPointCostLabel;
			thor.actionPointLabel = actionPointLabel;
			thor.actionPointMaxLabel = actionPointMaxLabel;
			thor.actionPointsMoveCostLabel = actionPointsMoveCostLabel;
			thor.healthLabel = healthLabel;
			thor.healthMaxLabel = healthMaxLabel;
			thor.actionPointsBarImage = actionPointsBarImage;
			thor.healthBarImage = healthBarImage;
			thor.backgroundThorImage = backgroundThorImage;
			this.enabled = false; 

		}
		if (bThorSelected)
		{
			if (m_nActionPoints > 1)        //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
				moveButton.onClick.AddListener(HighlightMovement);
			else
				moveButton.onClick.RemoveAllListeners();

			if (m_nActionPoints >= m_nBasicAttackCost)      //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
				basicAttackButton.onClick.AddListener(BasicAttack);
			else
				basicAttackButton.onClick.RemoveAllListeners();

			if (m_nActionPoints >= m_nAbility1AttackCost)       //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
				ability1Button.onClick.AddListener(Ability1);
			else
				ability1Button.onClick.RemoveAllListeners();

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

						if (bBasicAttack == true)
						{
							m_nActionPoints = m_nActionPoints - m_nBasicAttackCost;
							m_grid.ClearBoardData();
							hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nBasicAttackDamage;
							bBasicAttack = false;
						}
						if (bAbility1Attack == true)
						{
							m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
							m_grid.ClearBoardData();
							hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
							hit.collider.GetComponent<Enemy>().m_bStunned = true;
							bAbility1Attack = false;
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


	void BasicAttack()
	{
		m_grid.ClearBoardData();
		bMove = false;
		actionPointCostLabel.SetActive(true);
		actionPointsMoveCostLabel.text = m_nBasicAttackCost.ToString();
		bBasicAttack = true;

		dijkstrasSearchAttack(m_currentNode, m_nBasicAttackRange, AttackHighlight, 1);
	}
	void Ability1()
	{
		m_grid.ClearBoardData();
		bMove = false;
		actionPointCostLabel.SetActive(true);
		bAbility1Attack = true;
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
}