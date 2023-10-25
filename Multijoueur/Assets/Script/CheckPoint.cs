/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private List<Respawn> respawnControllers = new List<Respawn>();

    private void Start()
    {
        // Trouvez tous les joueurs dans la sc�ne qui ont le script Respawn attach�
        Respawn[] respawnScripts = FindObjectsOfType<Respawn>();
        respawnControllers.AddRange(respawnScripts);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            foreach (Respawn respawnController in respawnControllers)
            {
                // Mise � jour de la position de respawn pour chaque joueur
                respawnController.spawnPosition = gameObject.transform.position;
            }

            // Vous pouvez d�truire le checkpoint si n�cessaire
            //Destroy(gameObject);
        }
    }
}
*/