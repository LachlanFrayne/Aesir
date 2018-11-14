using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelStartTurn : StateMachineBehaviour
{
	public LevelStartDialog levelStartDialog;

	private float timer;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		levelStartDialog = animator.GetComponent<LevelStartDialog>();

		levelStartDialog.source.PlayOneShot(levelStartDialog.dialogAudio, 1f);

		levelStartDialog.StartDialog();

		timer = levelStartDialog.dialogAudio.length;
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		timer -= Time.deltaTime;

		if (timer <= 0)
		{
			animator.SetBool("PlayerTurn", true);
		}
	}

	
}
