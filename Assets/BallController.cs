using System.Collections;
using UnityEngine;
using Fusion;

namespace GGJ
{
    public class BallController : NetworkBehaviour
    {
        [Header("Movement Settings")]
        public float speed = 10f;
        public float dashMultiplier = 1.5f;
        public float acceleration = 5f;
        public float deceleration = 5f;
        public float jumpForce = 5f;

        [Header("Rotation Settings")]
        public float rotationSpeed = 100f;
        
        [Header("Wall Sliding Settings")]
        public float wallSlideGravity = -4.905f;
        public float wallSlideSpeed = 2f;

        private CharacterController controller;
        private Vector3 currentVelocity = Vector3.zero;
        private Transform _cameraTransform;

        [Networked] private Vector3 MoveDirection { get; set; }
        [Networked] private float VerticalVelocity { get; set; }
        [Networked] private bool IsWallSliding { get; set; }
        [Networked] private bool IsTouchingWall { get; set; }
        [Networked] private bool CanStick { get; set; } = true;

        [Header("Animation")]
        [SerializeField] private Animator playerAnim;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }
        
        public override void Spawned()
        {
            if (HasInputAuthority)
            {
                AssignSharedCamera();
            }
        }

        private void AssignSharedCamera()
        {
            // Find and assign the main camera
            if (_cameraTransform == null)
            {
                Camera mainCamera = Camera.main;

                if (mainCamera != null)
                {
                    _cameraTransform = mainCamera.transform;
                }
                else
                {
                    Debug.LogError("No Main Camera found in the scene. Please add a Main Camera and set its tag to 'MainCamera'.");
                    return;
                }
            }

            // Adjust camera position and target
            _cameraTransform.position = transform.position + new Vector3(0, 2, -3); // Adjust offset as needed
            _cameraTransform.LookAt(transform.position + Vector3.up * 2); // Adjust look position
        }

        public override void FixedUpdateNetwork()
        {
            if (!HasStateAuthority) return;

            // Camera Handling for Input Authority
            if (HasInputAuthority && _cameraTransform != null)
            {
                _cameraTransform.position = transform.position + new Vector3(0, 2, -3); // Adjust offset as needed
                _cameraTransform.LookAt(transform);
            }
            
            // Check if grounded
            bool isGrounded = controller.isGrounded;
            if (isGrounded && VerticalVelocity < 0)
                VerticalVelocity = -2f;

            HandleMovement();
            HandleJump(isGrounded);
            HandleRotation();

            if (IsWallSliding)
                VerticalVelocity = wallSlideGravity;

            Vector3 updatedDirection = MoveDirection;
            updatedDirection.y = VerticalVelocity;
            MoveDirection = updatedDirection;
            controller.Move(MoveDirection * Runner.DeltaTime);

            if (Input.GetKeyDown(KeyCode.E))
            {
                RPC_PlayShootAnimation();
            }
        }

        private void HandleMovement()
        {
            if (!HasInputAuthority) return;

            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");
            Vector3 forward = _cameraTransform.forward;
            Vector3 right = _cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;
            forward.Normalize();
            right.Normalize();

            Vector3 targetDirection = (forward * moveVertical + right * moveHorizontal).normalized * speed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                targetDirection *= dashMultiplier;
                ChangeIdleAnimationSpeed(10);
            }

            Vector3 newMoveDirection = Vector3.SmoothDamp(MoveDirection, targetDirection, ref currentVelocity, 
                MoveDirection.magnitude > 0 ? 1f / acceleration : 1f / deceleration);
            MoveDirection = newMoveDirection;

            ChangeIdleAnimationSpeed(MoveDirection.magnitude > 0 ? 5f : 1f);
        }

        private void HandleJump(bool isGrounded)
        {
            if (!HasInputAuthority) return;

            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                VerticalVelocity = jumpForce;
                IsWallSliding = false;
                playerAnim.SetTrigger("Jump");
            }

            if (!isGrounded && IsTouchingWall && Input.GetKeyDown(KeyCode.Space))
            {
                StartCoroutine(DisableStickBehavior());
                IsWallSliding = false;
                VerticalVelocity = jumpForce / 1.5f;
            }

            if (IsTouchingWall && !isGrounded && !IsWallSliding && CanStick)
                IsWallSliding = true;

            VerticalVelocity += Physics.gravity.y * Runner.DeltaTime;
        }

        private void HandleRotation()
        {
            if (!HasInputAuthority) return;

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            transform.Rotate(Vector3.up, mouseX * rotationSpeed * Runner.DeltaTime);
        }

        private IEnumerator DisableStickBehavior()
        {
            CanStick = false;
            IsWallSliding = false;
            IsTouchingWall = false;
            yield return new WaitForSeconds(1);
            CanStick = true;
        }

        private void ChangeIdleAnimationSpeed(float speed)
        {
            playerAnim.SetFloat("Idle_Speed", speed);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!HasStateAuthority) return;

            if (other.CompareTag("Wall") && CanStick)
            {
                IsTouchingWall = true;
                if (!controller.isGrounded)
                    IsWallSliding = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!HasStateAuthority) return;

            if (other.CompareTag("Wall"))
            {
                IsTouchingWall = false;
                IsWallSliding = false;
            }
        }

        [Rpc(RpcSources.InputAuthority, RpcTargets.All)]
        private void RPC_PlayShootAnimation()
        {
            playerAnim.SetTrigger("Shoot");
        }
    }
}
