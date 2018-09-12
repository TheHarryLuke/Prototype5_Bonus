using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public bool[] m_bIsFull;
    public GameObject[] m_inventorySlot;

    public bool hasYellowKey, hasBlueKey, hasGreenKey;
    public GameObject m_blueKeyUI;
    private PickupObject m_pickUp;

    public void Update ()
    {
        if(hasYellowKey)
        {
            //Display yellow key on screen
        }
        if (hasBlueKey)
        {
            m_blueKeyUI.SetActive(true);
            //Display blue key on screen
        }
        else
        {
            m_blueKeyUI.SetActive(false);
        }
        if (hasGreenKey)
        {
            //Display green key on screen
        }
    }

    public bool SearchKeyInInventory(PickupObject.EPickups _pickup)
    {
        for(int i = 0; i < m_inventorySlot.Length; ++i)
        {
            if(m_bIsFull[i])
            {
                if(m_inventorySlot[i].GetComponent<PickupObject>().pickUpObject.pickupType == _pickup)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
