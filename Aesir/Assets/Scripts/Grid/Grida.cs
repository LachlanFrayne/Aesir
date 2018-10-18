using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class Grida:MonoBehaviour
{

    public GameObject tilePrefab;
    public GameObject[,] boardArray = new GameObject[gridSizeX, gridSizeY];
    public static int gridSizeX = 30;
    public static int gridSizeY = 30;
    public Node[,] nodeBoardArray = new Node[gridSizeX, gridSizeY];

    public Material remove;

    void Awake()
    {
        GenerateGrid();
        
    }

    void GenerateGrid()
    {
        for (int columnTile = 0; columnTile < boardArray.GetLength(0); columnTile++)
        {
            for (int rowTile = 0; rowTile < boardArray.GetLength(1); rowTile++)
            {
                boardArray[columnTile, rowTile] = Instantiate(tilePrefab, new Vector3(rowTile * 1.05f, 0, columnTile * 1.05f), Quaternion.Euler(90, 0, 0), gameObject.transform);      //Creates the grid
                boardArray[columnTile, rowTile].AddComponent<Node>();
                nodeBoardArray[columnTile, rowTile] = boardArray[columnTile, rowTile].GetComponent<Node>();
            }
        }
        
        for (int columnTile = 0; columnTile < boardArray.GetLength(0); columnTile++)
        {
            for (int rowTile = 0; rowTile < boardArray.GetLength(1); rowTile++)
            {
                if(rowTile < gridSizeY - 1)
                {
                    boardArray[columnTile, rowTile].GetComponent<Node>().neighbours[0] = nodeBoardArray[columnTile, rowTile + 1];       //Sets the up node from the current node
                }
        
                if(columnTile < gridSizeX - 1)
                {
                    boardArray[columnTile, rowTile].GetComponent<Node>().neighbours[1] = nodeBoardArray[columnTile + 1, rowTile];        //Sets the right node from the current node
                }
                if (rowTile > 0)
                {
                    boardArray[columnTile, rowTile].GetComponent<Node>().neighbours[2] = nodeBoardArray[columnTile, rowTile - 1];     //Sets the down node from the current node
                }
        
                if (columnTile > 0)
                {
                    boardArray[columnTile, rowTile].GetComponent<Node>().neighbours[3] = nodeBoardArray[columnTile - 1, rowTile];     //Sets the left node from the current node
                }
            }
        }
    }

	public void ClearBoardData()
	{
		for (int i = 0; i < gridSizeX; i++)
		{
			for (int j = 0; j < gridSizeX; j++)
			{
                if (nodeBoardArray[i, j].contain != null)
                    continue;

                nodeBoardArray[i, j].m_gScore = 0;
                nodeBoardArray[i, j].m_hScore = 0;
                nodeBoardArray[i, j].m_fScore = 0;
                nodeBoardArray[i, j].prev = null;
                
                boardArray[i, j].GetComponent<Renderer>().material = remove;
			}
		}
	}
}


