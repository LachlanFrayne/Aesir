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


		levelStartDialog.StartDialog();

		if (levelStartDialog.source != null)
		{
			levelStartDialog.source.PlayOneShot(levelStartDialog.dialogAudio, 1f);
			timer = 15;// levelStartDialog.dialogAudio.length;

		}

		levelStartDialog.dialogPanel.SetActive(true);
		levelStartDialog.dialogBox.text = "";

		
		Transform ui = animator.GetComponent<EnemyLinker>().GetUI();

		ui.Find("Panel").gameObject.SetActive(false);
		ui.Find("BackgroundThor").gameObject.SetActive(false);
		ui.Find("BackgroundFreya").gameObject.SetActive(false);
		ui.Find("BackgroundLoki").gameObject.SetActive(false);
		ui.Find("MoveSet").gameObject.SetActive(false);
		ui.Find("EndTurnButton").gameObject.SetActive(false);
		ui.Find("EnemyPopUp").gameObject.SetActive(false);
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		timer -= Time.deltaTime;

		if (timer <= 0)
		{
			animator.SetBool("PlayerTurn", true);
		}

		if (Input.GetKeyDown(KeyCode.Return))
		{
			animator.SetBool("PlayerTurn", true);
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		levelStartDialog.StopAllCoroutines();

		levelStartDialog.source.Stop();

		levelStartDialog.dialogPanel.SetActive(false);

		Transform ui = animator.GetComponent<EnemyLinker>().GetUI();

		ui.Find("Panel").gameObject.SetActive(true);
		ui.Find("BackgroundThor").gameObject.SetActive(true);
		ui.Find("BackgroundFreya").gameObject.SetActive(true);
		ui.Find("BackgroundLoki").gameObject.SetActive(true);
		ui.Find("MoveSet").gameObject.SetActive(true);
		ui.Find("EndTurnButton").gameObject.SetActive(true);
		ui.Find("EnemyPopUp").gameObject.SetActive(true);
	}
}
