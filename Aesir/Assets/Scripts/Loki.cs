using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loki : Entity
{
    Grida m_grid;
    public GameObject m_gridObject;
    public Node m_currentNode;
    public Material movementHighlight;
    public Material removeHighlight;
    bool turn = false;

    void Start()
    {
        m_grid = m_gridObject.GetComponent<Grida>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 100))
        {
            for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
            {
                for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                {
                    if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile].self)
                    {
                        m_currentNode = m_grid.boardArray[columnTile, rowTile];
                        m_currentNode.self.tag = "CurrentTile";
                        transform.position = new Vector3(m_currentNode.self.transform.position.x, .5f, m_currentNode.self.transform.position.z);
                    }
                }
            }
        }
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Loki")
                {
                    HighlightMovement(m_currentNode.self.transform.position, 3);
                    turn = true;
                }
                if(hit.collider.tag == "Thor")
                {
                    RemoveHighlightMovement(m_currentNode.self.transform.position, 3);
                    turn = false;
                }
                if (hit.collider.tag == "Freya")
                {
                    RemoveHighlightMovement(m_currentNode.self.transform.position, 3);
                    turn = false;
                }
                if (hit.collider.tag == "LokiWalkableTile" && turn)
                {
                    transform.position = new Vector3(hit.collider.GetComponent<MeshRenderer>().bounds.center.x, 0.5f, hit.collider.GetComponent<MeshRenderer>().bounds.center.z);
                    for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
                    {
                        for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                        {
                            if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile].self)
                            {
                                m_currentNode.self.tag = "Tile";
                                RemoveHighlightMovement(m_currentNode.self.transform.position, 3);
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

    void HighlightMovement(Vector3 center, float radius)
    {

        Collider[] hitNodes = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitNodes.Length)
        {
            if (hitNodes[i].gameObject.tag == "Tile")
            {
                hitNodes[i].GetComponent<Renderer>().sharedMaterial = movementHighlight;
                m_currentNode.self.GetComponent<Renderer>().sharedMaterial = removeHighlight;
                hitNodes[i].gameObject.tag = "LokiWalkableTile";
                m_currentNode.self.gameObject.tag = "Tile";
            }
            i++;
        }
    }

    void RemoveHighlightMovement(Vector3 center, float radius)
    {

        Collider[] hitNodes = Physics.OverlapSphere(center, radius);
        int i = 0;
        while (i < hitNodes.Length)
        {
            if (hitNodes[i].gameObject.tag == "LokiWalkableTile")
            {
                hitNodes[i].GetComponent<Renderer>().sharedMaterial = removeHighlight;
                hitNodes[i].gameObject.tag = "Tile";
            }
            i++;
        }
    }
}
