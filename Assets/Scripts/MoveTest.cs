using UnityEngine;

namespace Assets.Scripts
{
    /// <summary>
    /// Test Player script Will be rewritten and refactored after test purpose is completed
    /// 
    /// Bug: Dashing doesn't work
    /// 
    /// </summary>
    [AddComponentMenu("Mythirial Test/Move Test")]
    public class MoveTest : MonoBehaviour
    {
        [SerializeField]
        private float moveSpeed = 7.5f;
        [SerializeField]
        private float rotationSpeed = 15f;
        [SerializeField]
        private float jumpHeight = 6f;
        [SerializeField]
        private float groundDistance = 0.5f;

        [SerializeField]
        private float fallMultiplier = 2.5f;
        [Range(0, 1)]
        public float airControlPercent;

        public Vector3 desiredMoveDirection;
        public bool blockRotationPlayer;
        public LayerMask Ground;

        //private Animator animator;
        private Camera _camera;
        private Rigidbody _body;
        private Vector3 _input = Vector3.zero;
        private bool _isGrounded = true;
        private Transform _groundChecker;

        /// <summary>
        /// Called in when this enters the scene
        /// </summary>
        private void Awake()
        {
            //animator = GetComponentInChildren<Animator>();
            _camera = Camera.main;
            _body = GetComponent<Rigidbody>();
            _groundChecker = transform;
        }

        private void Update()
        {
            MOVE();

            if (Input.GetKey(KeyCode.Escape))
                Cursor.lockState = CursorLockMode.None;
        }

        /// <summary>
        /// update meant for Physics related coding
        /// </summary>
        private void FixedUpdate()
        {
            MOVEPhysics();
        }

        /// <summary>
        /// MOVE (Subject to change)
        /// Basis of the movement system
        /// </summary>
        private void MOVE()
        {
            //animator.SetFloat("Speed", movement.magnitude);

            _isGrounded = Physics.CheckSphere(_groundChecker.position, groundDistance, Ground, QueryTriggerInteraction.Ignore);

            _input = Vector3.zero;
            _input = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            CamLock();

            if (_input != Vector3.zero)
                transform.forward = _input;

            if (Input.GetButtonDown("Jump") && _isGrounded)
            {
                _body.AddForce(Vector3.up * Mathf.Sqrt(jumpHeight * -2f * Physics.gravity.y), ForceMode.VelocityChange);
            }


            if (desiredMoveDirection.magnitude > 0)
            {
                if (blockRotationPlayer == false)
                {
                    transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(desiredMoveDirection), rotationSpeed); //Points in direction relative to the camera
                }
            }
        }

        /// <summary>
        /// MOVE physics function
        /// </summary>
        void MOVEPhysics()
        {
            CamLock();
            _body.MovePosition(_body.position + desiredMoveDirection * moveSpeed * Time.fixedDeltaTime); //Moves us in the desiredWay
            _body.velocity += Vector3.up * Physics.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }


        /// <summary>
        /// Calculate player rotation to be relative to camera rotation
        /// </summary>
        void CamLock()
        {
            var camera = Camera.main;
            var forward = _camera.transform.up;
            var right = _camera.transform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            desiredMoveDirection = forward * _input.z + right * _input.x;
        }
    }
}