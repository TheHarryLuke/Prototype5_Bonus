using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour {

    public bool hasYellowKey, hasBlueKey, hasGreenKey;
    public GameObject blueKeyUI;

    public void Update ()
    {
        if(hasYellowKey)
        {
            //Display yellow key on screen
        }
        if (hasBlueKey)
        {
            blueKeyUI.SetActive(true);
            //Display blue key on screen
        }
        else
        {
            blueKeyUI.SetActive(false);
        }
        if (hasGreenKey)
        {
            //Display green key on screen
        }
    }
}
