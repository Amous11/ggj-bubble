using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
    public class WallStickController : MonoBehaviour
    {
       /* [Header("Wall Stick Settings")]
        public float wallStickGravityScale = 0.25f; // Gravity scale when sticking to the wall
        public string specialWallTag = "SpecialWall"; // Tag used to identify special walls

        private BallController ballController;
        private CharacterController characterController;
        private bool isSticking = false;
        private float originalGravity;

        public bool IsSticking => isSticking; // Expose sticking status

        private void Awake()
        {
            ballController = GetComponent<BallController>();
            characterController = GetComponent<CharacterController>();
            originalGravity = ballController.gravity; // Accessing gravity as a public field
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            // Check if the ball collides with the special wall
            if (hit.collider.CompareTag(specialWallTag) && !characterController.isGrounded)
            {
                if (!isSticking)
                {
                    isSticking = true; // Start sticking to the wall
                    ballController.gravity = originalGravity * wallStickGravityScale; // Reduce gravity
                }
            }
            else if (characterController.isGrounded)
            {
                if (isSticking)
                {
                    isSticking = false; // Stop sticking when grounded
                    ballController.gravity = originalGravity; // Restore original gravity
                }
            }
        }

        public void HandleWallSticking()
        {
            // Allow rotation while sticking to the wall
            HandleRotation();

            // Apply movement with reduced gravity
            ballController.ApplyMovement();

            // Prevent any horizontal movement while sticking to the wall
            ballController.SetMoveDirection(Vector3.zero);
        }

        private void HandleRotation()
        {
            // Rotate the player in place based on mouse movement
            float mouseX = Input.GetAxis("Mouse X");
            ballController.transform.Rotate(Vector3.up, mouseX * ballController.rotationSpeed * Time.deltaTime);
        }*/
    }
}
