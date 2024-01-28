using System.Collections;
using UnityEngine;

public class TailMovement : MonoBehaviour
{
    // Draw the gizmo
    private Ray lastRay;
    public float distance = 10.0f;
    public Vector3 targetPosition;
    public GameObject finalPosition;

    public bool pinned = false;
    public bool doMovement = false;

    public Vector3 offset;
    public Vector3 finalOffset;

    private void Update()
    {
        // left mouse button
        if (!pinned && doMovement)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(lastRay, out RaycastHit hitInfo, maxDistance: 20.0f, layerMask: Physics.DefaultRaycastLayers, queryTriggerInteraction: QueryTriggerInteraction.Ignore))
                {
                    targetPosition = hitInfo.point;
                    transform.position = targetPosition + offset;
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(lastRay);
        // Gizmos.DrawCube(targetPosition, Vector3.one);
    }

    private void OnTriggerEnter(Collider other)
    {
        pinned = true;
        transform.position = finalPosition.transform.position + finalOffset;
    }
}
