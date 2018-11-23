using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelStartDialog : MonoBehaviour
{

	[TextArea(3,10)]
	public string dialogText;
	public AudioClip dialogAudio;
	public GameObject dialogPanel;

	public AudioSource source;
	public Text dialogBox;

	private void Start()
	{
		source = GameObject.Find("Camera").GetComponent<AudioSource>();
		dialogBox = GameObject.Find("DialogTextBox").GetComponent<Text>();
		dialogPanel = GameObject.Find("DialogPanel");
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

			yield return new WaitForSeconds((dialogAudio.length / dialogText.Length) / 12.0f);
		}
	}
}
