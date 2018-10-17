using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loki : Hero
{
    public Node m_tempNode;

    public GameObject moveSetButtons;

    public Image actionPointsBarImage;
    public Image backgroundFreyaImage;

    bool bBasicAttack = false;
    bool bAbility1Attack = false;

    List<Node> path = new List<Node>();

    Collider a;
    Collider b;

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
        actionPointCostLabel = GameObject.Find("Action Points Cost Loki");
        moveSetButtons = GameObject.Find("MoveSet");
        actionPointLabel = GameObject.Find("Action Points Loki").GetComponent<Text>();
        actionPointMaxLabel = GameObject.Find("Action Points Max Loki").GetComponent<Text>();
        actionPointsMoveCostLabel = GameObject.Find("Action Points Move Cost Loki").GetComponent<Text>();
        healthLabel = GameObject.Find("Health Loki").GetComponent<Text>();
        healthMaxLabel = GameObject.Find("Health Max Loki").GetComponent<Text>();
        actionPointsBarImage = GameObject.Find("Action Points Bar Loki").GetComponent<Image>();
        backgroundFreyaImage = GameObject.Find("BackgroundLoki").GetComponent<Image>();

        moveButton = GameObject.Find("Move").GetComponent<Button>();
        basicAttackButton = GameObject.Find("Basic Attack").GetComponent<Button>();
        ability1Button = GameObject.Find("Ability 1").GetComponent<Button>();


        moveSetButtons.SetActive(false);
        actionPointCostLabel.SetActive(false);

        m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grida>();        //Reference to Grida

        healthLabel.text = m_nHealth.ToString();
        healthMaxLabel.text = m_nHealthMax.ToString();
        actionPointLabel.text = m_nActionPoints.ToString();
        actionPointMaxLabel.text = m_nActionPointMax.ToString();

        SetTile();
    }
    
    void Update()
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

        actionPointsBarImage.fillAmount = (1f / m_nActionPointMax) * m_nActionPoints;       //Sets the amount of the actionPointsBar
        actionPointLabel.text = m_nActionPoints.ToString();      //Sets the ActionPoint text to the amount of actionPoints

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Loki")     //If click on character, show selection tab
                {
                    if (FreyaSelected == false && ThorSelected == false)
                        LokiSelected = true;

                    if (LokiSelected)
                    {
                        LokiSelected = true;

                        bBasicAttack = false;
                        bAbility1Attack = false;

                        backgroundFreyaImage.GetComponent<Image>().color = new Color32(0, 0, 255, 150);

                        moveSetButtons.SetActive(true);
                    }
                }
                if (hit.collider.tag == "Thor")     //If you click on another character, unhighlight 
                {
                    if (LokiSelected)
                    {
                        FreyaSelected = false;
                        ThorSelected = true;
                        actionPointCostLabel.SetActive(false);
                        backgroundFreyaImage.GetComponent<Image>().color = new Color32(0, 0, 255, 55);
                        dijkstrasSearchRemove(m_currentNode, m_nActionPoints, removeHighlight, m_nMovementActionPointCostPerTile);
                    }
                }
                if (hit.collider.tag == "Freya")     //If you click on another character, unhighlight    
                {
                    if (LokiSelected)
                    {
                        FreyaSelected = false;
                        LokiSelected = true;
                        actionPointCostLabel.SetActive(false);
                        backgroundFreyaImage.GetComponent<Image>().color = new Color32(0, 0, 255, 55);
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
    }
    void HighlightMovement()
    {
        dijkstrasSearch(m_currentNode, m_nActionPoints, movementHighlight, m_nMovementActionPointCostPerTile);

        moveSetButtons.SetActive(false);
    }

    void BasicAttack()
    {
        moveSetButtons.SetActive(false);

        actionPointCostLabel.SetActive(true);
        actionPointsMoveCostLabel.text = m_nBasicAttackCost.ToString();
        bBasicAttack = true;

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
    void Ability1()
    {
        moveSetButtons.SetActive(false);

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
                if (m_tempNode.tag == "FreyaAttackableTile")
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
                if (m_tempNode.tag == "FreyaAttackableTile")
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
                if (m_tempNode.tag == "FreyaAttackableTile")
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
                if (m_tempNode.tag == "FreyaAttackableTile")
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
