﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Freya : Hero
{
    public int regen;

    public Node m_tempNode;

    public Image actionPointsBarImage;
    public Image healthBarImage;

    [Header("Material")]
    public Material AttackHighlight;
    public Material EnemyHighlight;
    public Material healingHighlight;

    void Start()
    {

		actionPointCostLabel = GameObject.Find("Action Points Cost Freya");       
        actionPointLabel = GameObject.Find("Action Points Freya").GetComponent<Text>();
        actionPointMaxLabel = GameObject.Find("Action Points Max Freya").GetComponent<Text>();
        actionPointsMoveCostLabel = GameObject.Find("Action Points Move Cost Freya").GetComponent<Text>();
        healthLabel = GameObject.Find("Health Freya").GetComponent<Text>();
        healthMaxLabel = GameObject.Find("Health Max Freya").GetComponent<Text>();
        actionPointsBarImage = GameObject.Find("Action Points Bar Freya").GetComponent<Image>();
        healthBarImage = GameObject.Find("Health Bar Freya").GetComponent<Image>();
   
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
        if (bFreyaSelected)
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

			if (bFreyaSelected)
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


        if (!bFreyaSelected)
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
					}
				}
				else if (hit.collider.GetComponentInParent<Loki>() != null)
				{
					if (bAbility1Attack == true)
					{
						FaceTowards(hit.collider.GetComponent<Entity>());
						StartCoroutine(ability1Attack(m_animPresets[5], hit.collider.GetComponentInParent<Loki>()));						
					}
				}
				else if(hit.collider.GetComponentInParent<Thor>() != null)
				{
					if (hit.collider.GetComponentInParent<Thor>().enabled == false)
					{
						if (bAbility1Attack == true)
						{
							FaceTowards(hit.collider.GetComponent<Entity>());
							StartCoroutine(ability1Attack(m_animPresets[5], hit.collider.GetComponentInParent<BridalThor>()));
						}
					}
					else
					{
						if (bAbility1Attack == true)
						{
							FaceTowards(hit.collider.GetComponent<Entity>());
							StartCoroutine(ability1Attack(m_animPresets[5], hit.collider.GetComponentInParent<Thor>()));
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
		bAttacking = true;
		loki.bAttacking = true;
		bridalThor.bAttacking = true;
		bMove = false;
		bBasicAttack = false;
        actionPointCostLabel.SetActive(true);
        bAbility1Attack = true;
        actionPointsMoveCostLabel.text = m_nAbility1AttackCost.ToString();

        dijkstrasSearchHealing(m_currentNode, m_nAbility1AttackRange, healingHighlight, 1);
    }

    public void dijkstrasSearchHealing(Node startNode, int actionPointAvailable, Material healingHighlight, int MoveCostPerTile)
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

	IEnumerator ability1Attack(AnimationPreset anim, Hero hero)
	{
		StartCoroutine(RunAnim(anim));
		yield return new WaitForSeconds(anim.animationDuration);

		hero.m_nHealth += m_nAbility1Attack;
		m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
		m_grid.ClearBoardData();
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
