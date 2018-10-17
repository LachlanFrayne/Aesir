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
    public Material pathUpDown;
    public Material pathLeftRight;
    public Material pathUpLeft;
    public Material pathUpRight;
    public Material pathDownLeft;
    public Material pathDownRight;

    void Start()
    {
        actionPointCostLabel = GameObject.Find("Action Points Cost Thor");
        
        actionPointLabel = GameObject.Find("Action Points Thor").GetComponent<Text>();
        actionPointMaxLabel = GameObject.Find("Action Points Max Thor").GetComponent<Text>();
        actionPointsMoveCostLabel = GameObject.Find("Action Points Move Cost Thor").GetComponent<Text>();
        healthLabel = GameObject.Find("Health Thor").GetComponent<Text>();
        healthMaxLabel = GameObject.Find("Health Max Thor").GetComponent<Text>();
        actionPointsBarImage = GameObject.Find("Action Points Bar Thor").GetComponent<Image>();
        backgroundThorImage = GameObject.Find("BackgroundThor").GetComponent<Image>();

        actionPointCostLabel.SetActive(false);

        healthLabel.text = m_nHealth.ToString();
        healthMaxLabel.text = m_nHealthMax.ToString();
        actionPointLabel.text = m_nActionPoints.ToString();
        actionPointMaxLabel.text = m_nActionPointMax.ToString();

        base.Start();
        //moveSetButtons.SetActive(false);

        SetTile();
    }


    void Update()
    {
        if(ThorSelected)
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
        else
        {
            moveButton.onClick.RemoveAllListeners();
            basicAttackButton.onClick.RemoveAllListeners();
            ability1Button.onClick.RemoveAllListeners();
            ability2Button.onClick.RemoveAllListeners();
        }

        actionPointsBarImage.fillAmount = (1f / m_nActionPointMax) * m_nActionPoints;       //Sets the amount of the actionPointsBar
        actionPointLabel.text = m_nActionPoints.ToString();      //Sets the ActionPoint text to the amount of actionPoints



        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Thor")     //If click on character, show selection tab
                {
                    if (LokiSelected == false && FreyaSelected == false)
                        ThorSelected = true;

                    if (ThorSelected)
                    {
                        ThorSelected = true;

                        bBridalBasicAttack = false;
                        bBridalAbility1Attack = false;
                        bThorBasicAttack = false;
                        bThorAbility1Attack = false;
                        bThorAbility2Attack = false;

                        backgroundThorImage.GetComponent<Image>().color = new Color32(255, 0, 0, 150);

                        moveSetButtons.SetActive(true);
                    }
                }
                if (hit.collider.tag == "Loki")     //If you click on another character, unhighlight 
                {
                    if (ThorSelected)
                    {
                        ThorSelected = false;
                        LokiSelected = true;
                        backgroundThorImage.GetComponent<Image>().color = new Color32(255, 0, 0, 55);
                        actionPointCostLabel.SetActive(false);
                        dijkstrasSearchRemove(m_currentNode, m_nActionPoints, removeHighlight, m_nMovementActionPointCostPerTile);
                    }
                }
                if (hit.collider.tag == "Freya")     //If you click on another character, unhighlight    
                {
                    if (ThorSelected)
                    {
                        ThorSelected = false;
                        FreyaSelected = true;
                        backgroundThorImage.GetComponent<Image>().color = new Color32(255, 0, 0, 55);
                        actionPointCostLabel.SetActive(false);
                        dijkstrasSearchRemove(m_currentNode, m_nActionPoints, removeHighlight, m_nMovementActionPointCostPerTile);
                    }
                }

                if (hit.collider.tag == "Enemy")
                {
                    actionPointCostLabel.SetActive(false);

                    RaycastHit hit2;
                    if (Physics.Raycast(hit.collider.transform.position, new Vector3(0, -1, 0), out hit2, 100))       //Creates a raycast downwards
                    {
                        if (hit2.collider.tag == "CurrentAttackableEnemyTile")
                        {
                            if (bBridalBasicAttack == true)
                            {
                                m_nActionPoints = m_nActionPoints - m_nBasicAttackCost;
                                RemoveHighlightAttack();
                                hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nBasicAttack;

                                hit2.collider.GetComponent<Renderer>().material = removeHighlight;
                                if (hit.collider.GetComponent<Enemy>().m_nHealth > 0)

                                    hit2.collider.tag = "CurrentEnemyTile";
                                else
                                    hit2.collider.tag = "Tile";
                            }
                            if (bBridalAbility1Attack == true)
                            {
                                m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
                                RemoveHighlightAttack();
                                hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;

                                hit2.collider.GetComponent<Renderer>().material = removeHighlight;
                                if (hit.collider.GetComponent<Enemy>().m_nHealth > 0)

                                    hit2.collider.tag = "CurrentEnemyTile";
                                else
                                    hit2.collider.tag = "Tile";
                            }

                            if (bThorBasicAttack == true)
                            {
                                m_nActionPoints = m_nActionPoints - m_nBasicAttackCost;
                                RemoveHighlightAttack();
                                hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nBasicAttack;

                                hit2.collider.GetComponent<Renderer>().material = removeHighlight;
                                if (hit.collider.GetComponent<Enemy>().m_nHealth > 0)

                                    hit2.collider.tag = "CurrentEnemyTile";
                                else
                                    hit2.collider.tag = "Tile";
                            }
                            if (bThorAbility1Attack == true)
                            {
                                m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
                                RemoveHighlightAttack();
                                hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;

                                hit2.collider.GetComponent<Renderer>().material = removeHighlight;
                                if (hit.collider.GetComponent<Enemy>().m_nHealth > 0)

                                    hit2.collider.tag = "CurrentEnemyTile";
                                else
                                    hit2.collider.tag = "Tile";
                            }
                            if (bThorAbility2Attack == true)
                            {
                                m_nActionPoints = m_nActionPoints - m_nAbility2AttackCost;
                                RemoveHighlightAttack();
                                hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;

                                hit2.collider.GetComponent<Renderer>().material = removeHighlight;
                                if (hit.collider.GetComponent<Enemy>().m_nHealth > 0)

                                    hit2.collider.tag = "CurrentEnemyTile";
                                else
                                    hit2.collider.tag = "Tile";
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
        dijkstrasSearch(m_currentNode, m_nActionPoints, movementHighlight, m_nMovementActionPointCostPerTile);

        moveSetButtons.SetActive(false);
    }



    /// <summary>
    /// ///////////////////////////////////////////////////ATTACKS/////////////////////////////////////////////////////////////////////////////
    /// </summary>


    void BridalBasicAttack()
    {
        moveSetButtons.SetActive(false);

        actionPointCostLabel.SetActive(true);
        actionPointsMoveCostLabel.text = m_nBasicAttackCost.ToString();
        bBridalBasicAttack = true;

        m_tempNode = m_tempNodeBase;        //Sets base 
        int f = m_nBasicAttackRange;      //The attackRange
        int e = 1;

        for (int i = 0; i < e; i++)
        {
            m_tempNode = m_tempNode.neighbours[3];
            for (int a = 0; a < e; a++)
            {
                m_tempNode = m_tempNode.neighbours[1].neighbours[0];
                if (m_tempNode.tag == "Tile")
                {
                    m_tempNode.GetComponent<Renderer>().sharedMaterial = AttackHighlight;
                    m_tempNode.tag = "AttackableTile";
                }
                if (m_tempNode.tag == "CurrentEnemyTile")
                {
                    m_tempNode.GetComponent<Renderer>().material = EnemyHighlight;
                    m_tempNode.tag = "CurrentAttackableEnemyTile";
                }
            }

            for (int b = 0; b < e; b++)
            {
                m_tempNode = m_tempNode.neighbours[2].neighbours[1];
                if (m_tempNode.tag == "Tile")
                {
                    m_tempNode.GetComponent<Renderer>().sharedMaterial = AttackHighlight;
                    m_tempNode.tag = "AttackableTile";
                }
                if (m_tempNode.tag == "CurrentEnemyTile")
                {
                    m_tempNode.GetComponent<Renderer>().material = EnemyHighlight;
                    m_tempNode.tag = "CurrentAttackableEnemyTile";
                }

            }

            for (int c = 0; c < e; c++)
            {
                m_tempNode = m_tempNode.neighbours[3].neighbours[2];
                if (m_tempNode.tag == "Tile")
                {
                    m_tempNode.GetComponent<Renderer>().sharedMaterial = AttackHighlight;
                    m_tempNode.tag = "AttackableTile";
                }
                if (m_tempNode.tag == "CurrentEnemyTile")
                {
                    m_tempNode.GetComponent<Renderer>().material = EnemyHighlight;
                    m_tempNode.tag = "CurrentAttackableEnemyTile";
                }
            }

            for (int d = 0; d < e; d++)
            {
                m_tempNode = m_tempNode.neighbours[0].neighbours[3];
                if (m_tempNode.tag == "Tile")
                {
                    m_tempNode.GetComponent<Renderer>().sharedMaterial = AttackHighlight;
                    m_tempNode.tag = "AttackableTile";
                }
                if (m_tempNode.tag == "CurrentEnemyTile")
                {
                    m_tempNode.GetComponent<Renderer>().material = EnemyHighlight;
                    m_tempNode.tag = "CurrentAttackableEnemyTile";
                }
            }
            if (f > e)
                e++;
        }
    }
    void BridalAbility1()
    {
        moveSetButtons.SetActive(false);
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
        moveSetButtons.SetActive(false);

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
        moveSetButtons.SetActive(false);

        dijkstrasSearchAttack(m_currentNode, 4, movementHighlight, 1);
    }
    void ThorAbility2()
    {

    }
    

    void RemoveHighlightAttack()
    {
            m_tempNode = m_tempNodeBase;
            int f = m_nBasicAttackRange;
            int e = 1;

        for (int i = 0; i < e; i++)
        {
            m_tempNode = m_tempNode.neighbours[3];
            m_tempNode.GetComponent<Renderer>().sharedMaterial = removeHighlight;
            for (int a = 0; a < e; a++)
            {
                m_tempNode = m_tempNode.neighbours[1].neighbours[0];
                if (m_tempNode.tag == "AttackableTile")
                {
                    m_tempNode.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                    m_tempNode.tag = "Tile";
                }
                else if (m_tempNode.tag == "CurrentEnemyTile")
                {
                    m_tempNode.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                }
            }
            for (int b = 0; b < e; b++)
            {
                m_tempNode = m_tempNode.neighbours[2].neighbours[1];
                if (m_tempNode.tag == "AttackableTile")
                {
                    m_tempNode.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                    m_tempNode.tag = "Tile";
                }
                else if (m_tempNode.tag == "CurrentEnemyTile")
                {
                    m_tempNode.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                }

            }
            for (int c = 0; c < e; c++)
            {

                m_tempNode = m_tempNode.neighbours[3].neighbours[2];
                if (m_tempNode.tag == "AttackableTile")
                {
                    m_tempNode.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                    m_tempNode.tag = "Tile";
                }
                else if (m_tempNode.tag == "CurrentEnemyTile")
                {
                    m_tempNode.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                }
            }
            for (int d = 0; d < e; d++)
            {
                m_tempNode = m_tempNode.neighbours[0].neighbours[3];
                if (m_tempNode.tag == "AttackableTile")
                {
                    m_tempNode.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                    m_tempNode.tag = "Tile";
                }
                else if (m_tempNode.tag == "CurrentEnemyTile")
                {
                    m_tempNode.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                }
            }
            if (f > e)
                e++;
        }
    }
}
        
    

