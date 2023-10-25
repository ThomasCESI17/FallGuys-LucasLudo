using Unity.Netcode;
using UnityEngine;

public class Trampoline : NetworkBehaviour
{
    public float bounceForce = 300.0f; // Adjust the bounce force as needed

    private void OnCollisionEnter(Collision collision)
    {
        // Get the Rigidbody of the player
        Rigidbody playerRigidbody = collision.gameObject.GetComponent<Rigidbody>();
        if (playerRigidbody != null)
        {
            // Apply an upward force to the player to simulate bouncing
            playerRigidbody.AddForce(Vector3.up * bounceForce, ForceMode.Impulse);
        }
    }
}