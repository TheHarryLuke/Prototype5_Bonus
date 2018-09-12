using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetAtWaterHeight : MonoBehaviour {

    public float m_DestroyAtWaterHeight = 3f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.position = new Vector3(transform.position.x, WaterLevel.m_WaterLevel, transform.position.z);
        
        if(WaterLevel.m_WaterLevel >= m_DestroyAtWaterHeight)
        {
            Destroy(gameObject);
        }
	}
}
