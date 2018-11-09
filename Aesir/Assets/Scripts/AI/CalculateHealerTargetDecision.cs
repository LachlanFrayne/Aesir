using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateHealerTargetDecision : CalculateTargetDecision
{
	
	private new void Start()
	{
		m_self = gameObject.GetComponent<Enemy>();
		GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
		
		foreach(GameObject g in temp)
		{
			m_targets.Add(g.GetComponent<Enemy>());
			m_targetScore.Add(0.0f);
		}
		
		m_targets.Remove(this.gameObject.GetComponent<Enemy>());
		m_targetScore.Remove(0.0f);

		MakeDecision();
	}
	public override void MakeDecision()     //JM:STARTHERE, need to have MoveDirection.cs use Enemy's m_targetedHero to move toward, also have error check for when a hero is dead :)
	{


		for (int i = 0; i < m_targetScore.Count; i++)
		{
			if (m_targets[i])
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
			if (target == m_targetScore[i])
			{
				m_self.m_targetedHero = m_targets[i];
			}
		}
	}
}
