﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEditor;
using UnityEngine.UI;

public class BridalThor : Hero {

	public Image actionPointsBarImage;
	public Image healthBarImage;

	GameObject thorPlane;
	GameObject bridalThorPlane;

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

		thorPlane = GameObject.Find("ThorPlane");
		bridalThorPlane = GameObject.Find("BridalThorPlane");

		healthLabel.text = m_nHealth.ToString();
		healthMaxLabel.text = m_nHealthMax.ToString();

		actionPointLabel.text = m_nActionPoints.ToString();
		actionPointMaxLabel.text = m_nActionPointMax.ToString();

		base.Start();
		thorPlane.SetActive(false);
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
			thor.worldSpaceUI = worldSpaceUI;
			thor.m_currentNode = m_currentNode;
			bridalThorPlane.SetActive(false);
			thorPlane.SetActive(true);
			
			this.enabled = false; 

		}
		if (bThorSelected)
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

			if (bThorSelected)
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

		healthLabel.text = m_nHealth.ToString();      //Sets the health text to the amount of health left

		if (!bThorSelected)
		{
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
							FaceTowards(hit.collider.GetComponent<Entity>());

							StartCoroutine(basicAttack(m_animPresets[2], hit.collider.GetComponent<Enemy>()));
						}
						if (bAbility1Attack == true)
						{
							FaceTowards(hit.collider.GetComponent<Entity>());

							StartCoroutine(ability1Attack(m_animPresets[5], hit.collider.GetComponent<Enemy>()));
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
		bAbility1Attack = false;
		actionPointCostLabel.SetActive(true);
		actionPointsMoveCostLabel.text = m_nBasicAttackCost.ToString();
		bBasicAttack = true;

		dijkstrasSearchAttack(m_currentNode, m_nBasicAttackRange, AttackHighlight, 1);
	}
	void Ability1()
	{
		m_grid.ClearBoardData();
		bMove = false;
		bBasicAttack = false;
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

	IEnumerator ability1Attack(AnimationPreset anim, Enemy enemy)
	{
		StartCoroutine(RunAnim(anim));
		yield return new WaitForSeconds(anim.animationDuration);

		m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
		m_grid.ClearBoardData();
		enemy.m_nHealth = enemy.m_nHealth - m_nAbility1Attack;
		enemy.m_bStunned = true;
		bAbility1Attack = false;
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
