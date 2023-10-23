using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumperBehavior: MonoBehaviour
{
    public float bumpForce = 10.0f; // Adjust the bump force as needed

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the Rigidbody of the player
            Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();

            if (playerRigidbody != null)
            {
                // Apply a bump force to the player in the opposite direction of the collision
                Vector3 bumpDirection = (collision.contacts[0].point - transform.position).normalized;
                playerRigidbody.AddForce(bumpDirection * bumpForce, ForceMode.Impulse);
            }
        }
    }
}

