using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;


public class AlignPlayersOnStartLine : NetworkBehaviour
{
    public Transform startLine; // La position de la ligne de départ
    public float spacing = 2.0f; // Espacement entre les joueurs
    private bool IsOnLignSpawned = false;

    private GameObject _host;
    private GameObject _client;

    private void Start()
    {
        IsOnLignSpawned = false;
    }

    public void AlignPlayers()
    {
        if (PartyManager.GetPartyState() && !IsOnLignSpawned)
        {
            AlignPlayersOnStartLineServerRpc();
        }
    }

    [ServerRpc]
    public void AlignPlayersOnStartLineServerRpc()
    {
        // Recherchez tous les joueurs dans la scène
        List<NetworkClient> players = PartyManager.GetListOfPlayer();

        List<ulong> listClientId = new List<ulong>();

        // Placez chaque joueur le long de la ligne de départ
        int numPlayers = players.Count;

        // Calculez la longueur totale de la ligne
        float lineLength = spacing * (numPlayers - 1);

        // Calculez la position de départ au milieu de la ligne
        Vector3 startPlayerPosition = startLine.position - startLine.forward * (lineLength / 2.0f);

        startPlayerPosition = new Vector3(startPlayerPosition.x, 3, startPlayerPosition.z);

        foreach (NetworkClient player in players)
        {
            int i = 0;
            Vector3 playerPosition = startPlayerPosition + startLine.forward * (spacing * i);

            player.PlayerObject.gameObject.transform.position = playerPosition;
            player.PlayerObject.transform.position = playerPosition;
            i++;
        }
        IsOnLignSpawned = true;

    }
        
}
