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
            Vector3 desiredPosition = new Vector3(target.position.x, 0f, 0f) + offset;
            transform.position = new Vector3(desiredPosition.x, desiredPosition.y, desiredPosition.z);
            transform.rotation = Quaternion.Euler(30f, 0f, 0f);
            //transform.LookAt(target); // Make the camera look at the player
        }
    }
}
