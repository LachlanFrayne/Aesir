using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Freya : Hero
{
    public int regen;

    public Node m_tempNode;

    public Image actionPointsBarImage;
    public Image backgroundFreyaImage;

    bool bBasicAttack = false;
    bool bAbility1Attack = false;

    [Header("Material")]
    public Material AttackHighlight;
    public Material EnemyHighlight;
    public Material pathUpDown;
    public Material pathLeftRight;
    public Material pathUpLeft;
    public Material pathUpRight;
    public Material pathDownLeft;
    public Material pathDownRight;
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
        backgroundFreyaImage = GameObject.Find("BackgroundFreya").GetComponent<Image>();

       
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
        if (FreyaSelected)
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

        if (FreyaSelected)
        {
            bBasicAttack = false;
            bAbility1Attack = false;
            backgroundFreyaImage.GetComponent<Image>().color = new Color32(255, 255, 0, 150);
        }
        if (!FreyaSelected)
        {
            backgroundFreyaImage.GetComponent<Image>().color = new Color32(255, 255, 0, 55);
            actionPointCostLabel.SetActive(false);
            dijkstrasSearchRemove(m_currentNode, m_nActionPoints, removeHighlight, m_nMovementActionPointCostPerTile);
        }


        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Enemy")
                {
                    actionPointCostLabel.SetActive(false);
                    RaycastHit hit2;
                    if (Physics.Raycast(hit.collider.transform.position, new Vector3(0, -1, 0), out hit2, 100))       //Creates a raycast downwards
                    {
                        if (hit2.collider.tag == "CurrentAttackableEnemyTile")
                        {
                            if (bBasicAttack == true)
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
                            if (bAbility1Attack == true)
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

    }

    void BasicAttack()
    {
        actionPointCostLabel.SetActive(true);
        actionPointsMoveCostLabel.text = m_nBasicAttackCost.ToString();
        bBasicAttack = true;

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
                    m_tempNode.GetComponent<Renderer>().material = EnemyHighlight;
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
                    m_tempNode.GetComponent<Renderer>().material = EnemyHighlight;

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
                    m_tempNode.GetComponent<Renderer>().material = EnemyHighlight;
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
                    m_tempNode.GetComponent<Renderer>().material = EnemyHighlight;
            }
            if (f > e)
                e++;
        }
    }
    void Ability1()
    {
        actionPointCostLabel.SetActive(true);
        bAbility1Attack = true;
        actionPointsMoveCostLabel.text = m_nAbility1AttackCost.ToString();

        dijkstrasSearchAttack(m_currentNode, 5, healingHighlight, 1);
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
