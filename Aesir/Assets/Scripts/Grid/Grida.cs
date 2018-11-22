using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grida:MonoBehaviour
{
    public GameObject[,] boardArray;
    public static int gridSizeX;
    public static int gridSizeY;
    public Node[,] nodeBoardArray;

    public List<List<GameObject>> grid = new List<List<GameObject>>();
    public List<GameObject> rows;

    public Material remove;

    void Awake()
    {
        GenerateGrid();     
    }

    void GenerateGrid()
    {
        int i = 0;
        foreach (Transform row in transform)
        { 
            rows.Add(row.gameObject);
            grid.Add(new List<GameObject>());
            foreach (Transform tiles in row.transform)
            {
                grid[i].Add(tiles.transform.gameObject);
            }
            i++;
        }

        gridSizeX = grid.Count;
        gridSizeY = grid[0].Count;
        boardArray = new GameObject[gridSizeX, gridSizeY];
        nodeBoardArray = new Node[gridSizeX, gridSizeY];

        for (int columnTile = 0; columnTile < grid.Count; columnTile++)
        {
            for (int rowTile = 0; rowTile < grid[columnTile].Count; rowTile++)
            {
                boardArray[columnTile, rowTile] = grid[columnTile][rowTile];
                nodeBoardArray[columnTile, rowTile] = boardArray[columnTile,rowTile].GetComponent<Node>();
            }
        }

        for (int columnTile = 0; columnTile < grid.Count; columnTile++)
        {
            for (int rowTile = 0; rowTile < grid[columnTile].Count; rowTile++)
            {
                if (rowTile < grid[0].Count - 1)
                {
                    boardArray[columnTile, rowTile].GetComponent<Node>().neighbours[0] = nodeBoardArray[columnTile, rowTile + 1];
                }

                if (columnTile < grid.Count - 1)
                {
                    boardArray[columnTile, rowTile].GetComponent<Node>().neighbours[3] = nodeBoardArray[columnTile + 1, rowTile];
                }
                if (rowTile > 0)
                {
                    boardArray[columnTile, rowTile].GetComponent<Node>().neighbours[2] = nodeBoardArray[columnTile, rowTile - 1];
                }
                if (columnTile > 0)
                {
                    boardArray[columnTile, rowTile].GetComponent<Node>().neighbours[1] = nodeBoardArray[columnTile - 1, rowTile];
                }
            }
        }
    }

	public void ClearBoardData()
	{
		for (int i = 0; i < boardArray.GetLength(0); i++)
		{
			for (int j = 0; j < boardArray.GetLength(1); j++)
			{
                nodeBoardArray[i, j].m_gScore = 0;
                nodeBoardArray[i, j].m_hScore = 0;
                nodeBoardArray[i, j].m_fScore = 0;
                nodeBoardArray[i, j].prev = null;
                boardArray[i, j].GetComponent<Renderer>().material = remove;
			}
		}
	}
}


