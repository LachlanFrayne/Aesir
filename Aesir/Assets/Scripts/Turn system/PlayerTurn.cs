using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PlayerTurn : StateMachineBehaviour
{
	public Thor m_thor;
	public Freya m_freya;
	public Loki m_loki;

    public GameObject endturnbutton;

   

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
        endturnbutton = animator.gameObject.GetComponent<EndPlayerTurn>().GetButton();
        m_thor = GameObject.Find("Thor").GetComponent<Thor>();
        m_freya = GameObject.Find("Freya").GetComponent<Freya>();
        m_loki = GameObject.Find("Loki").GetComponent<Loki>();

        m_thor.m_nActionPoints = m_thor.m_nActionPointMax;
		m_freya.m_nActionPoints = m_freya.m_nActionPointMax;
	    m_loki.m_nActionPoints = m_loki.m_nActionPointMax;

		m_freya.m_nHealth += m_freya.regen;


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
