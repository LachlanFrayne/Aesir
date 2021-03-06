﻿using System.Collections;
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
		if (m_self.m_nActionPoints >= m_self.m_nBasicAttackCost)
		{
			if (m_self.m_targetedHero.GetType() == typeof(Hero))
			{
				m_self.m_targetedHero.GetComponent<Hero>().GetHit(m_self.m_nBasicAttackDamage);
			}
			else
			{
				m_self.m_targetedHero.m_nHealth -= m_self.m_nBasicAttackDamage;
			}

			m_self.GetComponentInChildren<Animator>().SetBool("Hit", true);
			yield return new WaitForSeconds(GetComponentInChildren<Animator>().GetCurrentAnimatorStateInfo(0).length * 2);
			m_self.m_nActionPoints -= m_self.m_nBasicAttackCost;
		}
		else
		{
			m_self.m_nActionPoints -= m_self.m_nBasicAttackCost;
		}
	
	}
}
