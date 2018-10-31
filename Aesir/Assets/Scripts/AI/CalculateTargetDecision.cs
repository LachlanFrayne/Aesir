using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateTargetDecision : BaseDecision
{

	public List<Entity> m_targets;
	protected List<float> m_targetScore;

	new void Start()
	{
		base.Start();

		m_targets.Add(GameObject.Find("Thor").GetComponent<Thor>());
		m_targetScore.Add(0);

		m_targets.Add(GameObject.Find("Freya").GetComponent<Freya>());
		m_targetScore.Add(0);

		m_targets.Add(GameObject.Find("Loki").GetComponent<Loki>());
		m_targetScore.Add(0);

		MakeDecision();
	}

	public override void MakeDecision()		//JM:STARTHERE, need to have MoveDirection.cs use Enemy's m_targetedHero to move toward, also have error check for when a hero is dead :)
	{

		for (int i = 0; i < m_targetScore.Count; i++)
		{
			m_targetScore[i] = (m_targets[i].m_nHealthMax / m_targets[i].m_nHealth) / Vector3.Distance(m_self.gameObject.transform.position, m_targets[i].gameObject.transform.position);
		}

		float target = Mathf.Max(m_targetScore.ToArray());

		for (int i = 0; i < m_targetScore.Count; i++)
		{
			if(target == m_targetScore[i])
			{
				m_self.m_targetedHero = (Hero)m_targets[i];
			}
		}
	}
}
