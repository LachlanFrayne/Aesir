﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    public Grida m_grid;

    bool turn = false;

    public int m_meleeDamage;
    public int m_rangeDamage;

	public MoveDecision m_move;

    void Start()
    {
		
		m_move = gameObject.AddComponent<MoveDecision>();

		m_grid = GameObject.FindGameObjectWithTag("Grid").GetComponent<Grida>();
        RaycastHit hit;
        if (Physics.Raycast(transform.position, new Vector3(0, -1, 0), out hit, 100))
        {
            for (int columnTile = 0; columnTile < m_grid.boardArray.GetLength(0); columnTile++)
            {
                for (int rowTile = 0; rowTile < m_grid.boardArray.GetLength(1); rowTile++)
                {
                    if (hit.collider.gameObject == m_grid.boardArray[columnTile, rowTile])
                    {
                        m_currentNode = m_grid.nodeBoardArray[columnTile, rowTile];
                        m_currentNode.contain = this.gameObject;
                        transform.position = new Vector3(m_currentNode.transform.position.x, .5f, m_currentNode.transform.position.z);
                    }
                }
            }
        }
    }

    void Update()
    {
        if(m_nHealth <= 0)
        {
            Destroy(this.gameObject);
        }

		if (Input.GetKeyDown(KeyCode.P))
		{
			m_move.MakeDecision();

			
		}
    }
}
