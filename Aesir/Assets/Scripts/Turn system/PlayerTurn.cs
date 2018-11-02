using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerTurn : StateMachineBehaviour
{
	public BridalThor m_thor;
	public Freya m_freya;
	public Loki m_loki;

    public GameObject endturnbutton;

	private void Awake()
	{
		m_thor = GameObject.Find("Thor").GetComponent<BridalThor>();
		m_freya = GameObject.Find("Freya").GetComponent<Freya>();
		m_loki = GameObject.Find("Loki").GetComponent<Loki>();
		
	}

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		if(endturnbutton == null)		//should only be called at start of game
		{
			endturnbutton = animator.gameObject.GetComponent<EndPlayerTurn>().GetButton();
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

	
        endturnbutton.SetActive(true);
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
