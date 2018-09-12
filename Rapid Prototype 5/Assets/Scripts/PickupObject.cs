using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour {
	public enum EPickups
    {
        YellowKey,
        BlueKey,
        GreenKey,
    }

    [System.Serializable]
    public struct EPickUpStats
    {
        public EPickups pickupType;
        public bool unlocksDoor1;
    }

    //public EPickups pickupType;
    public EPickUpStats pickUpObject;

    
}
