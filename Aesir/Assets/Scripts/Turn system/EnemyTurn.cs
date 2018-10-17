using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurn : StateMachineBehaviour
{
    //JM:STARTHERE, add a listener on an end turn button, and remove in the onexit then complete other states, also fix the thor variable not being set in the moveDecision script

    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
       
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
