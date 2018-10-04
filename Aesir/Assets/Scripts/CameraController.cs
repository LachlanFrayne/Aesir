using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public int m_xMax;
    public int m_xMin;
    public int m_zMax;
    public int m_zMin;

    public int m_height;
    public int m_speed;
    Rigidbody rb;

    // Use this for initialization
    void Start ()
    {
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update ()
    {

		if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(new Vector3(0, 0, m_speed) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.Translate(new Vector3(0, 0, -m_speed) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.Translate(new Vector3(-m_speed, 0, 0) * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.Translate(new Vector3(m_speed, 0, 0) * Time.deltaTime); 
        }


        /////////////////////////////////////////////////////////////////////////////////
        if (transform.position.x > m_xMax)
        {
            transform.position = new Vector3(m_xMax, transform.position.y, transform.position.z);
        }
        if (transform.position.x < m_xMin)
        {
            transform.position = new Vector3(m_xMin, transform.position.y, transform.position.z);
        }
        if (transform.position.z > m_zMax)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, m_zMax);
        }
        if (transform.position.z < m_zMin)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, m_zMin);
        }

        if (Input.GetMouseButtonDown(0))        //focus on gameobject clicked
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                transform.position = hit.transform.position;
            }
        }

        transform.position = new Vector3(transform.position.x, m_height, transform.position.z);     //sets y to m_height
    }
}
