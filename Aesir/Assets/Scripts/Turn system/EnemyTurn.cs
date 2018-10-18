using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : StateMachineBehaviour
{
    public GameObject m_temp;
    Enemy m_enemy;
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_enemy.m_nActionPoints = m_enemy.m_nActionPointMax;
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
       
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        m_enemy.m_move.MakeDecision();
        if(m_enemy.m_nActionPoints <= 0)
        {
            animator.SetBool("PlayerTurn", true);
        }
    }

    private void OnDisable()
    {

    }
}
