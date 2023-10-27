using Unity.Netcode;
using UnityEngine;

public class BumpingBetweenPlayer : NetworkBehaviour
{
    public float bumpForce = 100.0f; // Adjust the bump force as needed

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the Rigidbody of the player
            Rigidbody playerRigidbody = collision.gameObject.GetComponentInParent<Rigidbody>();

            if (playerRigidbody != null)
            {
                // Apply a bump force to the player in the opposite direction of the collision
                Vector3 bumpDirection = (collision.contacts[0].point - transform.position).normalized;
                playerRigidbody.AddForce(bumpDirection * bumpForce, ForceMode.Impulse);
            }
        }
    }
}