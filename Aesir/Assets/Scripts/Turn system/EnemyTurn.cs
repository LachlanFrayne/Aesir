using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : StateMachineBehaviour
{
    public GameObject m_temp;
    public List<Enemy> m_enemy;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
		m_enemy = animator.gameObject.GetComponent<EnemyLinker>().GetEnemies();

		//animator.gameObject.GetComponent<EnemyLinker>().EnemyTurnCoroutine();
		EnemyLinker link = animator.GetComponent<EnemyLinker>();

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

		link.StartCoroutine(link.EnemyDecisionManager(animator));

	}

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
		foreach(Enemy e in m_enemy)
		{
			if(e.m_nActionPoints > 0)
			{
				return;
			}
		}
		animator.SetBool("PlayerTurn", true);
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

	
}
