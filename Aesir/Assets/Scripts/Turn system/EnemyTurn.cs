using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : StateMachineBehaviour
{
    public GameObject m_temp;
    List<Enemy> m_enemy;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
		m_enemy = animator.gameObject.GetComponent<EnemyLinker>().GetEnemies();


		foreach (Enemy e in m_enemy)
		{
			if (!e.m_bStunned)
			{
				e.m_nActionPoints = e.m_nActionPointMax;
				e.m_calcTargetDecision.MakeDecision();
			}
			else
			{
				e.m_nActionPoints = 0;
			}
		}

	}

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
		foreach (Enemy e in m_enemy)
		{
			e.m_bStunned = false;
		}
	}

    private void OnDisable()
    {
        
    }

	IEnumerator EnemyDecisionManager(Animator animator)
	{
		foreach (Enemy e in m_enemy)
		{
			while (e.m_nActionPoints > 0)
			{
				//e.m_inRangeDecision.MakeDecision();
				yield return e.m_inRangeDecision.StartCoroutine(e.m_inRangeDecision.StartDecision()); 
			}
		}

		foreach (Enemy e in m_enemy)
		{
			if (e.m_nActionPoints > 0)
			{
				//return;
			}
		}
		animator.SetBool("PlayerTurn", true);
	}
}
