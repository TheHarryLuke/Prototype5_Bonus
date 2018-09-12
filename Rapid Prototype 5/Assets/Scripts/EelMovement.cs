using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelMovement : MonoBehaviour
{
    public Transform m_Target;

    public int m_AttackRange = 2;
    public int m_FollowRange = 10;
    public float m_MoveSpeed = 5f;
    public GameObject m_electricParticle;
    public GameObject m_screenFlash;
    public GameObject m_screenFlash2;

    private Vector3 m_AltTarget;
    private float m_TimeForNextTarget;

    private ScreenShake m_screenShake;

    private float m_Distance;
    private bool m_bDetected;
    private bool m_bAttacked;
    private Animator m_anim;

    // Use this for initialization
    void Start ()
    {
        m_screenFlash.SetActive(false);
        m_screenFlash2.SetActive(false);

        m_anim = GetComponentInChildren<Animator>();
        m_screenShake = GameObject.FindGameObjectWithTag("ScreenShaker").GetComponent<ScreenShake>();
        if (WaterLevel.m_WaterLevel < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, WaterLevel.m_WaterLevel, transform.position.z);
        }

        m_TimeForNextTarget = 0;
    }
	
	void Update ()
    {
        m_Distance = (m_Target.transform.position - transform.position).magnitude;
        
        if(m_Distance <= m_FollowRange)
        {
            m_bDetected = true;
        }
        else
        {
            m_bDetected = false;
        }

        if(m_bDetected)
        {
            FollowThePlayer();
        }
        else
        {
            SwimRandom();
        }
	}

    private void FollowThePlayer()
    {
        if(m_Distance >= m_AttackRange)
        {
            m_bAttacked = false;
            transform.position += transform.forward * m_MoveSpeed * Time.deltaTime;
            if (WaterLevel.m_WaterLevel < transform.position.y)
            {
                transform.position = new Vector3(transform.position.x, WaterLevel.m_WaterLevel, transform.position.z);
            }
        }
        else
        {
            if (!m_bAttacked)
            {
                m_bAttacked = true;

                //Get the spawn point from the player
                Transform spawnPoint = m_Target.Find("ElectricSpawnPoint").transform;

                //Spawn eletric particles infront of camera + screenshake
                GameObject bElectricParticle = (GameObject)Instantiate(m_electricParticle, spawnPoint.position, spawnPoint.rotation);
                GameObject bEelParticle = (GameObject)Instantiate(m_electricParticle, transform.position, transform.rotation);

                //Player electric
                bElectricParticle.transform.parent = spawnPoint;
                bElectricParticle.GetComponent<ParticleSystem>().Play();
                //StartCoroutine(m_screenShake.Shake(.5f, .05f));
                m_screenShake.CamShake();
                m_anim.SetTrigger("attack");
                StartCoroutine(FlashUIOnce( m_screenFlash, m_screenFlash2));
                //Eel electric
                bEelParticle.GetComponent<ParticleSystem>().Play();

                GetComponent<AudioSource>().Play();

                Destroy(bEelParticle, 1.5f);
                Destroy(bElectricParticle, 2.0f);
            }
            //SpawnMore.SpawnMoreOfThisType(this.gameObject);
            //GameObject.FindGameObjectWithTag("SpawnMore").GetComponent<SpawnMore>().SpawnMoreOfThisType(this.gameObject);
        }

        transform.LookAt(m_Target.transform);
    }

    IEnumerator FlashUIOnce(GameObject _UIObjectToFlash, GameObject _UIObjectToFlash2)
    {
        _UIObjectToFlash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        _UIObjectToFlash.SetActive(false);
        _UIObjectToFlash2.SetActive(true);
        yield return new WaitForSeconds(.1f);
        _UIObjectToFlash2.SetActive(false);
        _UIObjectToFlash.SetActive(true);
        yield return new WaitForSeconds(.1f);
        _UIObjectToFlash.SetActive(false);
        _UIObjectToFlash2.SetActive(true);
        yield return new WaitForSeconds(.1f);
        _UIObjectToFlash2.SetActive(false);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, m_FollowRange);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, m_AttackRange);
    }

    private void SwimRandom()
    {
        bool bTurning;
        if (Vector3.Distance(transform.position, new Vector3(0.0f, 0.0f, 0.0f)) >= 15.0f)
        {
            bTurning = true;
        }
        else
        {
            bTurning = false;
        }

        if (bTurning)
        {
            Vector3 direction = m_AltTarget - transform.position;
            transform.LookAt(direction);
        }

        if(Time.time >= m_TimeForNextTarget)
        {
            m_AltTarget = new Vector3(Random.Range(-12, 12), transform.position.y, Random.Range(-12, 12));

            m_TimeForNextTarget = Random.Range(2, 10);
        }
        
        transform.position += transform.forward * m_MoveSpeed * Time.deltaTime;
        if (WaterLevel.m_WaterLevel < transform.position.y)
        {
            transform.position = new Vector3(transform.position.x, WaterLevel.m_WaterLevel, transform.position.z);
        }
    }
}
