using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TeleportPlayers : MonoBehaviour
{
    public Transform platform; // La plateforme sur laquelle vous souhaitez téléporter les joueurs
    public float teleportRadius = 5.0f; // Rayon autour de la plateforme où les joueurs peuvent être téléportés
    public List<NetworkClient> players; // Liste des joueurs que vous souhaitez téléporter
    private PlayerListManager playerListManager;

    public void StartTeleportation()
    {
        //players = playerListManager.players;
        // Vérifiez s'il y a suffisamment d'espace autour de la plateforme pour téléporter les joueurs
        if (players.Count > 0)
        {
            TeleportPlayersToPlatform();
        }
        else
        {
            Debug.LogWarning("Aucun joueur à téléporter.");
        }
    }

    void TeleportPlayersToPlatform()
    {
        Vector3 platformPosition = platform.position;

        foreach (NetworkClient player in players)
        {
            // Déterminez une position de téléportation aléatoire autour de la plateforme
            Vector3 teleportPosition = platformPosition + Random.insideUnitSphere * teleportRadius;

            // Assurez-vous que la nouvelle position est au-dessus du sol (ajustez cela en fonction de votre jeu)
            teleportPosition.y = 0.5f;

            // Vérifiez que la position de téléportation ne chevauche pas d'autres joueurs
            while (IsOverlapping(teleportPosition))
            {
                teleportPosition = platformPosition + Random.insideUnitSphere * teleportRadius;
                teleportPosition.y = 0.5f;
            }

            // Téléportez le joueur à la nouvelle position
            player.PlayerObject.transform.position = teleportPosition;
        }
    }

    bool IsOverlapping(Vector3 position)
    {
        // Vérifiez si la nouvelle position chevauche un joueur existant
        foreach (NetworkClient player in players)
        {
            if (Vector3.Distance(player.PlayerObject.transform.position, position) < 1.0f)
            {
                return true;
            }
        }
        return false;
    }
}