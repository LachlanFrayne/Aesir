using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleObject : MonoBehaviour {

	public int m_nHealth;
	public Grida m_grid;
	public Node m_currentNode;

	void Start()
	{
		RaycastHit hit;
		if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 100))       //Creates a raycast downwards
		{
			for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)     //Goes through the grid 
			{
				for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
				{
					if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile])        //Goes through the grid until it finds the tiles the character is on         
					{
						m_currentNode = m_grid.nodeBoardArray[columnTile, rowTile];        //Sets currentNode to the tile the character is on
						transform.position = new Vector3(m_currentNode.transform.position.x, 0, m_currentNode.transform.position.z);        //Sets position to the center of the tile
						m_currentNode.contain = this.gameObject;
					}
				}
			}
		}
	}
	void Update ()
	{
		if (m_nHealth <= 0)
			Destroy(this.gameObject);
	}
}
