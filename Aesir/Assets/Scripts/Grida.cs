using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class Grida : MonoBehaviour {

    public GameObject tilePrefab;
    public Node[,] boardArray = new Node[gridSizeX, gridSizeY];
    public static int gridSizeX = 30;
    public static int gridSizeY = 30;

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
                boardArray[columnTile, rowTile] = new Node();       //Sets everyting in boardArray to a node
                boardArray[columnTile, rowTile].self = Instantiate(tilePrefab, new Vector3(rowTile * 2.05f, 0, columnTile * 2.05f), Quaternion.Euler(90, 0, 0), gameObject.transform);      //Creates the grid
            }
        }
        
        for (int columnTile = 0; columnTile < boardArray.GetLength(0); columnTile++)
        {
            for (int rowTile = 0; rowTile < boardArray.GetLength(1); rowTile++)
            {
                if(rowTile < gridSizeY - 1)
                {
                    boardArray[columnTile, rowTile].neighbours[0] = boardArray[columnTile, rowTile + 1];       //Sets the up node from the current node
                }
        
                if(columnTile < gridSizeX - 1)
                {
                    boardArray[columnTile, rowTile].neighbours[1] = boardArray[columnTile + 1, rowTile];        //Sets the right node from the current node
                }
                if (rowTile > 0)
                {
                    boardArray[columnTile, rowTile].neighbours[2] = boardArray[columnTile, rowTile - 1];     //Sets the down node from the current node
                }
        
                if (columnTile > 0)
                {
                    boardArray[columnTile, rowTile].neighbours[3] = boardArray[columnTile - 1, rowTile];     //Sets the left node from the current node
                }
            }
        }
    }
}

public class Node
{
    public Node[] neighbours = new Node[4];

    public Node prev;
    public int gScore;
    public GameObject self;
}
