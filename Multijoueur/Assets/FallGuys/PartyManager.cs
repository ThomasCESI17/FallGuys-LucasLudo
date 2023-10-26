using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;

public class PartyManager : NetworkBehaviour
{
    private static bool IsPartyStart { get; set; }
    private static List<NetworkClient> players;

    private void Start()
    {
        IsPartyStart = false;
        players = new List<NetworkClient>();
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
        IsPartyStart = true;
    }
}
