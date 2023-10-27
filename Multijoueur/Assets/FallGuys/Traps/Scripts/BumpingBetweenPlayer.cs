using UnityEngine;

public class BumpingBetweenPlayer : MonoBehaviour
{
    public float bumpForce = 10.0f; // Adjust the bump force as needed

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the CharacterController component of the player
            CharacterController characterController = collision.gameObject.GetComponent<CharacterController>();

            if (characterController != null)
            {
                // Calculate the bump direction
                Vector3 bumpDirection = (collision.contacts[0].point - transform.position).normalized;

                // Apply the bump force to the player using the CharacterController
                characterController.Move(bumpDirection * bumpForce * Time.deltaTime);
            }
        }
    }
}