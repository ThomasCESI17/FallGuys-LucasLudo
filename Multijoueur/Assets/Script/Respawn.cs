using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public float treshold;
    public Vector3 spawnPosition;
    private Rigidbody rb; // R�f�rence au composant Rigidbody

    private void Start()
    {
        spawnPosition = transform.position;
        rb = GetComponent<Rigidbody>(); // R�cup�rer le composant Rigidbody
    }

    // FixedUpdate est g�n�ralement utilis� pour les op�rations physiques
    void FixedUpdate()
    {
        if (transform.position.y < treshold)
        {
            // R�initialiser la position
            transform.position = spawnPosition;

            // Arr�ter tout mouvement en r�initialisant la vitesse du Rigidbody � z�ro
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
