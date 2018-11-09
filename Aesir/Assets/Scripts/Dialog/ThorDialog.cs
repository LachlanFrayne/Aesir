using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThorDialog : MonoBehaviour 
{
	Text m_dialogBox;

	public string m_thorHurtDialog;

	private void Start()
	{
		m_dialogBox = GameObject.Find("DialogBox").GetComponent<Text>();
	}

	public void ThorHurtDialog()
	{
		m_dialogBox.text = m_thorHurtDialog;
	}
}
