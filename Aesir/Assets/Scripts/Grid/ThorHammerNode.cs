using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorHammerNode : MonoBehaviour {

	public GameObject m_hammer;
	public BridalThor m_bridalThor;
	public Node m_node;

	void Start ()
	{
		m_node = GetComponent<Node>();
		m_bridalThor = GameObject.Find("Thor").GetComponent<BridalThor>();
	}
	
	void Update ()
	{
		if (m_bridalThor.m_currentNode == m_node)
		{
			m_bridalThor.bThor = true;
			Destroy(m_hammer);
			Destroy(this);
		}
	}
}
