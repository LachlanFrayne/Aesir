using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Thor : Hero
{
    Grida m_grid;

    public Node m_currentNode;
    public Node m_tempNode;
    public Node m_tempNodeBase;
  
    public GameObject actionPointCostLabel;
    public GameObject ThorMoveSetButtons;

    public Text actionPointLabel;
    public Text actionPointMaxLabel;
    public Text actionPointsMoveCostLabel;
    public Text healthLabel;
    public Text healthMaxLabel;

    public Image actionPointsBarImage;
    public Image backgroundThorImage;

    public Button moveButton;
    public Button basicAttackButton;
    public Button bridalAbility1Button;
    public Button ability2Button;

    bool basicAttack = false;
    bool ability1Attack = false;

    PlayerMovement playerMovement;
    PlayerMovementRemove playerMovementRemove;

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

    void Start ()
    {
        actionPointCostLabel = GameObject.Find("Action Points Cost Thor");        
        ThorMoveSetButtons = GameObject.Find("ThorMoveSet");       
        actionPointLabel = GameObject.Find("Action Points Thor").GetComponent<Text>();      
        actionPointMaxLabel = GameObject.Find("Action Points Max Thor").GetComponent<Text>();
        actionPointsMoveCostLabel = GameObject.Find("Action Points Move Cost Thor").GetComponent<Text>();        
        healthLabel = GameObject.Find("Health Thor").GetComponent<Text>();
        healthMaxLabel = GameObject.Find("Health Max Thor").GetComponent<Text>();
        actionPointsBarImage = GameObject.Find("Action Points Bar Thor").GetComponent<Image>();
        backgroundThorImage = GameObject.Find("BackgroundThor").GetComponent<Image>();
        moveButton = GameObject.Find("Move Thor").GetComponent<Button>();
        basicAttackButton = GameObject.Find("Basic Attack Thor").GetComponent<Button>();
        bridalAbility1Button = GameObject.Find("Ability 1 Thor").GetComponent<Button>();
        ability2Button = GameObject.Find("Ability 2 Thor").GetComponent<Button>();
        ThorMoveSetButtons.SetActive(false);
        actionPointCostLabel.SetActive(false);



        m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grida>();        //Reference to Grida
        playerMovement = GetComponent<PlayerMovement>();        //Reference to PlayerMovement 
        playerMovementRemove = GetComponent<PlayerMovementRemove>();        //Reference to PlayerMovementRemove

        healthLabel.text = m_nHealth.ToString();
        healthMaxLabel.text = m_nHealthMax.ToString();
        actionPointLabel.text = m_nActionPoints.ToString();
        actionPointMaxLabel.text = m_nActionPointMax.ToString();

        SetTile();
    }

    void Update()
    {
        if (m_nActionPoints > 0)
            moveButton.onClick.AddListener(HighlightMovement);
        else
            moveButton.onClick.RemoveAllListeners();

        if (m_nActionPoints >= m_nBasicAttackCost)
            basicAttackButton.onClick.AddListener(delegate { HighlightAttack("BridalBasicAttack"); });
        else
            basicAttackButton.onClick.RemoveAllListeners();

        if (m_nActionPoints >= m_nAbility1AttackCost)
            bridalAbility1Button.onClick.AddListener(delegate { HighlightAttack("BridalAbility1"); });
        else
            bridalAbility1Button.onClick.RemoveAllListeners();

        actionPointsBarImage.fillAmount = (1f / m_nActionPointMax) * m_nActionPoints;    

        actionPointLabel.text = m_nActionPoints.ToString();      //Sets the ActionPoint text to the amount of actionPoints

        Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit1;

        if (Physics.Raycast(ray1, out hit1, 100))
        {
            if (hit1.collider.tag == "ThorWalkableTile")
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
                                    tile.self.tag = "ThorWalkableTile";
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
        }



        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Thor")     //If click on character, show selection tab
                {
                    basicAttack = false;
                    ability1Attack = false;
                    playerMovementRemove.dijkstrasSearch(m_currentNode,m_nActionPoints, removeHighlight);
                    RemoveHighlightAttack();
                    backgroundThorImage.GetComponent<Image>().color = new Color32(255, 0, 0, 150);
                    ThorMoveSetButtons.SetActive(true);
                }
                if (hit.collider.tag == "Loki")     //If you click on another character, unhighlight 
                {
                    playerMovementRemove.dijkstrasSearch(m_currentNode, m_nActionPoints, removeHighlight);
                    actionPointCostLabel.SetActive(false);
                }
                if (hit.collider.tag == "Freya")     //If you click on another character, unhighlight    
                {
                    playerMovementRemove.dijkstrasSearch(m_currentNode, m_nActionPoints, removeHighlight);
                    actionPointCostLabel.SetActive(false);
                }



                if (hit.collider.tag == "ThorWalkableTile")        //Used for when you are moving
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

                                playerMovementRemove.dijkstrasSearch(tempNode, tempActionPoints, removeHighlight);        //Removes the highlight
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
                            }
                        }
                    }
                }

                if (hit.collider.tag == "Enemy" && basicAttack == true || hit.collider.tag == "Enemy" && ability1Attack == true)
                {
                    actionPointCostLabel.SetActive(false);
                    if (basicAttack == true)
                    {
                        m_nActionPoints = m_nActionPoints - m_nBasicAttackCost;
                        RemoveHighlightAttack();
                        hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nBasicAttack;
                    }
                    if (ability1Attack == true)
                    {
                        m_nActionPoints = m_nActionPoints - m_nAbility1AttackCost;
                        RemoveHighlightAttack();
                        hit.collider.GetComponent<Enemy>().m_nHealth = hit.collider.GetComponent<Enemy>().m_nHealth - m_nAbility1Attack;
                    }
                }
            }
        }
    }

    void HighlightMovement()
    {
        playerMovement.dijkstrasSearch(m_currentNode, m_nActionPoints, movementHighlight,m_nMovementActionPointCostPerTile);
        ThorMoveSetButtons.SetActive(false);
    }

    /////////////////////////////////////Attacks/////////////////////////////////////////////////////////
    void HighlightAttack(string attack)
    {
        ThorMoveSetButtons.SetActive(false);
        if (attack == "BridalBasicAttack")
        {
            actionPointCostLabel.SetActive(true);
            actionPointsMoveCostLabel.text = m_nBasicAttackCost.ToString();
            basicAttack = true;

            m_tempNode = m_tempNodeBase;        //Sets base 
            int f = m_nBasicAttackRange;      //The attackRange
            int e = 1;

            for (int i = 0; i < e; i++)
            {
                m_tempNode = m_tempNode.left;
                for (int a = 0; a < e; a++)
                {
                    m_tempNode = m_tempNode.right.up;
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
                    m_tempNode = m_tempNode.down.right;
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
                    m_tempNode = m_tempNode.left.down;
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
                    m_tempNode = m_tempNode.up.left;
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

        if(attack == "BridalAbility1")          //ADD STUN ONCE ENEMY IS WORKING
        {
            actionPointCostLabel.SetActive(true);
            ability1Attack = true;
            actionPointsMoveCostLabel.text = m_nAbility1AttackCost.ToString();

            m_currentNode.left.self.GetComponent<Renderer>().material = AttackHighlight;
            if (m_currentNode.left.self.tag == "Tile")
            {
                m_currentNode.left.self.GetComponent<Renderer>().material = AttackHighlight;
                m_currentNode.left.self.tag = "ThorAttackableTile";
            }
            if (m_currentNode.left.self.tag == "CurrentEnemyTile")
                m_currentNode.left.self.GetComponent<Renderer>().material = EnemyHighlight;

            if (m_currentNode.right.self.tag == "Tile")
            {
                m_currentNode.right.self.GetComponent<Renderer>().material = AttackHighlight;
                m_currentNode.right.self.tag = "ThorAttackableTile";
            }
            if (m_currentNode.right.self.tag == "CurrentEnemyTile")
                m_currentNode.right.self.GetComponent<Renderer>().material = EnemyHighlight;

            if (m_currentNode.up.self.tag == "Tile")
            {
                m_currentNode.up.self.GetComponent<Renderer>().material = AttackHighlight;
                m_currentNode.up.self.tag = "ThorAttackableTile";
            }
            if (m_currentNode.up.self.tag == "CurrentEnemyTile")
                m_currentNode.up.self.GetComponent<Renderer>().material = EnemyHighlight;

            if (m_currentNode.down.self.tag == "Tile")
            {
                m_currentNode.down.self.GetComponent<Renderer>().material = AttackHighlight;
                m_currentNode.down.self.tag = "ThorAttackableTile";
            }
            if (m_currentNode.down.self.tag == "CurrentEnemyTile")
                m_currentNode.down.self.GetComponent<Renderer>().material = EnemyHighlight;

            if (m_currentNode.left.up.self.tag == "Tile")
            {
                m_currentNode.left.up.self.GetComponent<Renderer>().material = AttackHighlight;
                m_currentNode.left.up.self.tag = "ThorAttackableTile";
            }
            if (m_currentNode.left.up.self.tag == "CurrentEnemyTile")
                m_currentNode.left.up.self.GetComponent<Renderer>().material = EnemyHighlight;

            if (m_currentNode.left.down.self.tag == "Tile")
            {
                m_currentNode.left.down.self.GetComponent<Renderer>().material = AttackHighlight;
                m_currentNode.left.down.self.tag = "ThorAttackableTile";
            }
            if (m_currentNode.left.down.self.tag == "CurrentEnemyTile")
                m_currentNode.left.down.self.GetComponent<Renderer>().material = EnemyHighlight;

            if (m_currentNode.right.up.self.tag == "Tile")
            {
                m_currentNode.right.up.self.GetComponent<Renderer>().material = AttackHighlight;
                m_currentNode.right.up.self.tag = "ThorAttackableTile";
            }
            if (m_currentNode.right.up.self.tag == "CurrentEnemyTile")
                m_currentNode.right.up.self.GetComponent<Renderer>().material = EnemyHighlight;

            if (m_currentNode.right.down.self.tag == "Tile")
            {
                m_currentNode.right.down.self.GetComponent<Renderer>().material = AttackHighlight;
                m_currentNode.right.down.self.tag = "ThorAttackableTile";
            }
            if (m_currentNode.right.down.self.tag == "CurrentEnemyTile")
                m_currentNode.right.down.self.GetComponent<Renderer>().material = EnemyHighlight;
        }


        if(attack == "Ability1")
        {
            m_tempNode = m_tempNodeBase;        //Sets base 
            int f = 4;      //The attackRange
            int e = 1;

            for (int i = 0; i < e; i++)
            {
                m_tempNode = m_tempNode.left;
                for (int a = 0; a < e; a++)
                {
                    m_tempNode = m_tempNode.right.up;
                    if (m_tempNode.self.tag == "Tile")
                    {
                        m_tempNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
                        m_tempNode.self.tag = "ThorAttackableTile";
                    }
                    if (m_tempNode.self.tag == "CurrentEnemyTile")
                        m_tempNode.self.GetComponent<Renderer>().material = AttackHighlight;
                }

                for (int b = 0; b < e; b++)
                {
                    m_tempNode = m_tempNode.down.right;
                    if (m_tempNode.self.tag == "Tile")
                    {
                        m_tempNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
                        m_tempNode.self.tag = "ThorAttackableTile";
                    }
                    if (m_tempNode.self.tag == "CurrentEnemyTile")
                        m_tempNode.self.GetComponent<Renderer>().material = AttackHighlight;

                }

                for (int c = 0; c < e; c++)
                {

                    m_tempNode = m_tempNode.left.down;
                    if (m_tempNode.self.tag == "Tile")
                    {
                        m_tempNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
                        m_tempNode.self.tag = "ThorAttackableTile";
                    }
                    if (m_tempNode.self.tag == "CurrentEnemyTile")
                        m_tempNode.self.GetComponent<Renderer>().material = AttackHighlight;
                }

                for (int d = 0; d < e; d++)
                {
                    m_tempNode = m_tempNode.up.left;
                    if (m_tempNode.self.tag == "Tile")
                    {
                        m_tempNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
                        m_tempNode.self.tag = "ThorAttackableTile";
                    }
                    if (m_tempNode.self.tag == "CurrentEnemyTile")
                        m_tempNode.self.GetComponent<Renderer>().material = AttackHighlight;
                }
                if (f > e)
                    e++;
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
            m_tempNode = m_tempNode.left;
            m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
            for (int a = 0; a < e; a++)
            {
                m_tempNode = m_tempNode.right.up;
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
                m_tempNode = m_tempNode.down.right;
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

                m_tempNode = m_tempNode.left.down;
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
                m_tempNode = m_tempNode.up.left;
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
}
