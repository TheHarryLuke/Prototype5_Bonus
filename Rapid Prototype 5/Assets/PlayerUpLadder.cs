using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpLadder : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if("Player" == other.tag)
        {
            if(other.transform.position.y + 1f < transform.position.y + 2f)
            {
                other.transform.Translate(other.transform.up * 5f);
            }
        }
    }
}
