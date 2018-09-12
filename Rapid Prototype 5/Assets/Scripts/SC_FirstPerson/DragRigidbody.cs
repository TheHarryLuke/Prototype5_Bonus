/*
DragRigidbodyUse.cs ver. 13.5.16 - wirted by ThunderWire Games * Script for Drag & Drop & Throw Objects & Draggable Door & PickupObjects
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

	[System.Serializable]
	public class GrabObjectClass{
		public bool m_FreezeRotation = true;
		public string m_GrabTag = "Pickable";
		//public float m_PickupRange = 3f; 
		public float m_ThrowStrength = 50f;
		public float m_distance = 3f;
		public float m_maxDistanceGrab = 4f;
	}
	
	[System.Serializable]
	public class ItemGrabClass{
		public bool m_FreezeRotation = true;		
		public string m_GrabItemsTag = "Item"; 
		//public float m_PickupRange = 2f;
		public float m_ThrowStrength = 45f;
		public float m_distance = 1f;
		public float m_maxDistanceGrab = 2.5f;
	}	
	
	[System.Serializable]
	public class DoorGrabClass{	
		public string m_DoorsTag = "Door"; 
		//public float m_PickupRange = 2f;
		public float m_ThrowStrength = 10f;
		public float m_distance = 2f;
		public float m_maxDistanceGrab = 3f;
	}

public class DragRigidbody : MonoBehaviour {
	
	public GameObject playerCam;
	
	public string GrabButton = "Grab";
	public string ThrowButton = "Throw";
	
	public float PickupRange = 3f;
	public bool HoldGrab;
	private bool m_HoldGrab;
	
	private float ThrowStrength = 50f;
	private float distance = 3f;
	private float maxDistanceGrab = 4f;
	
	public GrabObjectClass ObjectGrab = new GrabObjectClass();
	public ItemGrabClass ItemGrab = new ItemGrabClass();
	public DoorGrabClass DoorGrab = new DoorGrabClass();
	
	private Ray playerAim;
	private GameObject objectHeld;
	private bool isObjectHeld;
	private bool tryPickupObject;
	private bool isPressed;
	private bool GrabObject;
	
	void Start () {
		isObjectHeld = false;
		tryPickupObject = false;
		isPressed = false;
		GrabObject = false;
		objectHeld = null;
		m_HoldGrab = HoldGrab;
		if(!HoldGrab)
		{
			ObjectGrab.m_ThrowStrength = ObjectGrab.m_ThrowStrength * 10;
			ItemGrab.m_ThrowStrength = ItemGrab.m_ThrowStrength * 10;
		}
	}
	
	void FixedUpdate () {
		if(!HoldGrab){
			if(Input.GetButtonDown(GrabButton) && !isPressed){
				isPressed = true;
				GrabObject = !GrabObject;
			}else if(isPressed){
				isPressed = false;
			}
		}else{
			if(Input.GetButton(GrabButton)){
				GrabObject = true;
			}else{
				GrabObject = false;
			}
		}

		if(GrabObject){
			if(!isObjectHeld){
				tryPickObject();
				tryPickupObject = true;
			} else {
				holdObject();
			}
		}else if(isObjectHeld){
			DropObject();
		}
		
		if(Input.GetButton(ThrowButton) && isObjectHeld){
			isObjectHeld = false;
			objectHeld.GetComponent<Rigidbody>().useGravity = true;
			ThrowObject();
		}
	}
	
	void Update()
	{
		Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;
		
		if (Physics.Raycast (playerAim, out hit, PickupRange)){
			if(hit.collider.tag == ObjectGrab.m_GrabTag){
				//PickupRange = ObjectGrab.m_PickupRange; 
				if(m_HoldGrab){
					HoldGrab = true;
				}else{
					HoldGrab = false;
				}
				ThrowStrength = ObjectGrab.m_ThrowStrength;
				distance = ObjectGrab.m_distance;
				maxDistanceGrab = ObjectGrab.m_maxDistanceGrab;
			}
			if(hit.collider.tag == ItemGrab.m_GrabItemsTag){
				//PickupRange = ItemGrab.m_PickupRange; 
				if(m_HoldGrab){
					HoldGrab = true;
				}else{
					HoldGrab = false;
				}
				ThrowStrength = ItemGrab.m_ThrowStrength;
				distance = ItemGrab.m_distance;
				maxDistanceGrab = ItemGrab.m_maxDistanceGrab;
			}
			if(hit.collider.tag == DoorGrab.m_DoorsTag){
				//PickupRange = DoorGrab.m_PickupRange; 
				HoldGrab = true;
				ThrowStrength = DoorGrab.m_ThrowStrength;
				distance = DoorGrab.m_distance;
				maxDistanceGrab = DoorGrab.m_maxDistanceGrab;
			}
		}
	}
	
	private void tryPickObject(){
		Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		RaycastHit hit;	
		
		if (Physics.Raycast (playerAim, out hit, PickupRange)){
			objectHeld = hit.collider.gameObject;
			if(hit.collider.tag == ObjectGrab.m_GrabTag && tryPickupObject){
				isObjectHeld = true;
				objectHeld.GetComponent<Rigidbody>().useGravity = false;
				if(ObjectGrab.m_FreezeRotation){
					objectHeld.GetComponent<Rigidbody>().freezeRotation = true;
				}
				if(ObjectGrab.m_FreezeRotation == false){
					objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
				}
			}
			if(hit.collider.tag == ItemGrab.m_GrabItemsTag && tryPickupObject){
				isObjectHeld = true;
				objectHeld.GetComponent<Rigidbody>().useGravity = true;
				if(ItemGrab.m_FreezeRotation){
					objectHeld.GetComponent<Rigidbody>().freezeRotation = true;
				}
				if(ItemGrab.m_FreezeRotation == false){
					objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
				}
			}
			if(hit.collider.tag == DoorGrab.m_DoorsTag && tryPickupObject){
				isObjectHeld = true;
				objectHeld.GetComponent<Rigidbody>().useGravity = true;
				objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
			}
		}
	}
	
	private void holdObject(){
		Ray playerAim = playerCam.GetComponent<Camera>().ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
		
		Vector3 nextPos = playerCam.transform.position + playerAim.direction * distance;
		Vector3 currPos = objectHeld.transform.position;
		
		objectHeld.GetComponent<Rigidbody>().velocity = (nextPos - currPos) * 10;
		
		if (Vector3.Distance(objectHeld.transform.position, playerCam.transform.position) > maxDistanceGrab)
		{
           DropObject();
		}
	}
	
    private void DropObject()
    {
		isObjectHeld = false;
		tryPickupObject = false;
		objectHeld.GetComponent<Rigidbody>().useGravity = true;
		objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
		objectHeld = null;
		GrabObject = false;
		isPressed = false;
    }
	
    private void ThrowObject()
    {
        objectHeld.GetComponent<Rigidbody>().AddForce(playerCam.transform.forward * ThrowStrength);
		objectHeld.GetComponent<Rigidbody>().freezeRotation = false;
		objectHeld = null;
		GrabObject = false;
		isPressed = false;
    }
}
