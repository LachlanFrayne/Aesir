using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitNodeHighlight : MonoBehaviour
{
	public Material material;

	void Update ()
	{
		GetComponent<Renderer>().material = material;
	}
}
