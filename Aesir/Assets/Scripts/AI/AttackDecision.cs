using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackDecision : BaseDecision
{

	new void Start()
	{
		base.Start();
	}

	public override void MakeDecision()
	{
		if(m_self.m_nActionPoints >= m_self.m_nBasicAttackCost)
		{
			m_self.m_targetedHero.m_nHealth -= m_self.m_nBasicAttackDamage;
			m_self.m_nActionPoints -= m_self.m_nBasicAttackCost;
		}
		else
		{
			m_self.m_nActionPoints -= m_self.m_nBasicAttackCost;
		}
	}

	public override IEnumerator StartDecision()
	{
		throw new System.NotImplementedException();
	}
}
