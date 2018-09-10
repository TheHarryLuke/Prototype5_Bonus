using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasting : MonoBehaviour {

    public float m_fRayDistance = 3;
    RaycastHit m_objectHit;
    bool m_bPressedAlready = false;
    GameObject m_Player;

	// Use this for initialization
	void Start () {
        m_Player = GameObject.FindWithTag("Player");

    }

    bool GetAxisOnce(string _name)
    {
        bool current = Input.GetAxis(_name) > 0;

        if (current && m_bPressedAlready)
        {
            return false;
        }

        m_bPressedAlready = current;

        return current;
    }

    // Update is called once per frame
    void Update () {
        Debug.DrawRay(this.transform.position, this.transform.forward * m_fRayDistance, Color.magenta);
        if(Physics.Raycast(this.transform.position, this.transform.forward, out m_objectHit, m_fRayDistance))
        {
            if (GetAxisOnce("Pickup"))
            {
                if(m_objectHit.collider.tag == "Pickup")
                {
                    if (m_objectHit.collider.gameObject.GetComponent<PickupObject>().pickupType == PickupObject.EPickups.BlueKey)
                    {
                        m_Player.GetComponent<Inventory>().hasBlueKey = true;
                        //Debug.Log("Picked up: " + m_rObjectHit.collider.gameObject.name);
                        Destroy(m_objectHit.collider.gameObject);
                    }
                }
                if (m_objectHit.collider.tag == "Lock")
                {
                    if (m_objectHit.collider.gameObject.GetComponent<LockManager>().lockType == LockManager.ELocks.BlueLock)
                    {
                        if(m_Player.GetComponent<Inventory>().hasBlueKey)
                        {
                            m_Player.GetComponent<Inventory>().hasBlueKey = false;
                            Destroy(m_objectHit.collider.gameObject);
                        }
                        //Debug.Log("Picked up: " + m_rObjectHit.collider.gameObject.name);
                    }
                }
            }
        }
	}
}
