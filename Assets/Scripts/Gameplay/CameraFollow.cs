using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // Reference to the player character's transform
    public Vector3 offset = new Vector3(3.67f, 4f, -7.3f); // Adjust this to set the distance between the camera and the character
    public float smoothSpeed = 0.125f; // Adjust this to control the smoothness of the camera follow



    // Start is called before the first frame update
    void Start()
    {

    }

    void LateUpdate()
    {
        if (target != null)
        {
            Vector3 desiredPosition = new Vector3(target.position.x + offset.x, transform.position.y, transform.position.z);
            transform.position = desiredPosition;
            //transform.LookAt(target); // Make the camera look at the player
        }
    }
}
