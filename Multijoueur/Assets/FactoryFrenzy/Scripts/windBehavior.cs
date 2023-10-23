using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class windBehavior : MonoBehaviour
{
    public float windSpeed = 0.25f;

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            ApplyWindForceToPlayer(other.gameObject);
        }
    }

    private void ApplyWindForceToPlayer(GameObject playerObject)
    {
        // Get the Rigidbody of the player
        Rigidbody playerRigidbody = playerObject.GetComponent<Rigidbody>();

        if (playerRigidbody != null)
        {
            // Calculate the wind force direction based on the wind zone's orientation
            Vector3 windDirection = transform.forward;

            // Apply a force to the player in the wind direction
            playerRigidbody.AddForce(-windDirection * windSpeed, ForceMode.Impulse);

        }
    }
}
