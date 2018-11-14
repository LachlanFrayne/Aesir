using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStartDialog : MonoBehaviour
{

	[TextArea(3,10)]
	public string dialogText;
	public AudioClip dialogAudio;

	public AudioSource source;
	public Text dialogBox;

	private void Start()
	{
		source = GameObject.Find("Camera").GetComponent<AudioSource>();
		dialogBox = GameObject.Find("DialogTextBox").GetComponent<Text>();
	}

	public void StartDialog()
	{
		StopAllCoroutines();
		StartCoroutine(TypeDialog());
	}

	IEnumerator TypeDialog()
	{
		dialogBox.text = "";

		foreach (char letter in dialogText.ToCharArray())
		{
			dialogBox.text += letter;

			yield return null;
		}

		
	}
}
