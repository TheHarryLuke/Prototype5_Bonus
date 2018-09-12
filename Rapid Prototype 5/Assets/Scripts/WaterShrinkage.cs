using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterShrinkage : MonoBehaviour {

    private ParticleSystem m_ParticleSystem;

	// Use this for initialization
	void Start () {
        m_ParticleSystem = GetComponent<ParticleSystem>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        var main = m_ParticleSystem.main;
        main.startLifetime = 3f - WaterLevel.m_WaterLevel;
    }
}
