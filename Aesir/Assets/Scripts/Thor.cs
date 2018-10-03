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
    public GameObject Selection;
    bool turn = false;

    void Start ()
    { 
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
        {
            Move();
        }
        if(attack)
        {
            Attack();
        }

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Thor")
                {
                    Selection.SetActive(true);              
                    turn = true;
                }
                if (hit.collider.tag == "Loki")
                {
                    RemoveHighlight(m_currentNode.self.transform.position, 3);
                    turn = false;
                }
                if (hit.collider.tag == "Freya")
                {
                    RemoveHighlight(m_currentNode.self.transform.position, 3);
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
                                m_currentNode.self.tag = "Tile";
                                RemoveHighlight(m_currentNode.self.transform.position, 3);
                                m_currentNode = m_grid.boardArray[columnTile, rowTile];
                                m_currentNode.self.tag = "CurrentTile";
                                turn = false;
                            }
                        }
                    }
                }

                if (hit.collider.tag == "ThorAttackableTile" && turn)
                {
                    hit.collider.tag = "Thor";
                    hit.collider.GetComponent<MeshRenderer>().material.color = Color.red;
                    //Add Attack Code

                    for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
                    {
                        for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                        {
                            if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile].self)
                            {
                                m_currentNode.self.tag = "Tile";
                                RemoveHighlight(m_currentNode.self.transform.position, 3);
                                m_currentNode = m_grid.boardArray[columnTile, rowTile];
                                m_currentNode.self.tag = "CurrentTile";
                                turn = false;
                            }
                        }
                    }
                    
                }
            }

        }
    }

    void HighlightMovement()//Vector3 center, float radius, float line)
    {
        //int a = 8;
        //
        //m_tempNode = m_tempNodeBase;
        //for (int i = 0; i < a; i++)
        //{
        //    test();
        //    m_tempNode = m_tempNode.left;
        //}
        //for(int i = 0; i < a; i++)
        //{
        //    m_tempNode.right.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        //    m_tempNode = m_tempNode.right;
        //    m_tempNode.down.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        //    m_tempNode = m_tempNode.down;
        //}
        //
        //m_tempNode = m_tempNodeBase;
        //for (int i = 0; i < a; i++)
        //{
        //    test();
        //    m_tempNode = m_tempNode.right;
        //}
        //for (int i = 0; i < a; i++)
        //{
        //    m_tempNode.left.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        //    m_tempNode = m_tempNode.left;
        //    m_tempNode.up.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        //    m_tempNode = m_tempNode.up;
        //}
        //
        //m_tempNode = m_tempNodeBase;
        //for (int i = 0; i < a; i++)
        //{
        //    test();
        //    m_tempNode = m_tempNode.up;
        //}
        //for (int i = 0; i < a; i++)
        //{
        //    m_tempNode.down.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        //    m_tempNode = m_tempNode.down;
        //    m_tempNode.left.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        //    m_tempNode = m_tempNode.left;
        //}
        //
        //
        //m_tempNode = m_tempNodeBase;
        //for (int i = 0; i < a; i++)
        //{
        //    test();
        //    m_tempNode = m_tempNode.down;
        //}
        //for (int i = 0; i < a; i++)
        //{
        //    m_tempNode.up.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        //    m_tempNode = m_tempNode.up;
        //    m_tempNode.right.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        //    m_tempNode = m_tempNode.right;
        //}

        m_tempNode = m_tempNodeBase;
        int f = 6;         
        int e = 1;

        for(int i = 0; i < e; i++)
        {
            m_tempNode = m_tempNode.left;
            m_tempNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
            for(int a = 0; a < e; a++)
            {
                m_tempNode = m_tempNode.right.up;
                m_tempNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
            }
            for(int b = 0; b < e; b++)
            {
            m_tempNode = m_tempNode.down.right;
            m_tempNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;

            }
            for(int c = 0; c < e; c++)
            {

            m_tempNode = m_tempNode.left.down;
            m_tempNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
            }
            for(int d = 0; d < e; d++)
            {
                m_tempNode = m_tempNode.up.left;
                m_tempNode.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
            }
            if(f > e)
                e++;
        }

    }

    void test()
    {
        m_tempNode.left.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        m_tempNode.right.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        m_tempNode.up.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
        m_tempNode.down.self.GetComponent<Renderer>().sharedMaterial = movementHighlight;
    }

    void HighlightAttack(Vector3 center, float radius)
    {

        Collider[] hitNodes = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitNodes.Length)
        {
            if (hitNodes[i].gameObject.tag == "Tile")
            {
                hitNodes[i].GetComponent<Renderer>().sharedMaterial = movementHighlight;
                m_currentNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                hitNodes[i].gameObject.tag = "ThorAttackableTile";
                m_currentNode.self.gameObject.tag = "Tile";

            }
            i++;
        }
    }

    void RemoveHighlight(Vector3 center, float radius)
    {

        Collider[] hitNodes = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitNodes.Length)
        {
            if (hitNodes[i].gameObject.tag == "ThorWalkableTile" || hitNodes[i].gameObject.tag == "ThorAttackableTile")
            {
                hitNodes[i].GetComponent<Renderer>().sharedMaterial = removeHighlight;
                hitNodes[i].gameObject.tag = "Tile";
            }
            i++;
        }
    }
   

    void Move()
    {
        //switch (m_actionPoints)
        //{
        //    case 1:
        //        HighlightMovement(m_currentNode.self.transform.GetComponent<Renderer>().bounds.center, 1.3f, 0);
        //        break;
        //    case 2:
        //        HighlightMovement(m_currentNode.self.transform.GetComponent<Renderer>().bounds.center, 3.2f, 0);
        //        break;
        //    case 3:
        //        HighlightMovement(m_currentNode.self.transform.GetComponent<Renderer>().bounds.center, 4.2f, 6);
        //        break;
        //    case 4:
        //        HighlightMovement(m_currentNode.self.transform.GetComponent<Renderer>().bounds.center, 6f, 8);
        //        break;
        //    case 5:
        //        HighlightMovement(m_currentNode.self.transform.GetComponent<Renderer>().bounds.center, 7.28f, 10);
        //        break;
        //    case 6:
        //        break;
        //    case 7:
        //        break;
        //    case 8:
        //        break;
        //    case 9:
        //        break;
        //    case 10:
        //        break;
        //    case 11:
        //        break;
        //    case 12:
        //        break;
        //
        //}




        //if(m_actionPoints == 2)
        HighlightMovement();//m_currentNode.self.transform.GetComponent<Renderer>().bounds.center, 1.3f, 0);                                     //1.3 for 1 tile, 3.2 for 2 tiles, 4.3 for 3 tiles + 6,
        Selection.SetActive(false);                                                                                                                     //6 for 4 tiles + 8, 7.28 for 5 tiles + 10, YOU FUCKING CANT DO 6 TILES
        SetMoveFalse();
    }
    void Attack()
    {
        HighlightAttack(m_currentNode.self.transform.position, 3);
        Selection.SetActive(false);
        SetAttackFalse();
    }
}
