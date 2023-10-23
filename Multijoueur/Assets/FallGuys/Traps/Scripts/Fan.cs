using UnityEngine;

public class Fan : MonoBehaviour
{
    public float windForce = 10.0f; // Adjust the wind force as needed
    public float range = 5f;
    //public ParticleSystem windParticles; // Assign the Particle System for wind visualization

    private void Start()
    {
        // Assurez-vous que le GameObject possède un composant Collider attaché
        Collider collider = GetComponent<Collider>();

        if (collider != null)
        {
            // Obtenez la taille actuelle du collider
            Vector3 tailleActuelle = collider.transform.localScale;

            // Modifiez uniquement la taille de l'axe Z
            tailleActuelle.z = range;

            // Appliquez la nouvelle taille au collider
            collider.transform.localScale = tailleActuelle;
        }
        else
        {
            Debug.LogError("Le GameObject ne possède pas de composant Collider attaché.");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ApplyWindForce(other.gameObject);
        }
    }

    private void ApplyWindForce(GameObject playerObject)
    {
        // Get the Rigidbody of the player
        Rigidbody playerRigidbody = playerObject.GetComponentInParent<Rigidbody>();

        if (playerRigidbody != null)
        {
            // Calculate the wind force direction based on the wind zone's orientation
            Vector3 windDirection = transform.forward;

            // Apply a force to the player in the wind direction
            playerRigidbody.AddForce(windDirection * windForce, ForceMode.Impulse);

            // Activate the wind particles
            /*if (windParticles != null)
            {
                windParticles.Play();
            }*/
        }
    }
}