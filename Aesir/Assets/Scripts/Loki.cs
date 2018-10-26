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


    bool bBasicAttack = false;
    bool bAbility1Attack = false;

    [Header("Material")]
    public Material AttackHighlight;
    public Material EnemyHighlight;

    void Start()
    {
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

        if (bLokiSelected)
        {
            backgroundLokiImage.GetComponent<Image>().color = new Color32(0, 0, 255, 150);
        }
        if (!bLokiSelected)
        {
            backgroundLokiImage.GetComponent<Image>().color = new Color32(0, 0, 255, 55);
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
                        bAbility1Attack = false;
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

        for (int i = 0; i < m_currentNode.neighbours.Length; i++)
        {
            if (m_currentNode.neighbours[i].tag == "Tile")
            {
                m_currentNode.neighbours[i].GetComponent<Renderer>().material = AttackHighlight;
            }
            if (m_currentNode.neighbours[i].tag == "CurrentEnemyTile")
                m_currentNode.neighbours[i].GetComponent<Renderer>().material = EnemyHighlight;
        }                                       

        if (m_currentNode.neighbours[3].neighbours[0].tag == "Tile")
        {
            m_currentNode.neighbours[3].neighbours[0].GetComponent<Renderer>().material = AttackHighlight;
        }
        if (m_currentNode.neighbours[3].neighbours[0].tag == "CurrentEnemyTile")
            m_currentNode.neighbours[3].neighbours[0].GetComponent<Renderer>().material = EnemyHighlight;

        if (m_currentNode.neighbours[3].neighbours[2].tag == "Tile")
        {
            m_currentNode.neighbours[3].neighbours[2].GetComponent<Renderer>().material = AttackHighlight;
        }
        if (m_currentNode.neighbours[3].neighbours[2].tag == "CurrentEnemyTile")
            m_currentNode.neighbours[3].neighbours[2].GetComponent<Renderer>().material = EnemyHighlight;

        if (m_currentNode.neighbours[1].neighbours[0].tag == "Tile")
        {
            m_currentNode.neighbours[1].neighbours[0].GetComponent<Renderer>().material = AttackHighlight;
        }
        if (m_currentNode.neighbours[1].neighbours[0].tag == "CurrentEnemyTile")
            m_currentNode.neighbours[1].neighbours[0].GetComponent<Renderer>().material = EnemyHighlight;

        if (m_currentNode.neighbours[1].neighbours[2].tag == "Tile")
        {
            m_currentNode.neighbours[1].neighbours[2].GetComponent<Renderer>().material = AttackHighlight;
        }
        if (m_currentNode.neighbours[1].neighbours[2].tag == "CurrentEnemyTile")
            m_currentNode.neighbours[1].neighbours[2].GetComponent<Renderer>().material = EnemyHighlight;
    }
    void Ability1()
    {
        actionPointCostLabel.SetActive(true);
        bAbility1Attack = true;
        actionPointsMoveCostLabel.text = m_nAbility1AttackCost.ToString();


        int gScore = m_nMovementActionPointCostPerTile;
        Heap openList = new Heap(false);
        List<Node> closedList = new List<Node>();

        openList.Add(m_currentNode);

        while (openList.m_tHeap.Count > 0)
        {
            Node currentNode = openList.Pop();

            closedList.Add(currentNode);

            if (currentNode.m_gScore > m_nActionPoints)
            {
                continue;
            }
            if (currentNode.tag == "CurrentEnemyTile")
                continue;

            currentNode.GetComponent<Renderer>().sharedMaterial = movementHighlight;
            currentNode.tag = "TeleportableTile";

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
}
