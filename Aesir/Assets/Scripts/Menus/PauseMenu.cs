using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
	public GameObject m_pauseMenu;

	public Canvas m_canvas;

	public void PauseGame()
	{
		m_canvas.transform.Find("MoveSet").gameObject.SetActive(false);
		m_canvas.transform.Find("EndTurnButton").gameObject.SetActive(false);
		m_canvas.transform.Find("Panel").gameObject.SetActive(false);
		Time.timeScale = 0.0f;
		m_pauseMenu.SetActive(true);
	}

	public void ResumeGame()
	{
		m_canvas.transform.Find("MoveSet").gameObject.SetActive(true);
		m_canvas.transform.Find("EndTurnButton").gameObject.SetActive(true);
		m_canvas.transform.Find("Panel").gameObject.SetActive(true);
		Time.timeScale = 1.0f;
		m_pauseMenu.SetActive(false);
	}

	public void ExitToMainMenu()
	{
		SceneManager.LoadScene(0);
	}

	private void Start()
	{
		m_pauseMenu.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			PauseGame();
		}
	}
}
