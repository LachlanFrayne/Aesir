using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurn : StateMachineBehaviour
{
	public Thor m_thor;
	public Freya m_freya;
	public Loki m_loki;

	//JM:STARTHERE, add a listener on an end turn button, and remove in the onexit then complete other states, also fix the thor variable not being set in the moveDecision script

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		m_thor.m_nActionPoints = m_thor.m_nActionPointMax;
		m_thor.m_nActionPoints = m_thor.m_nActionPointMax;
		m_thor.m_nActionPoints = m_thor.m_nActionPointMax;

		m_freya.m_nActionPoints = m_freya.m_nActionPointMax;
		m_freya.m_nActionPoints = m_freya.m_nActionPointMax;
		m_freya.m_nActionPoints = m_freya.m_nActionPointMax;

		m_loki.m_nActionPoints = m_loki.m_nActionPointMax;
		m_loki.m_nActionPoints = m_loki.m_nActionPointMax;
		m_loki.m_nActionPoints = m_loki.m_nActionPointMax;

		m_freya.m_nHealth += m_freya.regen;
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		
	}

	private void OnDisable()
	{
		
	}
}
