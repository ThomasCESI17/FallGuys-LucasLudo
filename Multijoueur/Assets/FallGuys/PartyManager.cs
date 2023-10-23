using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;

public class PartyManager : NetworkBehaviour
{
    private static bool IsPartyStart { get; set; }
    private static List<NetworkClient> players;

    private void Start()
    {
        IsPartyStart = false;
    }

    private void Update()
    {
        if (!IsPartyStart && IsServer)
        {
            GetConnectedPlayers();
        }
    }

    private void GetConnectedPlayers()
    {
        var networkManager = NetworkManager.Singleton;

        // Vérifiez si le NetworkManager est valide
        if (networkManager != null)
        {
            players = networkManager.ConnectedClientsList.ToList();
        }
    }

    public static List<NetworkClient> GetListOfPlayer()
    {
        return players;
    }

    public static bool GetPartyState()
    {
        return IsPartyStart;
    }

    public static void LaunchParty()
    {
        foreach (NetworkClient player in players)
        {
            player.PlayerObject.GetComponent<PlayerManager>().ActivateCollider();
        }
        IsPartyStart = true;
    }
}
