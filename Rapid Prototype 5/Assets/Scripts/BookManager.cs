using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookManager : MonoBehaviour {

    //private PickupObject m_pickup;

    public enum EBook
    {
        CorrectKeyBook = 0,
        WrongKeyBook,
        EmptyBook,
    }

    public EBook bookType;

    public void Start()
    {
        int randomBook = Random.Range(0, 2);
        //m_pickup;
    }
}
