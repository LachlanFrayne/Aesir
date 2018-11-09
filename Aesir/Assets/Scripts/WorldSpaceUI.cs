using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldSpaceUI : MonoBehaviour {

	public Camera camera;
	public List<GameObject> m_heroes;
	public GameObject uiHealth;
	public Canvas canvas;
	public List<GameObject> healths;

	public GameObject ThorHealthBar;

	public Image thorHealthBarOverheadImage;
	public Text thorHealthOverheadLabel;
	public Text thorHealthMaxOverheadLabel;

	public GameObject LokiHealthBar;

	public Image lokiHealthBarOverheadImage;
	public Text lokiHealthOverheadLabel;
	public Text lokiHealthMaxOverheadLabel;
	public GameObject FreyaHealthBar;

	public Image freyaHealthBarOverheadImage;
	public Text freyaHealthOverheadLabel;
	public Text freyaHealthMaxOverheadLabel;

	public GameObject thor;
	public GameObject freya;
	public GameObject loki;

	void Awake ()
	{
		thor = GameObject.Find("Thor");
		freya = GameObject.Find("Freya");
		loki = GameObject.Find("Loki");

		camera = Camera.main;
		m_heroes = new List<GameObject>();

		m_heroes.Add(thor);
		m_heroes.Add(loki);
		m_heroes.Add(freya);

		test();

		ThorHealthBar = healths[0];
		LokiHealthBar = healths[1];
		FreyaHealthBar = healths[2];

		thorHealthBarOverheadImage = ThorHealthBar.transform.GetChild(0).GetComponent<Image>();
		thorHealthMaxOverheadLabel = ThorHealthBar.transform.GetChild(2).GetComponent<Text>();
		thorHealthOverheadLabel = ThorHealthBar.transform.GetChild(3).GetComponent<Text>();

		lokiHealthBarOverheadImage = LokiHealthBar.transform.GetChild(0).GetComponent<Image>();
		lokiHealthMaxOverheadLabel = LokiHealthBar.transform.GetChild(2).GetComponent<Text>();
		lokiHealthOverheadLabel = LokiHealthBar.transform.GetChild(3).GetComponent<Text>();

		freyaHealthBarOverheadImage = FreyaHealthBar.transform.GetChild(0).GetComponent<Image>();
		freyaHealthMaxOverheadLabel = FreyaHealthBar.transform.GetChild(2).GetComponent<Text>();
		freyaHealthOverheadLabel = FreyaHealthBar.transform.GetChild(3).GetComponent<Text>();


	}
	
	void test()
	{
		foreach (GameObject hero in m_heroes)
		{
			GameObject health = Instantiate(uiHealth);
			health.name = uiHealth.name + ' ' + hero.name;
			healths.Add(health);
			health.transform.SetParent(canvas.transform, false);
			
		}
	}
	void Update ()
	{	
		ThorHealthBar.transform.position = new Vector3(thor.transform.position.x, thor.transform.position.y + 2, thor.transform.position.z);
		LokiHealthBar.transform.position = new Vector3(loki.transform.position.x, loki.transform.position.y + 2, loki.transform.position.z);
		FreyaHealthBar.transform.position = new Vector3(freya.transform.position.x, freya.transform.position.y + 2, freya.transform.position.z);

		ThorHealthBar.transform.LookAt(ThorHealthBar.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
		LokiHealthBar.transform.LookAt(LokiHealthBar.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
		FreyaHealthBar.transform.LookAt(FreyaHealthBar.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);

		

	}
}
