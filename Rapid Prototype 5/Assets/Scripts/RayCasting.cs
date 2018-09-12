using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RayCasting : MonoBehaviour {

    public GameObject m_keyObtainedParticle;
    public Image CanPickUp;
    public GameObject m_SpawnEachInteraction;

    public float m_fRayDistance = 3;
    bool m_bPressedAlready = false;
    bool m_bActionDone = false;

    private GameObject m_Player;
    private RaycastHit m_objectHit;
    private Inventory m_inventory;
    //public EventManager m_eventManager;

    // Use this for initialization
    void Start () {
        CanPickUp.gameObject.SetActive(false);

        m_Player = GameObject.FindWithTag("Player");
        m_inventory = m_Player.GetComponent<Inventory>();
        //m_eventManager = m_eventManager.GetComponent<EventManager>();
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
        //Draw a debug ray that displays in the editor
        Debug.DrawRay(this.transform.position, this.transform.forward * m_fRayDistance, Color.magenta);

        //Get raycast of object the player 
        if (Physics.Raycast(this.transform.position, this.transform.forward, out m_objectHit, m_fRayDistance))
        {
            if(m_objectHit.collider.CompareTag("Pickup") || m_objectHit.collider.CompareTag("Lock"))
            {
                CanPickUp.gameObject.SetActive(true);
            }
            else
            {
                CanPickUp.gameObject.SetActive(false);
            }
            
            if (GetAxisOnce("Pickup"))
            {
                if (m_objectHit.collider.CompareTag("Pickup"))
                {
                    if (m_objectHit.collider.gameObject.GetComponent<PickupObject>().pickUpObject.pickupType == PickupObject.EPickups.BlueKey)
                    {
                        m_Player.GetComponent<Inventory>().hasBlueKey = true;
                        m_bActionDone = true;
                        Destroy(m_objectHit.collider.gameObject);
                    }
                }
                if (m_objectHit.collider.CompareTag("Lock")) 
                {
                    if (m_objectHit.collider.gameObject.GetComponent<LockManager>().lockType == LockManager.ELocks.BlueLock)
                    {
                        if(m_Player.GetComponent<Inventory>().hasBlueKey)
                        {
                            m_Player.GetComponent<Inventory>().hasBlueKey = false;
                            m_bActionDone = true;
                            Destroy(m_objectHit.collider.gameObject);
                        }
                    }
                }

                if(m_bActionDone)
                {
                    m_bActionDone = false;
                    GameObject bKeyParticle = (GameObject)Instantiate(m_keyObtainedParticle, m_objectHit.transform.position, m_objectHit.transform.rotation);
                    bKeyParticle.GetComponent<ParticleSystem>().Play();
                    Destroy(bKeyParticle, 2.0f);
                    
                    GameObject.FindGameObjectWithTag("SpawnMore").GetComponent<SpawnMore>().SpawnMoreOfThisType(m_SpawnEachInteraction);
                }
            }
        }
	}

    void AddItemToInventory()
    {

    }
}
