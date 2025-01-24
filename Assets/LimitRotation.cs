using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GGJ
{
    public class LimitRotation : MonoBehaviour
    {
        public float minX = -45f; // Minimum X rotation in degrees
        public float maxX = 45f;  // Maximum X rotation in degrees

        void Update()
        {
            // Get the current rotation
            Vector3 currentRotation = transform.localEulerAngles;
            print("_________________" + currentRotation);
            // Normalize the rotation to handle values over 180 degrees
            if (currentRotation.x > 180)
            {
                currentRotation.x -= 360;
            }

            // Clamp the rotation on the X-axis
            currentRotation.x = Mathf.Clamp(currentRotation.x, minX, maxX);

            // Apply the clamped rotation back
            transform.localEulerAngles = currentRotation;
        }
    }
}
