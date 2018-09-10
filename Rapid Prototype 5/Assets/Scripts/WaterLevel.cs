using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterLevel : MonoBehaviour {

    public static float m_WaterLevel = 0.0f;

    public float m_WaterLevelRiseSpeed = 0.1f;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        m_WaterLevel += (m_WaterLevelRiseSpeed * Time.deltaTime);
        gameObject.transform.Translate(Vector3.up * m_WaterLevelRiseSpeed * Time.deltaTime);
    }
}
