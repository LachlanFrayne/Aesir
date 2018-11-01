using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
	public GameObject pauseMenu;


	public void PauseGame()
	{
		Time.timeScale = 0.0f;
		pauseMenu.SetActive(true);
	}

	public void ResumeGame()
	{
		Time.timeScale = 1.0f;
		pauseMenu.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGame();
		}
	}
}
