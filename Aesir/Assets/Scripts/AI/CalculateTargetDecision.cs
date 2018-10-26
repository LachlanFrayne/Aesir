using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateTargetDecision : BaseDecision
{
	public Thor m_thor;
	public Freya m_freya;
	public Loki m_loki;

	new void Start()
	{
		base.Start();

		m_thor = GameObject.Find("Thor").GetComponent<Thor>();
		m_freya = GameObject.Find("Freya").GetComponent<Freya>();
		m_loki = GameObject.Find("Loki").GetComponent<Loki>();

		MakeDecision();
	}

	public override void MakeDecision()		//JM:STARTHERE, need to have MoveDirection.cs use Enemy's m_targetedHero to move toward, also have error check for when a hero is dead :)
	{
		float thorTargetScore = 0.0f;
		float freyaTargetScore = 0.0f;
		float lokiTargetScore = 0.0f;

		if (m_thor != null)		//nullcheck
		{
			thorTargetScore = (m_thor.m_nHealthMax / m_thor.m_nHealth) / Vector3.Distance(m_self.gameObject.transform.position, m_thor.gameObject.transform.position);
		}
		if(m_freya != null)		//null check
		{
			freyaTargetScore = (m_freya.m_nHealthMax / m_freya.m_nHealth) / Vector3.Distance(m_self.gameObject.transform.position, m_freya.gameObject.transform.position);
		}
		if(m_loki != null)		//null check
		{
			lokiTargetScore = (m_loki.m_nHealthMax / m_loki.m_nHealth) / Vector3.Distance(m_self.gameObject.transform.position, m_loki.gameObject.transform.position);
		}

		float target = Mathf.Max(thorTargetScore, freyaTargetScore, lokiTargetScore);

		if (target == thorTargetScore)
		{
			m_self.m_targetedHero = m_thor;
		}
		else
		if(target == freyaTargetScore)
		{
			m_self.m_targetedHero = m_freya;
		}
		else
		if(target == lokiTargetScore)
		{
			m_self.m_targetedHero = m_loki;
		}

	}
}
