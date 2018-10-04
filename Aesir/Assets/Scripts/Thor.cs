using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Thor : Entity
{
    Grida m_grid;
    public Node m_currentNode;
    public Node m_tempNode;
    public Node m_tempNodeBase;
    public Material movementHighlight;
    public Material removeHighlight;
    public Material enemyHighlight;
    public GameObject Selection;
    public Text ActionPoint;
    public Text ActionPointsMoveCost;
    bool turn = false;
    bool ability1 = true;
    PlayerMovement playerMovement;

    void Start ()
    {
        playerMovement = GetComponent<PlayerMovement>();
        m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grida>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 100))       //Creates a raycast downwards
        {
            for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)     //Goes through the grid 
            {
                for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)      
                {
                    if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile].self)         
                    {
                        m_currentNode = m_grid.boardArray[columnTile, rowTile];
                        m_currentNode.self.tag = "CurrentTile";
                        transform.position = new Vector3(m_currentNode.self.transform.position.x, .5f, m_currentNode.self.transform.position.z);
                        m_tempNodeBase = m_currentNode;
                    }
                }
            }
        }

    }

    void Update()
    {
        if (move)
            Move();
        if (attack)
            Attack();

        ActionPoint.text = m_actionPoints.ToString();


        Ray ray1 = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit1;
        if (Physics.Raycast(ray1, out hit1, 100))
        {
            if (hit1.collider.tag == "ThorWalkableTile" && turn)
            {
                for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)     //Goes through the grid 
                {
                    for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                    {
                        if (hit1.collider.gameObject == m_grid.boardArray[columnTile, rowTile].self)
                        {
                            ActionPointsMoveCost.text = m_grid.boardArray[columnTile, rowTile].gScore.ToString();
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
                    Selection.SetActive(true);
                    turn = true;
                }
                if (hit.collider.tag == "Loki")     //If you click on another character, unhighlight 
                {
                    RemoveHighlightAttack();
                    turn = false;
                }
                if (hit.collider.tag == "Freya")     //If you click on another character, unhighlight    
                {
                    RemoveHighlightAttack();
                    turn = false;
                }
               
                

                if (hit.collider.tag == "ThorWalkableTile" && turn)
                {
                    transform.position = new Vector3(hit.collider.GetComponent<MeshRenderer>().bounds.center.x, 0.5f, hit.collider.GetComponent<MeshRenderer>().bounds.center.z);
                    for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
                    {
                        for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                        {
                            if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile].self)     
                            {
                                m_currentNode = m_grid.boardArray[columnTile, rowTile];
                                m_currentNode.self.tag = "CurrentTile";
                                m_actionPoints = m_actionPoints - m_grid.boardArray[columnTile, rowTile].gScore;
                                turn = false;
                            }
                        }
                    }
                }
                if (hit.collider.tag == "ThorAttackableTile" && turn && ability1)
                {
                    transform.position = new Vector3(hit.collider.GetComponent<MeshRenderer>().bounds.center.x, 0.5f, hit.collider.GetComponent<MeshRenderer>().bounds.center.z);
                    for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
                    {
                        for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                        {
                            if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile].self)
                            {
                                m_currentNode.self.tag = "Tile";
                                m_currentNode = m_grid.boardArray[columnTile, rowTile];
                                m_currentNode.self.tag = "CurrentTile";

                                RemoveHighlightAttack();
                                if (m_currentNode.left.self.tag == "Tile")
                                    m_currentNode.left.self.GetComponent<Renderer>().material = enemyHighlight;
                                if (m_currentNode.right.self.tag == "Tile")
                                    m_currentNode.right.self.GetComponent<Renderer>().material = enemyHighlight;
                                if (m_currentNode.up.self.tag == "Tile")
                                    m_currentNode.up.self.GetComponent<Renderer>().material = enemyHighlight;
                                if (m_currentNode.down.self.tag == "Tile")
                                    m_currentNode.down.self.GetComponent<Renderer>().material = enemyHighlight;

                                if (m_currentNode.left.up.self.tag == "Tile")
                                    m_currentNode.left.up.self.GetComponent<Renderer>().material = enemyHighlight;
                                if (m_currentNode.left.down.self.tag == "Tile")
                                    m_currentNode.left.down.self.GetComponent<Renderer>().material = enemyHighlight;
                                if (m_currentNode.right.up.self.tag == "Tile")
                                    m_currentNode.right.up.self.GetComponent<Renderer>().material = enemyHighlight;
                                if (m_currentNode.right.down.self.tag == "Tile")
                                    m_currentNode.right.down.self.GetComponent<Renderer>().material = enemyHighlight;

                                turn = false;
                                ability1 = false;
                            }
                        }
                    }
                }

                if (hit.collider.tag == "Enemy" && turn)
                {
                    //hit.collider.tag = "Thor";      //Remove once code that damages is put in
                    RemoveHighlightAttack();
                    //hit.collider.GetComponent<MeshRenderer>().material.color = Color.red;   //Remove once code that damages is put in    
                    //Add Attack Code
                    hit.collider.GetComponent<Entity>().m_health = hit.collider.GetComponent<Entity>().m_health - m_basicAttack;


                }
            }

        }
    }

    void HighlightMovement()
    {
         playerMovement.dijkstrasSearch(m_currentNode, 8, movementHighlight);
        
    }


    void HighlightAttack(string attack)
    {
        if (attack == "BridalBasicAttack")
        {
            m_tempNode = m_tempNodeBase;        //Sets base 
            int f = m_attackRange;      //The attackRange
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
                        m_tempNode.self.GetComponent<Renderer>().material = enemyHighlight;
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
                        m_tempNode.self.GetComponent<Renderer>().material = enemyHighlight;

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
                        m_tempNode.self.GetComponent<Renderer>().material = enemyHighlight;
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
                        m_tempNode.self.GetComponent<Renderer>().material = enemyHighlight;
                }
                if (f > e)
                    e++;
            }
        }

        if(attack == "BridalAbility1")
        {
            if (m_currentNode.left.self.tag == "Tile")
                m_currentNode.left.self.GetComponent<Renderer>().material = enemyHighlight;
            if (m_currentNode.right.self.tag == "Tile")
                m_currentNode.right.self.GetComponent<Renderer>().material = enemyHighlight;
            if (m_currentNode.up.self.tag == "Tile")
                m_currentNode.up.self.GetComponent<Renderer>().material = enemyHighlight;
            if (m_currentNode.down.self.tag == "Tile")
                m_currentNode.down.self.GetComponent<Renderer>().material = enemyHighlight;

            if (m_currentNode.left.up.self.tag == "Tile")
                m_currentNode.left.up.self.GetComponent<Renderer>().material = enemyHighlight;
            if (m_currentNode.left.down.self.tag == "Tile")
                m_currentNode.left.down.self.GetComponent<Renderer>().material = enemyHighlight;
            if (m_currentNode.right.up.self.tag == "Tile")
                m_currentNode.right.up.self.GetComponent<Renderer>().material = enemyHighlight;
            if (m_currentNode.right.down.self.tag == "Tile")
                m_currentNode.right.down.self.GetComponent<Renderer>().material = enemyHighlight;
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
                        m_tempNode.self.GetComponent<Renderer>().material = enemyHighlight;
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
                        m_tempNode.self.GetComponent<Renderer>().material = enemyHighlight;

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
                        m_tempNode.self.GetComponent<Renderer>().material = enemyHighlight;
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
                        m_tempNode.self.GetComponent<Renderer>().material = enemyHighlight;
                }
                if (f > e)
                    e++;
            }
        }
        
    }

    void RemoveHighlightAttack()
    {
        m_tempNode = m_tempNodeBase;
        int f = m_attackRange;
        int e = 1;

        for (int i = 0; i < e; i++)
        {
            m_tempNode = m_tempNode.left;
            m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
            for (int a = 0; a < e; a++)
            {
                m_tempNode = m_tempNode.right.up;
                if (m_tempNode.self.tag == "ThorAttackableTile" || m_tempNode.self.tag == "CurrentEnemyTile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                    m_tempNode.self.tag = "Tile";
                }
            }
            for (int b = 0; b < e; b++)
            {
                m_tempNode = m_tempNode.down.right;
                if (m_tempNode.self.tag == "ThorAttackableTile" || m_tempNode.self.tag == "CurrentEnemyTile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                    m_tempNode.self.tag = "Tile";
                }

            }
            for (int c = 0; c < e; c++)
            {

                m_tempNode = m_tempNode.left.down;
                if (m_tempNode.self.tag == "ThorAttackableTile" || m_tempNode.self.tag == "CurrentEnemyTile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                    m_tempNode.self.tag = "Tile";
                }
            }
            for (int d = 0; d < e; d++)
            {
                m_tempNode = m_tempNode.up.left;
                if (m_tempNode.self.tag == "ThorAttackableTile" || m_tempNode.self.tag == "CurrentEnemyTile")
                {
                    m_tempNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                    m_tempNode.self.tag = "Tile";
                }
            }
            if (f > e)
                e++;
        }
    }

   
    
    void Move()
    {
        HighlightMovement();                                  
        Selection.SetActive(false);
        SetMoveFalse();

    }
    void Attack()
    {
        //HighlightAttack("BasicAttack");
        HighlightAttack("BridalAbility1");
        Selection.SetActive(false);
        SetAttackFalse();
    }
}
