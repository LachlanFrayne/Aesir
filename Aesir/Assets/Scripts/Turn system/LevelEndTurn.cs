using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEndTurn : StateMachineBehaviour {

	public LevelEndDialog levelEndDialog;

	private float timer;

	public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		levelEndDialog = animator.GetComponent<LevelEndDialog>();

		levelEndDialog.source.PlayOneShot(levelEndDialog.dialogAudio, 1f);

		levelEndDialog.StartDialog();

		if(levelEndDialog.dialogAudio != null)
		{
			timer = levelEndDialog.dialogAudio.length;
		}
		else
		{
			timer = 0;
		}

		levelEndDialog.dialogPanel.SetActive(true);
		levelEndDialog.dialogBox.text = "";
	}

	public override void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		timer -= Time.deltaTime;

		if (timer <= 0)
		{
			levelEndDialog.StopAllCoroutines();

			levelEndDialog.dialogPanel.SetActive(false);

			if(SceneManager.GetActiveScene().buildIndex >= SceneManager.sceneCountInBuildSettings )
			{
				SceneManager.LoadScene(0);
			}
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}

		if (Input.GetKeyDown(KeyCode.Return))
		{
			levelEndDialog.StopAllCoroutines();

			levelEndDialog.dialogPanel.SetActive(false);

			SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
		}
	}

	public override void OnStateExit(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
	{
		levelEndDialog.StopAllCoroutines();

		levelEndDialog.dialogPanel.SetActive(false);
	}
}
