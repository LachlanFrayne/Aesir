using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour       //Used to find out what the prev node
{
    public Node m_currentNode;
    public Node m_tempNodeBase;

    public Material pathUpDown;
    public Material pathLeftRight;
    public Material pathUpLeft;
    public Material pathUpRight;
    public Material pathDownLeft;
    public Material pathDownRight;


    void Update()
    {
        Grida m_grid = GetComponent<Grida>();

        if (Input.GetMouseButtonUp(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                if (hit.collider.tag == "Tile")
                {
                    for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)     //Goes through the grid 
                    {
                        for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                        {
                            if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile].self)
                            {
                                //m_grid.boardArray[columnTile, rowTile].prev.self.GetComponent<MeshRenderer>().material.color = Color.red;
                                m_grid.boardArray[columnTile, rowTile].self.GetComponent<MeshRenderer>().material = pathDownLeft;

                                //upleft = downleft
                                //upright = upleft
                                //downleft = downright
                                //downright = upright
                          
                            }
                        }
                    }
                }
            }
        }
    }
}
