using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
    public class BallController : MonoBehaviour
    {
        [Header("Movement Settings")]
        public float speed = 10f;
        public float dashMultiplier = 1.5f;
        public float acceleration = 5f;
        public float deceleration = 5f;
        public float jumpForce = 5f;

        [Header("Rotation Settings")]
        public float rotationSpeed = 100f;

        [Header("Camera Settings")]
        public Transform cameraTransform;

        [Header("Wall Sliding Settings")]
        public float wallSlideGravity = -4.905f; // Gravity when wall sliding
        public float wallSlideSpeed = 2f; // Speed of sliding down the wall

        private CharacterController controller;
        private Vector3 moveDirection = Vector3.zero;
        private Vector3 currentVelocity = Vector3.zero;
        private float gravity = -9.81f;
        private float verticalVelocity;
        private bool isGrounded;
        private bool isTouchingWall; // Check if touching a wall
        private bool isWallSliding; // Flag for wall sliding
        public bool canStick;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Update()
        {
            isGrounded = controller.isGrounded;

            if (isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = -2f;
            }

            HandleMovement();
            HandleJump();
            HandleRotation();

            if (isWallSliding)
            {
                // Apply wall sliding gravity
                verticalVelocity = wallSlideGravity;
            }

            controller.Move(moveDirection * Time.deltaTime);
        }
        IEnumerator DisableStickBehavior()
        {
            canStick = false;
            isWallSliding = false;
            isTouchingWall = false;
            yield return new WaitForSeconds(1);
            canStick = true;

        }
        private void HandleMovement()
        {
            float moveHorizontal = Input.GetAxisRaw("Horizontal");
            float moveVertical = Input.GetAxisRaw("Vertical");

            Vector3 forward = cameraTransform.forward;
            Vector3 right = cameraTransform.right;

            forward.y = 0f;
            right.y = 0f;

            forward.Normalize();
            right.Normalize();

            Vector3 targetDirection = (forward * moveVertical + right * moveHorizontal).normalized * speed;

            if (Input.GetKey(KeyCode.LeftShift))
            {
                targetDirection *= dashMultiplier;
            }

            moveDirection = Vector3.SmoothDamp(moveDirection, targetDirection, ref currentVelocity, moveDirection.magnitude > 0 ? 1f / acceleration : 1f / deceleration);
        }

        private void HandleJump()
        {
            if (isGrounded && Input.GetKeyDown(KeyCode.Space))
            {
                // Normal jump when grounded
                verticalVelocity = jumpForce;
                isWallSliding = false; // Ensure sliding is stopped when jumping from the ground
            }

            if (!isGrounded && isTouchingWall && Input.GetKeyDown(KeyCode.Space))
            {
                // Eliminate sticky behavior and perform jump
                StartCoroutine("DisableStickBehavior");
                isWallSliding = false; // Stop wall sliding when jumping off the wall

                // Apply vertical velocity like a normal jump (no wall influence)
                verticalVelocity = jumpForce/1.5f;

                // Reset horizontal movement to avoid sticking to the wall
                //moveDirection.x = 0f; // Reset horizontal movement
                //moveDirection.z = 0f;

                // Calculate direction opposite to the wall to jump away from it
                Vector3 wallDirection = transform.right; // If touching the left side of the wall, this will give the opposite direction
                if (isTouchingWall && !isGrounded)
                {
                    wallDirection = -transform.right; // Jump in the opposite direction of the wall
                }

                // Apply horizontal force to move the player away from the wall (in the opposite direction)
                //moveDirection += wallDirection * speed;

                isWallSliding = false; // Stop wall sliding when jumping off the wall
            }

            if (isTouchingWall && !isGrounded && !isWallSliding && canStick)
            {
                // Start wall sliding when player is not grounded and touches the wall
                isWallSliding = true;
            }

            verticalVelocity += gravity * Time.deltaTime;
            moveDirection.y = verticalVelocity;
        }



        private void HandleRotation()
        {
            float mouseX = Input.GetAxis("Mouse X");
            transform.Rotate(Vector3.up, mouseX * rotationSpeed * Time.deltaTime);
        }

        public void ApplyVerticalVelocity(float velocity)
        {
            verticalVelocity = velocity;
        }

        public Vector3 GetMoveDirection()
        {
            return moveDirection;
        }

        public void SetMoveDirection(Vector3 direction)
        {
            moveDirection = direction;
        }

        // Detect when player enters a wall trigger
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Wall") && canStick)
            {
                isTouchingWall = true;
                // Start wall sliding if the player is not grounded
                if (!isGrounded)
                {
                    isWallSliding = true;
                }
            }
        }

        // Detect when player exits a wall trigger
        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Wall"))
            {
                isTouchingWall = false;
                isWallSliding = false; // Stop wall sliding when exiting the wall
            }
        }
    }
}
