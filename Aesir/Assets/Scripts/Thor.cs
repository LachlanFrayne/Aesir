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

    Grida m_grid;

    public Node m_currentNode;
    public Node m_tempNode;
    public Node m_tempNodeBase;

    public GameObject actionPointCostLabel;
    public GameObject bridalThorMoveSetButtons;
    public GameObject thorMoveSetButtons;

    public Text actionPointLabel;
    public Text actionPointMaxLabel;
    public Text actionPointsMoveCostLabel;
    public Text healthLabel;
    public Text healthMaxLabel;

    public Image actionPointsBarImage;
    public Image backgroundThorImage;

    public Button bridalMoveButton;
    public Button bridalBasicAttackButton;
    public Button bridalAbility1Button;
    public Button moveButton;
    public Button basicAttackButton;
    public Button ability1Button;
    public Button ability2Button;

    bool bBridalBasicAttack = false;
    bool bBridalAbility1Attack = false;
    bool bThorBasicAttack = false;
    bool bThorAbility1Attack = false;
    bool bThorAbility2Attack = false;
    public bool bBridal = true;



    Hero hero;

    List<Node> path = new List<Node>();

    Collider a;
    Collider b;

    [Header("Material")]
    public Material movementHighlight;
    public Material removeHighlight;
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
        bridalThorMoveSetButtons = GameObject.Find("BridalThorMoveSet");
        thorMoveSetButtons = GameObject.Find("ThorMoveSet");
        actionPointLabel = GameObject.Find("Action Points Thor").GetComponent<Text>();
        actionPointMaxLabel = GameObject.Find("Action Points Max Thor").GetComponent<Text>();
        actionPointsMoveCostLabel = GameObject.Find("Action Points Move Cost Thor").GetComponent<Text>();
        healthLabel = GameObject.Find("Health Thor").GetComponent<Text>();
        healthMaxLabel = GameObject.Find("Health Max Thor").GetComponent<Text>();
        actionPointsBarImage = GameObject.Find("Action Points Bar Thor").GetComponent<Image>();
        backgroundThorImage = GameObject.Find("BackgroundThor").GetComponent<Image>();

        bridalMoveButton = GameObject.Find("Move Bridal").GetComponent<Button>();
        bridalBasicAttackButton = GameObject.Find("Basic Attack Bridal").GetComponent<Button>();
        bridalAbility1Button = GameObject.Find("Ability 1 Bridal").GetComponent<Button>();
        
        moveButton = GameObject.Find("Move Thor").GetComponent<Button>();
        basicAttackButton = GameObject.Find("Basic Attack Thor").GetComponent<Button>();
        ability1Button = GameObject.Find("Ability 1 Thor").GetComponent<Button>();
        ability2Button = GameObject.Find("Ability 2 Thor").GetComponent<Button>();


        bridalThorMoveSetButtons.SetActive(false);
        thorMoveSetButtons.SetActive(false);
        actionPointCostLabel.SetActive(false);

        m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grida>();        //Reference to Grida

        healthLabel.text = m_nHealth.ToString();
        healthMaxLabel.text = m_nHealthMax.ToString();
        actionPointLabel.text = m_nActionPoints.ToString();
        actionPointMaxLabel.text = m_nActionPointMax.ToString();

        hero = new Hero();

        SetTile();
    }
    void SetTile()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 100))       //Creates a raycast downwards
        {
            for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)     //Goes through the grid 
            {
                for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                {
                    if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile].self)        //Goes through the grid until it finds the tiles the character is on         
                    {
                        m_currentNode = m_grid.boardArray[columnTile, rowTile];        //Sets currentNode to the tile the character is on
                        m_currentNode.self.tag = "CurrentTile";        //Sets currentNodes tag to "CurrentTile"
                        transform.position = new Vector3(m_currentNode.self.transform.position.x, .5f, m_currentNode.self.transform.position.z);        //Sets position to the center of the tile
                        m_tempNodeBase = m_currentNode;        //Creates a temp node on the currentNode
                    }
                }
            }
        }
    }

    void Update()
    {
        if (bBridal)
        {
            if (m_nActionPoints > 1)        //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
                bridalMoveButton.onClick.AddListener(HighlightMovement);
            else
                bridalMoveButton.onClick.RemoveAllListeners();

            if (m_nActionPoints >= m_nBasicAttackCost)      //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
                bridalBasicAttackButton.onClick.AddListener(BridalBasicAttack);
            else
                bridalBasicAttackButton.onClick.RemoveAllListeners();

            if (m_nActionPoints >= m_nAbility1AttackCost)       //If you have enough actionPoints, add a listener, if you don't have enough remove the listener
                bridalAbility1Button.onClick.AddListener(BridalAbility1);
            else
                bridalAbility1Button.onClick.RemoveAllListeners();
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

            
            if (i == 0)
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


        actionPointsBarImage.fillAmount = (1f / m_nActionPointMax) * m_nActionPoints;       //Sets the amount of the actionPointsBar
        actionPointLabel.text = m_nActionPoints.ToString();      //Sets the ActionPoint text to the amount of actionPoints

        Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit1;
        if (Physics.Raycast(ray1, out hit1, 100))
        {
            if (hit1.collider.tag == "WalkableTile")
            {
                for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
                {
                    for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                    {
                        if (hit1.collider.gameObject == m_grid.boardArray[columnTile, rowTile].self)
                        {
                            actionPointCostLabel.SetActive(true);
                            actionPointsMoveCostLabel.text = m_grid.boardArray[columnTile, rowTile].gScore.ToString();       //Sets the ActionPointCost to the gScore of the tile

                            if (a == null)
                                a = hit1.collider;      //a = the first hit
                            else
                                b = hit1.collider;      //b = the second hit

                            if (a != b)         //if the first hit is different then the second hit
                            {
                                foreach (Node tile in path)      //Goes through all tiles in the path and sets them back to walkable
                                {
                                    tile.self.GetComponent<Renderer>().material = movementHighlight;
                                    tile.self.tag = "WalkableTile";
                                }
                                a = null;       //Resets a 
                                b = null;       //Resets b
                                path.Clear();       //Clears the path list
                            }
                            Node temp;      //Creates a temp node
                            temp = m_grid.boardArray[columnTile, rowTile];      //Sets it to the hit node

                            while (temp.prev != null)
                            {
                                temp.self.GetComponent<Renderer>().material.color = Color.green;
                                path.Add(temp);        //Adds node to path
                                temp = temp.prev;       //Set temp to temp.prev 
                            }
                        }
                    }
                }
            }
            if (hit1.collider.tag == "ThorAttackableTile")
            {
                for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
                {
                    for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                    {
                        if (hit1.collider.gameObject == m_grid.boardArray[columnTile, rowTile].self)
                        {                           
                            if (a == null)
                                a = hit1.collider;      //a = the first hit
                            else
                                b = hit1.collider;      //b = the second hit

                            if (a != b)         //if the first hit is different then the second hit
                            {
                               
                                foreach (Node tile in path)      //Goes through all tiles in the path and sets them back to walkable
                                {
                                    tile.self.GetComponent<Renderer>().material = movementHighlight;
                                    tile.self.tag = "ThorAttackableTile";
                                }
                                a = null;       //Resets a 
                                b = null;       //Resets b
                                path.Clear();       //Clears the path list
                                j = 0;
                            }
                            Node temp;      //Creates a temp node
                            temp = m_grid.boardArray[columnTile, rowTile];      //Sets it to the hit node

                            while (temp.prev != null)
                            {
                                temp.self.GetComponent<Renderer>().material.color = Color.green;
                                path.Add(temp);        //Adds node to path
                                

                                if (j == 0)
                                {
                                    for (int i = 0; i < temp.neighbours.Length; i++)
                                    {
                                        if (temp.neighbours[i].self.tag == "ThorAttackableTile")
                                        {
                                            temp.neighbours[i].self.GetComponent<Renderer>().material = AttackHighlight;
                                            temp.neighbours[i].self.tag = "ThorAttackableTile";
                                        }
                                        if (temp.neighbours[i].self.tag == "CurrentEnemyTile")
                                            temp.neighbours[i].self.GetComponent<Renderer>().material = EnemyHighlight;

                                        path.Add(temp.neighbours[i]);
                                    }

                                    if (temp.neighbours[3].neighbours[0].self.tag == "ThorAttackableTile")
                                    {
                                        temp.neighbours[3].neighbours[0].self.GetComponent<Renderer>().material = AttackHighlight;
                                    }
                                    if (temp.neighbours[3].neighbours[0].self.tag == "CurrentEnemyTile")
                                        temp.neighbours[3].neighbours[0].self.GetComponent<Renderer>().material = EnemyHighlight;

                                    path.Add(temp.neighbours[3].neighbours[0]);

                                    if (temp.neighbours[3].neighbours[2].self.tag == "ThorAttackableTile")
                                    {
                                        temp.neighbours[3].neighbours[2].self.GetComponent<Renderer>().material = AttackHighlight;
                                    }
                                    if (temp.neighbours[3].neighbours[2].self.tag == "CurrentEnemyTile")
                                        temp.neighbours[3].neighbours[2].self.GetComponent<Renderer>().material = EnemyHighlight;

                                    path.Add(temp.neighbours[3].neighbours[2]);

                                    if (temp.neighbours[1].neighbours[0].self.tag == "ThorAttackableTile")
                                    {
                                        temp.neighbours[1].neighbours[0].self.GetComponent<Renderer>().material = AttackHighlight;
                                    }
                                    if (temp.neighbours[1].neighbours[0].self.tag == "CurrentEnemyTile")
                                        temp.neighbours[1].neighbours[0].self.GetComponent<Renderer>().material = EnemyHighlight;

                                    path.Add(temp.neighbours[1].neighbours[0]);

                                    if (temp.neighbours[1].neighbours[2].self.tag == "ThorAttackableTile")
                                    {
                                        temp.neighbours[1].neighbours[2].self.GetComponent<Renderer>().material = AttackHighlight;
                                    }
                                    if (temp.neighbours[1].neighbours[2].self.tag == "CurrentEnemyTile")
                                        temp.neighbours[1].neighbours[2].self.GetComponent<Renderer>().material = EnemyHighlight;
                                    path.Add(temp.neighbours[1].neighbours[2]);

                                    j++;
                                }
                                temp = temp.prev;       //Set temp to temp.prev 
                            }


                            




                        }
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Thor")     //If click on character, show selection tab
                {
                    if (hero.LokiSelected == false && hero.FreyaSelected == false)
                        hero.ThorSelected = true;

                    if (hero.ThorSelected)
                    {
                        hero.ThorSelected = true;

                        bBridalBasicAttack = false;
                        bBridalAbility1Attack = false;
                        bThorBasicAttack = false;
                        bThorAbility1Attack = false;
                        bThorAbility2Attack = false;
                        //hero.dijkstrasSearchRemove(m_currentNode, m_nActionPoints, removeHighlight);
                        //RemoveHighlightAttack();
                        backgroundThorImage.GetComponent<Image>().color = new Color32(255, 0, 0, 150);

                        if (bBridal)
                            bridalThorMoveSetButtons.SetActive(true);
                        else
                            thorMoveSetButtons.SetActive(true);
                    }
                }
                if (hit.collider.tag == "Loki")     //If you click on another character, unhighlight 
                {
                    if (hero.ThorSelected)
                    {
                        hero.ThorSelected = false;
                        hero.LokiSelected = true;
                        backgroundThorImage.GetComponent<Image>().color = new Color32(255, 0, 0, 55);
                        actionPointCostLabel.SetActive(false);
                        hero.ChangeCharacters("Thor", "Loki");
                    }
                }
                if (hit.collider.tag == "Freya")     //If you click on another character, unhighlight    
                {
                    if (hero.ThorSelected)
                    {
                        hero.ThorSelected = false;
                        hero.FreyaSelected = true;
                        backgroundThorImage.GetComponent<Image>().color = new Color32(255, 0, 0, 55);
                        actionPointCostLabel.SetActive(false);
                        hero.ChangeCharacters("Thor", "Freya");
                    }
                }



                if (hit.collider.tag == "WalkableTile" && hero.ThorSelected == true)        //Used for when you are moving
                {
                    transform.position = new Vector3(hit.collider.GetComponent<MeshRenderer>().bounds.center.x, 0.5f, hit.collider.GetComponent<MeshRenderer>().bounds.center.z);       //Moves player to hit tile
                    for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
                    {
                        for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                        {
                            if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile].self)
                            {
                                Node tempNode = m_currentNode;      //Creates a tempNode and sets it to currentNode
                                int tempActionPoints = m_nActionPoints;         //Creates a tempActionPoints and sets it to ActionPoints
                                m_nActionPoints = m_nActionPoints - m_grid.boardArray[columnTile, rowTile].gScore;        //Sets ActionPoints to ActionPoints - hit tile gscore

                                hero.dijkstrasSearchRemove(tempNode, tempActionPoints, removeHighlight, m_nMovementActionPointCostPerTile);        //Removes the highlight
                                m_currentNode = m_grid.boardArray[columnTile, rowTile];        //Sets currentNode to the hit tile
                                m_currentNode.self.tag = "CurrentTile";        //Sets currentNode tag = "CurrentTIle"
                                m_tempNodeBase = m_currentNode;        //Sets tempNodeBase to new currentNode
                                actionPointCostLabel.SetActive(false);
                                foreach (Node tile in path)      //Goes through all tiles in the path and removes the material and tag 
                                {
                                    tile.self.GetComponent<Renderer>().material = removeHighlight;
                                    tile.self.tag = "Tile";
                                }
                                a = null;       //Resets a
                                b = null;       //Resets b
                                path.Clear();       //Clears the path list
                                SetTile();
                            }
                        }
                    }
                }

                if (hit.collider.tag == "Enemy" && bBridalBasicAttack == true || hit.collider.tag == "Enemy" && bBridalAbility1Attack == true)
                {
                    actionPointCostLabel.SetActive(false);
                    if (bBridalBasicAttack == true)
                    {
                        m_nActionPoints = m_nActionPoints - m_nBasicAttackCost;
                        RemoveHighlightAttack();
                        hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nBasicAttack;
                    }
                    if (bBridalAbility1Attack == true)
                    {
                        m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
                        RemoveHighlightAttack();
                        hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
                    }
                }
                if (hit.collider.tag == "Enemy" && bThorBasicAttack == true || hit.collider.tag == "Enemy" && bThorAbility1Attack == true || hit.collider.tag == "Enemy" && bThorAbility2Attack == true)
                {
                    if (bThorBasicAttack == true)
                    {
                        m_nActionPoints = m_nActionPoints - m_nBasicAttackCost;
                        RemoveHighlightAttack();
                        hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nBasicAttack;
                    }
                    if (bThorAbility1Attack == true)
                    {
                        m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
                        RemoveHighlightAttack();
                        hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
                    }
                    if (bThorAbility2Attack == true)
                    {
                        m_nActionPoints = m_nActionPoints - m_nAbility2AttackCost;
                        RemoveHighlightAttack();
                        hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
                    }
                }
            }
        }
    }
    void HighlightMovement()
    {
        hero.dijkstrasSearch(m_currentNode, m_nActionPoints, movementHighlight, m_nMovementActionPointCostPerTile);
        if (bBridal)
            bridalThorMoveSetButtons.SetActive(false);
        else
            thorMoveSetButtons.SetActive(false);
    }

    void BridalBasicAttack()
    {
        bridalThorMoveSetButtons.SetActive(false);

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
                if (m_tempNode.self.tag == "Tile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = AttackHighlight;
                    m_tempNode.self.tag = "ThorAttackableTile";
                }
                if (m_tempNode.self.tag == "CurrentEnemyTile")
                    m_tempNode.self.GetComponent<Renderer>().material = EnemyHighlight;
            }

            for (int b = 0; b < e; b++)
            {
                m_tempNode = m_tempNode.neighbours[2].neighbours[1];
                if (m_tempNode.self.tag == "Tile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = AttackHighlight;
                    m_tempNode.self.tag = "ThorAttackableTile";
                }
                if (m_tempNode.self.tag == "CurrentEnemyTile")
                    m_tempNode.self.GetComponent<Renderer>().material = EnemyHighlight;

            }

            for (int c = 0; c < e; c++)
            {
                m_tempNode = m_tempNode.neighbours[3].neighbours[2];
                if (m_tempNode.self.tag == "Tile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = AttackHighlight;
                    m_tempNode.self.tag = "ThorAttackableTile";
                }
                if (m_tempNode.self.tag == "CurrentEnemyTile")
                    m_tempNode.self.GetComponent<Renderer>().material = EnemyHighlight;
            }

            for (int d = 0; d < e; d++)
            {
                m_tempNode = m_tempNode.neighbours[0].neighbours[3];
                if (m_tempNode.self.tag == "Tile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = AttackHighlight;
                    m_tempNode.self.tag = "ThorAttackableTile";
                }
                if (m_tempNode.self.tag == "CurrentEnemyTile")
                    m_tempNode.self.GetComponent<Renderer>().material = EnemyHighlight;
            }
            if (f > e)
                e++;
        }
    }
    void BridalAbility1()
    {
        bridalThorMoveSetButtons.SetActive(false);
        actionPointCostLabel.SetActive(true);
        bBridalAbility1Attack = true;
        actionPointsMoveCostLabel.text = m_nAbility1AttackCost.ToString();

        for (int i = 0; i < m_currentNode.neighbours.Length; i++)
        {
            if (m_currentNode.neighbours[i].self.tag == "Tile")
            {
                m_currentNode.neighbours[i].self.GetComponent<Renderer>().material = AttackHighlight;
                m_currentNode.neighbours[i].self.tag = "ThorAttackableTile";
            }
            if (m_currentNode.neighbours[i].self.tag == "CurrentEnemyTile")
                m_currentNode.neighbours[i].self.GetComponent<Renderer>().material = EnemyHighlight;
        }

        if (m_currentNode.neighbours[3].neighbours[0].self.tag == "Tile")
        {
            m_currentNode.neighbours[3].neighbours[0].self.GetComponent<Renderer>().material = AttackHighlight;
            m_currentNode.neighbours[3].neighbours[0].self.tag = "ThorAttackableTile";
        }
        if (m_currentNode.neighbours[3].neighbours[0].self.tag == "CurrentEnemyTile")
            m_currentNode.neighbours[3].neighbours[0].self.GetComponent<Renderer>().material = EnemyHighlight;

        if (m_currentNode.neighbours[3].neighbours[2].self.tag == "Tile")
        {
            m_currentNode.neighbours[3].neighbours[2].self.GetComponent<Renderer>().material = AttackHighlight;
            m_currentNode.neighbours[3].neighbours[2].self.tag = "ThorAttackableTile";
        }
        if (m_currentNode.neighbours[3].neighbours[2].self.tag == "CurrentEnemyTile")
            m_currentNode.neighbours[3].neighbours[2].self.GetComponent<Renderer>().material = EnemyHighlight;

        if (m_currentNode.neighbours[1].neighbours[0].self.tag == "Tile")
        {
            m_currentNode.neighbours[1].neighbours[0].self.GetComponent<Renderer>().material = AttackHighlight;
            m_currentNode.neighbours[1].neighbours[0].self.tag = "ThorAttackableTile";
        }
        if (m_currentNode.neighbours[1].neighbours[0].self.tag == "CurrentEnemyTile")
            m_currentNode.neighbours[1].neighbours[0].self.GetComponent<Renderer>().material = EnemyHighlight;

        if (m_currentNode.neighbours[1].neighbours[2].self.tag == "Tile")
        {
            m_currentNode.neighbours[1].neighbours[2].self.GetComponent<Renderer>().material = AttackHighlight;
            m_currentNode.neighbours[1].neighbours[2].self.tag = "ThorAttackableTile";
        }
        if (m_currentNode.neighbours[1].neighbours[2].self.tag == "CurrentEnemyTile")
            m_currentNode.neighbours[1].neighbours[2].self.GetComponent<Renderer>().material = EnemyHighlight;
    }
    void ThorBasicAttack()
    {
        thorMoveSetButtons.SetActive(false);

        actionPointCostLabel.SetActive(true);
        bThorBasicAttack = true;
        actionPointsMoveCostLabel.text = m_nBasicAttack.ToString();

        for (int i = 0; i < m_currentNode.neighbours.Length; i++)
        {
            if (m_currentNode.neighbours[i].self.tag == "Tile")
            {
                m_currentNode.neighbours[i].self.GetComponent<Renderer>().material = AttackHighlight;
                m_currentNode.neighbours[i].self.tag = "ThorAttackableTile";
            }
            if (m_currentNode.neighbours[i].self.tag == "CurrentEnemyTile")
                m_currentNode.neighbours[i].self.GetComponent<Renderer>().material = EnemyHighlight;
        }
    }
    void ThorAbility1()
    {
        thorMoveSetButtons.SetActive(false);

        hero.dijkstrasSearchAttack(m_currentNode, 4, movementHighlight, 1);

  



        //m_tempNode = m_tempNodeBase;        //Sets base 
        //int f = 4;      //The attackRange
        //int e = 1;
        //
        //for (int i = 0; i < e; i++)
        //{
        //    m_tempNode = m_tempNode.neighbours[3];
        //    for (int a = 0; a < e; a++)
        //    {
        //        m_tempNode = m_tempNode.neighbours[1].neighbours[0];
        //        if (m_tempNode.self.tag == "Tile")
        //        {
        //            m_tempNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        //            m_tempNode.self.tag = "ThorAttackableTile";
        //        }
        //        if (m_tempNode.self.tag == "CurrentEnemyTile")
        //            m_tempNode.self.GetComponent<Renderer>().material = AttackHighlight;
        //    }
        //
        //    for (int b = 0; b < e; b++)
        //    {
        //        m_tempNode = m_tempNode.neighbours[2].neighbours[1];
        //        if (m_tempNode.self.tag == "Tile")
        //        {
        //            m_tempNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        //            m_tempNode.self.tag = "ThorAttackableTile";
        //        }
        //        if (m_tempNode.self.tag == "CurrentEnemyTile")
        //            m_tempNode.self.GetComponent<Renderer>().material = AttackHighlight;
        //
        //    }
        //
        //    for (int c = 0; c < e; c++)
        //    {
        //
        //        m_tempNode = m_tempNode.neighbours[3].neighbours[2];
        //        if (m_tempNode.self.tag == "Tile")
        //        {
        //            m_tempNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        //            m_tempNode.self.tag = "ThorAttackableTile";
        //        }
        //        if (m_tempNode.self.tag == "CurrentEnemyTile")
        //            m_tempNode.self.GetComponent<Renderer>().material = AttackHighlight;
        //    }
        //
        //    for (int d = 0; d < e; d++)
        //    {
        //        m_tempNode = m_tempNode.neighbours[0].neighbours[3];
        //        if (m_tempNode.self.tag == "Tile")
        //        {
        //            m_tempNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        //            m_tempNode.self.tag = "ThorAttackableTile";
        //        }
        //        if (m_tempNode.self.tag == "CurrentEnemyTile")
        //            m_tempNode.self.GetComponent<Renderer>().material = AttackHighlight;
        //    }
        //    if (f > e)
        //        e++;
        //}
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
            m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
            for (int a = 0; a < e; a++)
            {
                m_tempNode = m_tempNode.neighbours[1].neighbours[0];
                if (m_tempNode.self.tag == "ThorAttackableTile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                    m_tempNode.self.tag = "Tile";
                }
                else if (m_tempNode.self.tag == "CurrentEnemyTile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                }
            }
            for (int b = 0; b < e; b++)
            {
                m_tempNode = m_tempNode.neighbours[2].neighbours[1];
                if (m_tempNode.self.tag == "ThorAttackableTile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                    m_tempNode.self.tag = "Tile";
                }
                else if (m_tempNode.self.tag == "CurrentEnemyTile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                }

            }
            for (int c = 0; c < e; c++)
            {

                m_tempNode = m_tempNode.neighbours[3].neighbours[2];
                if (m_tempNode.self.tag == "ThorAttackableTile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                    m_tempNode.self.tag = "Tile";
                }
                else if (m_tempNode.self.tag == "CurrentEnemyTile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                }
            }
            for (int d = 0; d < e; d++)
            {
                m_tempNode = m_tempNode.neighbours[0].neighbours[3];
                if (m_tempNode.self.tag == "ThorAttackableTile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                    m_tempNode.self.tag = "Tile";
                }
                else if (m_tempNode.self.tag == "CurrentEnemyTile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                }
            }
            if (f > e)
                e++;
        }
    }
}
        
    

