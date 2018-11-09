using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateTargetDecision : BaseDecision
{

	public List<Entity> m_targets;
	protected List<float> m_targetScore = new List<float>();

	new void Start()
	{
		base.Start();

		//if (GameObject.Find("Thor").GetComponent<Thor>())
		//{
			
		//}
		//else 
		//if (GameObject.Find("Thor").GetComponent<BridalThor>())
		//{
			
		//}

		//m_targets.Add(GameObject.Find("Thor").GetComponent<Thor>());
		//m_targetScore.Add(0.0f);

		m_targets.Add(GameObject.Find("Thor").GetComponent<BridalThor>());
		m_targetScore.Add(0.0f);

		m_targets.Add(GameObject.Find("Freya").GetComponent<Freya>());
		m_targetScore.Add(0.0f);

		m_targets.Add(GameObject.Find("Loki").GetComponent<Loki>());
		m_targetScore.Add(0.0f);

		MakeDecision();
	}

	public override void MakeDecision()		//JM:STARTHERE, need to have MoveDirection.cs use Enemy's m_targetedHero to move toward, also have error check for when a hero is dead :)
	{
		if(m_targets[0].enabled == false)
		{
			m_targets[0] = GameObject.Find("Thor").GetComponent<Thor>();
		}

		for (int i = 0; i < m_targetScore.Count; i++)
		{
			if(m_targets[i])
			{
				m_targetScore[i] = (m_targets[i].m_nHealthMax / m_targets[i].m_nHealth) / Vector3.Distance(m_self.gameObject.transform.position, m_targets[i].gameObject.transform.position);
			}
			else
			{
				m_targetScore[i] = 0.0f;
			}
		}

		float target = Mathf.Max(m_targetScore.ToArray());

		for (int i = 0; i < m_targetScore.Count; i++)
		{
			if(target == m_targetScore[i])
			{
				m_self.m_targetedHero = m_targets[i];
			}
		}
	}
}
