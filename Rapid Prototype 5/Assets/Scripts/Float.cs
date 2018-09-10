using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Float : MonoBehaviour {

    public float m_FloatHeight = 1.0f;
    public float m_BounceDamp = 0.05f;
    public Vector3 m_BouyancyCentreOffset;

    private float m_ForceFactor;
    private Vector3 m_ActionPoint;
    private Vector3 m_UpLift;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
	

	void Update ()
    {
        m_ActionPoint = transform.position + transform.TransformDirection(m_BouyancyCentreOffset);
        m_ForceFactor = 1f - ((m_ActionPoint.y - WaterLevel.m_WaterLevel) / m_FloatHeight);

        if(m_ForceFactor > 0.0f)
        {
            m_UpLift = -Physics.gravity * (m_ForceFactor - rb.velocity.y * m_BounceDamp);
            rb.AddForceAtPosition(m_UpLift, m_ActionPoint);
        }
    }
}
