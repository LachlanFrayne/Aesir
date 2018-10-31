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
}
