using System;
using UnityEngine;
using UnityStandardAssets.Utility;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

namespace UnityStandardAssets.Characters.FirstPerson
{

    [RequireComponent(typeof (CharacterController))]
    [RequireComponent(typeof (AudioSource))]
    public class FirstPersonController : MonoBehaviour
    {
        public float WalkingSpeedChange = 1.0f;
        public float RunningSpeedChange = 2.0f;
        [SerializeField] private bool m_IsWalking;
        [SerializeField] private float m_WalkSpeed;
        [SerializeField] private float m_RunSpeed;
        [SerializeField] private float climbSpeed = 3.0f;
        [SerializeField] private float climbRate = 0.5f;
        private float climbDownThreshold = -0.4f;

        [SerializeField] [Range(0f, 1f)] private float m_RunstepLenghten;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;

        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private bool m_UseFovKick;
        [SerializeField] private FOVKick m_FovKick = new FOVKick();
        [SerializeField] private bool m_UseHeadBob;
        [SerializeField] private CurveControlledBob m_HeadBob = new CurveControlledBob();
        [SerializeField] private LerpControlledBob m_JumpBob = new LerpControlledBob();
        [SerializeField] private float m_StepInterval;
        [SerializeField] private AudioClip[] m_LadderSounds;
        [SerializeField] private AudioClip[] m_FootstepSounds;    // an array of footstep sounds that will be randomly selected from.
        [SerializeField] private AudioClip m_JumpSound;           // the sound played when character leaves the ground.
        [SerializeField] private AudioClip m_LandSound;           // the sound played when character touches back on ground.

        /* LADDER */
        private bool m_onLadder = false;
        private bool useLadder = true;

        private Vector3 climbDirection = Vector3.up;
        private Vector3 lateralMove = Vector3.zero;
        private Vector3 ladderMovement = Vector3.zero;
        private Rigidbody rigidbody;
        private CharacterController ChController;
        private GameObject LadderObject;
        private float CamRot;
        private float playTime = 0.0f;


        private Camera m_Camera;
        public GameObject water;
        private bool m_Jump;
        private float m_YRotation;
        private Vector2 m_Input;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private Vector3 m_OriginalCameraPosition;
        private float m_StepCycle;
        private float m_NextStep;
        private bool m_Jumping;
        private AudioSource m_AudioSource;

        public Color fogColorVal = new Color(0.17f, 0.22f, 0.43f, 1.0f);
        public float fogDensity = 0.5f;
        public float fogEnd = 20.0f;

        // Use this for initialization
        private void Start()
        {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_OriginalCameraPosition = m_Camera.transform.localPosition;
            m_FovKick.Setup(m_Camera);
            m_HeadBob.Setup(m_Camera, m_StepInterval);
            m_StepCycle = 0f;
            m_NextStep = m_StepCycle/2f;
            m_Jumping = false;
            m_AudioSource = GetComponent<AudioSource>();
			m_MouseLook.Init(transform , m_Camera.transform);
            rigidbody = this.GetComponent<Rigidbody>();
            m_onLadder = false;
            useLadder = true;
        }


        // Update is called once per frame
        void Update()
        {
            if (m_Camera.transform.position.y < water.transform.position.y)
            {
                RenderSettings.fogColor = new Color(fogColorVal.r, fogColorVal.g, fogColorVal.b, fogColorVal.a);
                RenderSettings.fogDensity = fogDensity;
                RenderSettings.fogEndDistance = fogEnd;
                RenderSettings.fog = true;
            }
            else
            {
                RenderSettings.fog = false;
            }

            RotateView();

            if (EventSystem.current.IsPointerOverGameObject())
                return;
                        
            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded && !m_onLadder)
            {
                StartCoroutine(m_JumpBob.DoBobCycle());
                PlayLandingSound();
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded)
            {
                m_MoveDir.y = 0f;
            }

            m_PreviouslyGrounded = m_CharacterController.isGrounded;

            if (m_onLadder && Input.GetAxis("Ladder") > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
                LadderUpdate();
            }
            else
            {
                LadderObject = null;
                rigidbody.useGravity = true;
                rigidbody.isKinematic = true;
            }
        }


        private void PlayLandingSound()
        {
            m_AudioSource.clip = m_LandSound;
            m_AudioSource.Play();
            m_NextStep = m_StepCycle + .5f;
        }


        private void FixedUpdate()
        {
            float speed;
            GetInput(out speed);

            if (!m_onLadder)
            {
                // always move along the camera forward as it is the direction that it being aimed at
                Vector3 desiredMove = transform.forward * m_Input.y + transform.right * m_Input.x;

                // get a normal for the surface that is being touched to move along it
                RaycastHit hitInfo;
                Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo,
                                   m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
                desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;

                m_MoveDir.x = desiredMove.x * speed;
                m_MoveDir.z = desiredMove.z * speed;


                if (!m_onLadder)
                {
                    m_MoveDir.y = -m_StickToGroundForce;

                    if (m_Jump)
                    {
                        m_MoveDir.y = m_JumpSpeed;
                        PlayJumpSound();
                        m_Jump = false;
                        m_Jumping = true;
                    }
                }
                else
                {
                    m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
                }
                m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);

                ProgressStepCycle(speed);
            }
            UpdateCameraPosition(speed);

            m_MouseLook.UpdateCursorLock();
        }


        private void PlayJumpSound()
        {
            m_AudioSource.clip = m_JumpSound;
            m_AudioSource.Play();
        }


        private void ProgressStepCycle(float speed)
        {
            if (m_CharacterController.velocity.sqrMagnitude > 0 && (m_Input.x != 0 || m_Input.y != 0))
            {
                m_StepCycle += (m_CharacterController.velocity.magnitude + (speed*(m_IsWalking ? 1f : m_RunstepLenghten)))*
                             Time.fixedDeltaTime;
            }

            if (!(m_StepCycle > m_NextStep))
            {
                return;
            }

            m_NextStep = m_StepCycle + m_StepInterval;

            PlayFootStepAudio();
        }


        private void PlayFootStepAudio()
        {
            //if (!m_CharacterController.isGrounded)
            //{
            //    return;
            //}
            // pick & play a random footstep sound from the array,
            // excluding sound at index 0
            int n = Random.Range(1, m_FootstepSounds.Length);
            m_AudioSource.clip = m_FootstepSounds[n];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            // move picked sound to index 0 so it's not picked next time
            m_FootstepSounds[n] = m_FootstepSounds[0];
            m_FootstepSounds[0] = m_AudioSource.clip;
        }


        private void UpdateCameraPosition(float speed)
        {
            Vector3 newCameraPosition;
            if (!m_UseHeadBob)
            {
                return;
            }
            if (m_CharacterController.velocity.magnitude > 0 && m_CharacterController.isGrounded)
            {
                m_Camera.transform.localPosition =
                    m_HeadBob.DoHeadBob(m_CharacterController.velocity.magnitude +
                                      (speed*(m_IsWalking ? 1f : m_RunstepLenghten)));
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_Camera.transform.localPosition.y - m_JumpBob.Offset();
            }
            else
            {
                newCameraPosition = m_Camera.transform.localPosition;
                newCameraPosition.y = m_OriginalCameraPosition.y - m_JumpBob.Offset();
            }
            m_Camera.transform.localPosition = newCameraPosition;
        }


        private void GetInput(out float speed)
        {
            // Read input
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            bool waswalking = m_IsWalking;

#if !MOBILE_INPUT
            // On standalone builds, walk/run speed is modified by a key press.
            // keep track of whether or not the character is walking or running
            m_IsWalking = !Input.GetKey(KeyCode.LeftShift);
#endif
            //Change walk and run speed depending on the water level
            float TempWaterLevelToPlayer = WaterLevel.m_WaterLevel - transform.position.y + 0.25f;
            float TempWalkingSpeed = m_WalkSpeed - (WalkingSpeedChange * TempWaterLevelToPlayer);
            float TempRunningSpeed = m_RunSpeed - (RunningSpeedChange * TempWaterLevelToPlayer);

            // set the desired speed to be walking or running
            //speed = m_IsWalking ? m_WalkSpeed : m_RunSpeed; // Originally

            speed = m_IsWalking ? TempWalkingSpeed : TempRunningSpeed;

            //Debug.Log("WalkingSpeed: " + TempWalkingSpeed);

            m_Input = new Vector2(horizontal, vertical);

            // normalize input if it exceeds 1 in combined length:
            if (m_Input.sqrMagnitude > 1)
            {
                m_Input.Normalize();
            }

            // handle speed change to give an fov kick
            // only if the player is going to a run, is running and the fovkick is to be used
            if (m_IsWalking != waswalking && m_UseFovKick && m_CharacterController.velocity.sqrMagnitude > 0)
            {
                StopAllCoroutines();
                StartCoroutine(!m_IsWalking ? m_FovKick.FOVKickUp() : m_FovKick.FOVKickDown());
            }
        }


        private void RotateView()
        {
            m_MouseLook.LookRotation (transform, m_Camera.transform);
        }


        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            Rigidbody body = hit.collider.attachedRigidbody;

            //dont move the rigidbody if the character is on top of it
            if (m_CollisionFlags == CollisionFlags.Below)
            {
                return;
            }

            if (body == null || body.isKinematic)
            {
                return;
            }
            body.AddForceAtPosition(m_CharacterController.velocity*0.1f, hit.point, ForceMode.Impulse);
        }

        //When on Ladder Trigger
        private void OnTriggerEnter(Collider ladder)
        {
            if (ladder.CompareTag("Ladder") && useLadder)
            {
                LadderObject = ladder.gameObject;
                m_onLadder = true;
            }
        }

        //Ladder Trigger Exit
        private void OnTriggerExit(Collider ladder)
        {
            if (ladder.tag == "Ladder")
            {
                m_onLadder = false;
                useLadder = true;
            }
        }

        //Ladder Movement
        private void LadderUpdate()
        {
            CamRot = m_Camera.transform.forward.y;
            
            if (m_onLadder)
            {
                Vector3 verticalMove;
                verticalMove = climbDirection.normalized;
                verticalMove *= Input.GetAxis("Vertical");
                verticalMove *= (CamRot > climbDownThreshold) ? 1 : -1;
                lateralMove = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
                lateralMove = transform.TransformDirection(lateralMove);
                ladderMovement = verticalMove + lateralMove;
                m_CharacterController.Move(ladderMovement * climbSpeed * Time.deltaTime);

                if (Input.GetAxis("Vertical") == 1 && !(m_AudioSource.isPlaying) && Time.time >= playTime)
                {
                    PlayLadderSound();
                }
            }
            else
            {
                useLadder = false;
                m_onLadder = false;
                LadderObject = null;
            }
        }

        //Ladder Footsteps
        void PlayLadderSound()
        {
            Debug.Log("PLAYING SOUND");
            int r = Random.Range(0, m_LadderSounds.Length);
            m_AudioSource.clip = m_LadderSounds[r];
            m_AudioSource.PlayOneShot(m_AudioSource.clip);
            playTime = Time.time + climbRate;
        }
    }
}
