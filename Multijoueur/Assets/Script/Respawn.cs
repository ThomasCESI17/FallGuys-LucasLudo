using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public float treshold;
    public Vector3 spawnPosition;
    private Rigidbody rb; // Référence au composant Rigidbody

    private void Start()
    {
        spawnPosition = transform.position;
        rb = GetComponent<Rigidbody>(); // Récupérer le composant Rigidbody
    }

    // FixedUpdate est généralement utilisé pour les opérations physiques
    void FixedUpdate()
    {
        if (transform.position.y < treshold)
        {
            // Réinitialiser la position
            transform.position = spawnPosition;

            // Arrêter tout mouvement en réinitialisant la vitesse du Rigidbody à zéro
            if (rb != null)
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }
}
