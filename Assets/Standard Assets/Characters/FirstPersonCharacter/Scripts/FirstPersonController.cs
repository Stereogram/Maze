using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

namespace UnityStandardAssets.Characters.FirstPerson {
    [RequireComponent(typeof(CharacterController))]
    public class FirstPersonController : MonoBehaviour {
        [SerializeField] private float m_Speed;
        [SerializeField] private float m_JumpSpeed;
        [SerializeField] private float m_StickToGroundForce;
        [SerializeField] private float m_GravityMultiplier;
        [SerializeField] private MouseLook m_MouseLook;
        [SerializeField] private AudioSource _wallSource;
        [SerializeField] private AudioSource _footSource;

        private Camera m_Camera;
        private bool m_Jump;
        private Vector3 m_MoveDir = Vector3.zero;
        private CharacterController m_CharacterController;
        private CollisionFlags m_CollisionFlags;
        private bool m_PreviouslyGrounded;
        private bool m_Jumping;

        private void Start() {
            m_CharacterController = GetComponent<CharacterController>();
            m_Camera = Camera.main;
            m_Jumping = false;
            m_MouseLook.Init(transform, m_Camera.transform);
        }

        public void Reset() {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
            m_Camera.transform.rotation = Quaternion.identity;
            m_MouseLook.Init(transform, m_Camera.transform);
        }

        private void Update() {
            m_MouseLook.LookRotation(transform, m_Camera.transform);
            if (!m_Jump) {
                m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
            }
            if (!m_PreviouslyGrounded && m_CharacterController.isGrounded) {
                m_MoveDir.y = 0f;
                m_Jumping = false;
            }
            if (!m_CharacterController.isGrounded && !m_Jumping && m_PreviouslyGrounded) {
                m_MoveDir.y = 0f;
            }
            m_PreviouslyGrounded = m_CharacterController.isGrounded;
        }

        private void FixedUpdate() {
            Vector3 desiredMove = transform.forward * CrossPlatformInputManager.GetAxisRaw("Vertical") + transform.right * CrossPlatformInputManager.GetAxisRaw("Horizontal");
            RaycastHit hitInfo;
            Physics.SphereCast(transform.position, m_CharacterController.radius, Vector3.down, out hitInfo, m_CharacterController.height / 2f, Physics.AllLayers, QueryTriggerInteraction.Ignore);
            desiredMove = Vector3.ProjectOnPlane(desiredMove, hitInfo.normal).normalized;
            m_MoveDir.x = desiredMove.x * m_Speed;
            m_MoveDir.z = desiredMove.z * m_Speed;
            if (m_CharacterController.isGrounded) {
                m_MoveDir.y = -m_StickToGroundForce;
                if (m_Jump) {
                    m_MoveDir.y = m_JumpSpeed;
                    m_Jump = false;
                    m_Jumping = true;
                }
            } else {
                m_MoveDir += Physics.gravity * m_GravityMultiplier * Time.fixedDeltaTime;
            }
            if(desiredMove == Vector3.zero && _footSource.isPlaying) {
                _footSource.Pause();
            }
            else if(desiredMove != Vector3.zero && !_footSource.isPlaying)
            {
                _footSource.Play();
            }
            m_CollisionFlags = m_CharacterController.Move(m_MoveDir * Time.fixedDeltaTime);
            m_MouseLook.UpdateCursorLock();
        }

        public void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if(hit.collider.CompareTag("wall") && !_wallSource.isPlaying)
                _wallSource.Play();
        }

    }

    
}
