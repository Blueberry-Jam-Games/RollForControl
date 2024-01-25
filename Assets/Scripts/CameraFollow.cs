using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player character's transform
    public Vector3 offset = new Vector3(2f, 0f, 0f); // Adjust this to set the distance between the camera and the character
    public float smoothSpeed = 0.125f; // Adjust this to control the smoothness of the camera follow

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x, transform.position.y, transform.position.z) + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.position = new Vector3(smoothedPosition.x, transform.position.y, transform.position.z);
            //transform.LookAt(target); // Make the camera look at the player
        }
    }
}
