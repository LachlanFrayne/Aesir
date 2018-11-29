using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RotateHeart : MonoBehaviour {

    public Image heart;
    public Image heartTwo;

    public GameObject camera;

    private void Update()
    {
        heart.transform.LookAt(heart.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        heartTwo.transform.LookAt(heartTwo.transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
    }
}
