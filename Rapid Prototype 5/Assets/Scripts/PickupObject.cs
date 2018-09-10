using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupObject : MonoBehaviour {

	public enum EPickups
    {
        YellowKey,
        BlueKey,
        GreenKey
    }

    public EPickups pickupType;
}
