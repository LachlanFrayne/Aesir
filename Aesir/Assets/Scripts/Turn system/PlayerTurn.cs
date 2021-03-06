﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerTurn : StateMachineBehaviour
{
	public BridalThor m_thor;
	public Freya m_freya;
	public Loki m_loki;

    public GameObject endturnbutton;

	public bool m_enemyTurnFirst;

	private void Awake()
	{
		m_thor = GameObject.Find("Thor").GetComponent<BridalThor>();
		m_freya = GameObject.Find("Freya").GetComponent<Freya>();
		m_loki = GameObject.Find("Loki").GetComponent<Loki>();
		endturnbutton = GameObject.Find("EndTurnButton");
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		if(m_enemyTurnFirst)
		{
			m_enemyTurnFirst = false;
			animator.SetBool("EnemyTurn", true);
		}

		endturnbutton.GetComponent<Button>().onClick.AddListener(animator.GetComponent<EndPlayerTurn>().ExitPlayerTurn);

		if (m_thor != null)
		{
			if (m_thor.enabled == false)
			{
				m_thor = GameObject.Find("Thor").GetComponent<Thor>();
			}
		}

		if (m_thor != null)
		{
			m_thor.m_nActionPoints = m_thor.m_nActionPointMax;
		}
		
		if (m_freya != null)
		{
			m_freya.m_nActionPoints = m_freya.m_nActionPointMax;

			m_freya.m_nHealth += m_freya.regen;		//add health to freya
			if (m_freya.m_nHealth > m_freya.m_nHealthMax)		//if freya health above max, bring it back down to max
			{
				m_freya.m_nHealth = m_freya.m_nHealthMax;
			}
		}
		if (m_loki != null)
		{
			m_loki.m_nActionPoints = m_loki.m_nActionPointMax;
		}

	
        endturnbutton.gameObject.SetActive(true);
    }

	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
        endturnbutton.SetActive(false);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		
	}

	private void OnDisable()
	{
		
	}
}
