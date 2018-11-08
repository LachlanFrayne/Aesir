using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridAutoBlock : MonoBehaviour
{

	void Start ()
	{

		Grida m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grida>();
		RaycastHit hit;

		if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 100))
		{
			for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
			{
				for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
				{
					if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile])
					{
						Node currentNode = m_grid.nodeBoardArray[columnTile, rowTile];
						currentNode.BlockNode();
						currentNode.contain = this.gameObject;
					}
				}
			}
		}
	}
}
