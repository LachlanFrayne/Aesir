using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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



    bool bBridalBasicAttack = false;
    bool bBridalAbility1Attack = false;
    bool bThorBasicAttack = false;
    bool bThorAbility1Attack = false;
    bool bThorAbility2Attack = false;
    public bool bBridal = true;


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
        if(m_nHealth <= 0)
            Destroy(this.gameObject);

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
                    m_nBasicAttack = m_nThorBasicAttack;
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
                    actionPointCostLabel.SetActive(false);

                    if (bBridalBasicAttack == true)
                    {
                        m_nActionPoints = m_nActionPoints - m_nBasicAttackCost;
                        m_grid.ClearBoardData();
                        hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nBasicAttack;
                        bBridalBasicAttack = false;
                    }
                    if (bBridalAbility1Attack == true)
                    {
                        m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
                        m_grid.ClearBoardData();
                        bBridalAbility1Attack = false;
                    }

                    if (bThorBasicAttack == true)
                    {
                        m_nActionPoints = m_nActionPoints - m_nBasicAttackCost;
                        m_grid.ClearBoardData();
                        hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nBasicAttack;
                        bThorBasicAttack = false;
                    }
                    if (bThorAbility1Attack == true)
                    {
                        m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
                        m_grid.ClearBoardData();
                        hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
                        bThorAbility1Attack = false;
                    }
                    if (bThorAbility2Attack == true)
                    {
                        m_nActionPoints = m_nActionPoints - m_nAbility2AttackCost;
                        m_grid.ClearBoardData();
                        hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
                        bThorAbility2Attack = false;
                    }
                }
            }
        }
        base.Update();
    }

    void HighlightMovement()
    {
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
        actionPointCostLabel.SetActive(true);
        bBridalAbility1Attack = true;
        actionPointsMoveCostLabel.text = m_nAbility1AttackCost.ToString();

        for (int i = 0; i < m_currentNode.neighbours.Length; i++)
        {
            if (m_currentNode.neighbours[i].tag == "Tile")
            {
                m_currentNode.neighbours[i].GetComponent<Renderer>().material = AttackHighlight;
                m_currentNode.neighbours[i].tag = "AttackableTile";
            }
            if (m_currentNode.neighbours[i].tag == "CurrentEnemyTile")
                m_currentNode.neighbours[i].GetComponent<Renderer>().material = EnemyHighlight;
        }

        if (m_currentNode.neighbours[3].neighbours[0].tag == "Tile")
        {
            m_currentNode.neighbours[3].neighbours[0].GetComponent<Renderer>().material = AttackHighlight;
            m_currentNode.neighbours[3].neighbours[0].tag = "AttackableTile";
        }
        if (m_currentNode.neighbours[3].neighbours[0].tag == "CurrentEnemyTile")
            m_currentNode.neighbours[3].neighbours[0].GetComponent<Renderer>().material = EnemyHighlight;

        if (m_currentNode.neighbours[3].neighbours[2].tag == "Tile")
        {
            m_currentNode.neighbours[3].neighbours[2].GetComponent<Renderer>().material = AttackHighlight;
            m_currentNode.neighbours[3].neighbours[2].tag = "AttackableTile";
        }
        if (m_currentNode.neighbours[3].neighbours[2].tag == "CurrentEnemyTile")
            m_currentNode.neighbours[3].neighbours[2].GetComponent<Renderer>().material = EnemyHighlight;

        if (m_currentNode.neighbours[1].neighbours[0].tag == "Tile")
        {
            m_currentNode.neighbours[1].neighbours[0].GetComponent<Renderer>().material = AttackHighlight;
            m_currentNode.neighbours[1].neighbours[0].tag = "AttackableTile";
        }
        if (m_currentNode.neighbours[1].neighbours[0].tag == "CurrentEnemyTile")
            m_currentNode.neighbours[1].neighbours[0].GetComponent<Renderer>().material = EnemyHighlight;

        if (m_currentNode.neighbours[1].neighbours[2].tag == "Tile")
        {
            m_currentNode.neighbours[1].neighbours[2].GetComponent<Renderer>().material = AttackHighlight;
            m_currentNode.neighbours[1].neighbours[2].tag = "AttackableTile";
        }
        if (m_currentNode.neighbours[1].neighbours[2].tag == "CurrentEnemyTile")
            m_currentNode.neighbours[1].neighbours[2].GetComponent<Renderer>().material = EnemyHighlight;
    }
    void ThorBasicAttack()
    {
        actionPointCostLabel.SetActive(true);
        bThorBasicAttack = true;
        actionPointsMoveCostLabel.text = m_nBasicAttack.ToString();

        for (int i = 0; i < m_currentNode.neighbours.Length; i++)
        {
            if (m_currentNode.neighbours[i].tag == "Tile")
            {
                m_currentNode.neighbours[i].GetComponent<Renderer>().material = AttackHighlight;
                m_currentNode.neighbours[i].tag = "AttackableTile";
            }
            if (m_currentNode.neighbours[i].tag == "CurrentEnemyTile")
                m_currentNode.neighbours[i].GetComponent<Renderer>().material = EnemyHighlight;
        }
    }
    void ThorAbility1()
    {
        dijkstrasSearchAttack(m_currentNode, 4, movementHighlight, 1);
    }
    void ThorAbility2()
    {

    }
}
        
    

