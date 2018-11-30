using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveDecision : BaseDecision
{
	public List<Node> m_path;

    public new void Start()
    {
        m_path = new List<Node>();

        base.Start();
    }

	public override void MakeDecision()
    {
        m_path.Clear();
        m_self.m_grid.ClearBoardData();

		Heap openList = new Heap(true);
        List<Node> closedList = new List<Node>();

        Node currentNode = m_self.m_currentNode;

        openList.Add(currentNode);

        while (openList.m_tHeap.Count > 0)      //loop through openlist to get path
        {
            currentNode = openList.Pop();       //update current node with openlist
            closedList.Add(currentNode);        //add current node to closed list
            
            if (m_self.m_targetedHero.m_currentNode == currentNode)        //if at end of path
            {
                break;
            }

            if (currentNode.contain != m_self.gameObject)
            {
                if (currentNode.contain != null)
                {
                    continue;
                }
            }

            foreach (Node n in currentNode.neighbours)
            {
				if (!n)     //if node doesnt exist
				{
					continue;
				}

				if (n.bBlocked || n.contain || n.bObstacle)        //if the node is blocked
				{
					if (n.contain != m_self.m_targetedHero.gameObject)
					{
						continue;
					}
				}

				if (!closedList.Contains(n))        //if not in closed list
                {
                    if (openList.m_tHeap.Contains(n))       //if in openlist
                    {
                        if (currentNode.m_gScore + m_self.m_nMovementActionPointCostPerTile < n.m_gScore)     //if better path
                        {
                            n.m_gScore = currentNode.m_gScore + m_self.m_nMovementActionPointCostPerTile;
							if (m_self.m_targetedHero != null)
							{

								n.m_hScore = Vector3.Distance(currentNode.transform.position, m_self.m_targetedHero.transform.position);
							}
                            n.m_fScore = n.m_hScore + n.m_gScore;
                            n.prev = currentNode;
                        }
                    }
					else		//if not in openlist
                    {		//update neighbors info
                        n.m_gScore = currentNode.m_gScore + m_self.m_nMovementActionPointCostPerTile;
						if (m_self.m_targetedHero != null)
						{
							n.m_hScore = Vector3.Distance(currentNode.transform.position, m_self.m_targetedHero.transform.position);

						}
                        n.m_fScore = n.m_hScore + n.m_gScore;
                        n.prev = currentNode;

                        openList.Add(n);		//add to openlist
                    }
                }
                
            }
        }


		Node currentPathNode = m_self.m_targetedHero.m_currentNode.prev;

		if(currentPathNode == null)
		{
			m_self.m_nActionPoints = 0;
			return;
		}

		while(currentPathNode.prev != null)
		{
			m_path.Add(currentPathNode);
			currentPathNode = currentPathNode.prev;
		}

		if (m_self.m_nActionPoints >= m_self.m_nMovementActionPointCostPerTile)
		{
			if (m_path.Count >= 1)
			{
				m_self.m_currentNode.contain = null;
				m_self.m_currentNode = m_path[m_path.Count - 1];
				transform.position = new Vector3(m_path[m_path.Count - 1].transform.position.x, transform.position.y, m_path[m_path.Count - 1].transform.position.z);
				m_self.m_currentNode.contain = this.gameObject;

				m_self.m_nActionPoints -= m_self.m_nMovementActionPointCostPerTile;
			}
		}
		else
		{
			m_self.m_nActionPoints -= m_self.m_nMovementActionPointCostPerTile;
		}

        m_path.Clear();
        m_self.m_grid.ClearBoardData();
    }

	public override IEnumerator StartDecision()
	{
		m_path.Clear();
		m_self.m_grid.ClearBoardData();

		Heap openList = new Heap(true);
		List<Node> closedList = new List<Node>();

		Node currentNode = m_self.m_currentNode;

		openList.Add(currentNode);

		while (openList.m_tHeap.Count > 0)      //loop through openlist to get path
		{
			currentNode = openList.Pop();       //update current node with openlist
			closedList.Add(currentNode);        //add current node to closed list

			if (m_self.m_targetedHero.m_currentNode == currentNode)        //if at end of path
			{
				break;
			}

			if (currentNode.contain != m_self.gameObject)
			{
				if (currentNode.contain != null)
				{
					continue;
				}
			}

			foreach (Node n in currentNode.neighbours)
			{
				if (!n)     //if node doesnt exist
				{
					continue;
				}

				if (n.bBlocked || n.contain || n.bObstacle)        //if the node is blocked
				{
					if (n.contain != m_self.m_targetedHero.gameObject)
					{
						continue;
					}
				}

				if (!closedList.Contains(n))        //if not in closed list
				{
					if (openList.m_tHeap.Contains(n))       //if in openlist
					{
						if (currentNode.m_gScore + m_self.m_nMovementActionPointCostPerTile < n.m_gScore)     //if better path
						{
							n.m_gScore = currentNode.m_gScore + m_self.m_nMovementActionPointCostPerTile;

							n.m_hScore = Vector3.Distance(currentNode.transform.position, m_self.m_targetedHero.transform.position);
							n.m_fScore = n.m_hScore + n.m_gScore;
							n.prev = currentNode;
						}
					}
					else        //if not in openlist
					{       //update neighbors info
						n.m_gScore = currentNode.m_gScore + m_self.m_nMovementActionPointCostPerTile;
						n.m_hScore = Vector3.Distance(currentNode.transform.position, m_self.m_targetedHero.transform.position);
						n.m_fScore = n.m_hScore + n.m_gScore;
						n.prev = currentNode;

						openList.Add(n);        //add to openlist
					}
				}

			}
		}


		Node currentPathNode = m_self.m_targetedHero.m_currentNode.prev;

		if (currentPathNode == null)        //this means that the enemy cant get to the hero
		{
			m_self.m_nActionPoints = 0;
			//return; JM:StartHere
		}
		else
		{


			while (currentPathNode.prev != null)
			{
				m_path.Add(currentPathNode);
				currentPathNode = currentPathNode.prev;
			}

			if (m_self.m_nActionPoints >= m_self.m_nMovementActionPointCostPerTile)
			{
				if (m_path.Count >= 1)
				{
					m_self.m_currentNode.contain = null;
					m_self.m_currentNode = m_path[m_path.Count - 1];
					//transform.position = new Vector3(m_path[m_path.Count - 1].transform.position.x, transform.position.y, m_path[m_path.Count - 1].transform.position.z);

					yield return StartCoroutine(Lerp(transform.position, m_path[m_path.Count - 1].transform.position, Time.time, Vector3.Distance(transform.position, m_path[m_path.Count - 1].transform.position), 4));

					m_self.m_currentNode.contain = this.gameObject;

					m_self.m_nActionPoints -= m_self.m_nMovementActionPointCostPerTile;
				}
			}
			else
			{
				m_self.m_nActionPoints -= m_self.m_nMovementActionPointCostPerTile;
			}

			m_path.Clear();
			m_self.m_grid.ClearBoardData();
		}
	}

	IEnumerator Lerp(Vector3 playerPosition, Vector3 tilePosition, float startTime, float journeyLength, float speed)
	{
		m_self.GetComponentInChildren<Animator>().SetBool("Walk", true);
		while (transform.position != tilePosition)
		{
			
			float distCovered = ((Time.time - startTime) * speed);

			float fracJourney = distCovered / journeyLength;
			transform.position = Vector3.Lerp(playerPosition, tilePosition, fracJourney);
			yield return null;
		}
		m_self.GetComponentInChildren<Animator>().SetBool("Walk", false);
	}
}

