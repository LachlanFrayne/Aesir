using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

	public void ExitToMainMenu()
	{
		SceneManager.LoadScene(0);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGame();
		}
	}
}
