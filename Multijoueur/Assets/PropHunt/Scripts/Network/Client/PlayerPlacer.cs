using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
namespace PropHunt.Gameplay
{
    public class PlayerPlacer : NetworkBehaviour
    {
        /*public override void OnNetworkSpawn()
        {
            Debug.Log(NetworkManager.Singleton.LocalClient.PlayerObject);
            // Place the player on the position 0,0,0 if the LocalClient is not null
            NetworkManager.Singleton.LocalClient.PlayerObject.transform.position = new Vector3(0, 1.5f, 0);
        }*/
    }

}