using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TeleportPlayers : MonoBehaviour
{
    public Transform platform; // La plateforme sur laquelle vous souhaitez t�l�porter les joueurs
    public float teleportRadius = 5.0f; // Rayon autour de la plateforme o� les joueurs peuvent �tre t�l�port�s
    public List<NetworkClient> players; // Liste des joueurs que vous souhaitez t�l�porter
    private PlayerListManager playerListManager;

    public void StartTeleportation()
    {
        //players = playerListManager.players;
        // V�rifiez s'il y a suffisamment d'espace autour de la plateforme pour t�l�porter les joueurs
        if (players.Count > 0)
        {
            TeleportPlayersToPlatform();
        }
        else
        {
            Debug.LogWarning("Aucun joueur � t�l�porter.");
        }
    }

    void TeleportPlayersToPlatform()
    {
        Vector3 platformPosition = platform.position;

        foreach (NetworkClient player in players)
        {
            // D�terminez une position de t�l�portation al�atoire autour de la plateforme
            Vector3 teleportPosition = platformPosition + Random.insideUnitSphere * teleportRadius;

            // Assurez-vous que la nouvelle position est au-dessus du sol (ajustez cela en fonction de votre jeu)
            teleportPosition.y = 0.5f;

            // V�rifiez que la position de t�l�portation ne chevauche pas d'autres joueurs
            while (IsOverlapping(teleportPosition))
            {
                teleportPosition = platformPosition + Random.insideUnitSphere * teleportRadius;
                teleportPosition.y = 0.5f;
            }

            // T�l�portez le joueur � la nouvelle position
            player.PlayerObject.transform.position = teleportPosition;
        }
    }

    bool IsOverlapping(Vector3 position)
    {
        // V�rifiez si la nouvelle position chevauche un joueur existant
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