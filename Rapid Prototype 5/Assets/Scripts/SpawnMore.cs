using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMore : MonoBehaviour {

    public int m_SpawnAmount = 5;
    public GameObject[] SpawnLocations;

    private GameObject m_WhatToSpawn;

    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnMoreOfThisType(GameObject _Type)
    {
        int iRandTime;

        m_WhatToSpawn = _Type;

        int iTemp = 0;
        while(iTemp < m_SpawnAmount)
        {
            iRandTime = Random.Range(0, 5);
            Invoke("Spawn", iRandTime + 0.5f);

            iTemp += 1;
        }
    }

    private void Spawn()
    {
        int iRandPlace = Random.Range(0, SpawnLocations.Length);

        GameObject temp = Instantiate(m_WhatToSpawn, SpawnLocations[iRandPlace].GetComponent<Transform>());
        temp.transform.parent = null;
    }
}
