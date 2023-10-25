using UnityEngine;
using Unity.Netcode;

public class MovingPlatformController : NetworkBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float speed = 2.0f;

    private bool movingToA = true;

    void Update()
    {
        // Determine the target point based on the current direction
        Transform target = movingToA ? pointA : pointB;

        // Move the platform towards the target
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        // Check if the platform has reached the target
        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // Switch the direction
            movingToA = !movingToA;
        }
    }
}