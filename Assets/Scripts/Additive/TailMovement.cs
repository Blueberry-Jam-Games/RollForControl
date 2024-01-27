using System.Collections;
using UnityEngine;

public class TailMovement : MonoBehaviour
{
    // Draw the gizmo
    private Ray lastRay;
    public float distance = 10.0f;
    public Vector3 targetPosition;
    private Rigidbody rb;
    public GameObject finalPosition;

    public bool pinned = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // left mouse button
        if (!pinned)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButton(0))
            {
                lastRay = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(lastRay, out RaycastHit hitInfo, maxDistance: 20.0f, layerMask: Physics.DefaultRaycastLayers, queryTriggerInteraction: QueryTriggerInteraction.Ignore))
                {
                    targetPosition = hitInfo.point;
                    // transform.position = targetPosition - new Vector3(0, 0.5f, 0);
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (!pinned)
        {
            rb.MovePosition(targetPosition);
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
        transform.position = finalPosition.transform.position;

        // Level Done, do something about it
        StartCoroutine(LevelDone());
    }

    private IEnumerator LevelDone()
    {
        yield return new WaitForSeconds(0.5f);
        FlowManager.Instance.PinTailComplete();
    }
}
