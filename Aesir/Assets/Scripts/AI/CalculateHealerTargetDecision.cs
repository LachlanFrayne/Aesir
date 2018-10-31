using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : CalculateTargetDecision
{
	

	private new void Start()
	{

		GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");
		
		foreach(GameObject g in temp)
		{		//JM:STARTHERE need to check for self as to not be able to heal self, and then apply script to a healer enemy
			m_targets.Add(g.GetComponent<Enemy>());
			m_targetScore.Add(0);
		}

	}
}
