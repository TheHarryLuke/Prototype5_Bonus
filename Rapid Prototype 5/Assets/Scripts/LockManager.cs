using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockManager : MonoBehaviour {

    public enum ELocks
    {
        YellowLock,
        BlueLock,
        GreenLock
    }

    public ELocks lockType;
}
