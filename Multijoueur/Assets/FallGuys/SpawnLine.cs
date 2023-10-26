using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;


public class AlignPlayersOnStartLine : NetworkBehaviour
{
    private bool IsOnLignSpawned = false;
    private bool IsRaceStart = false;
    private bool IsPlayerFreeze = false;

    public float timeRemaining = 10;
    private float countDown;
    public TextMeshProUGUI timerText;
    private List<SpawnLinePosition> spawnPositions = new List<SpawnLinePosition>();
    List<PlayerManager> playerManagers = new List<PlayerManager>();



    private void Start()
    {     
        spawnPositions = GetComponentsInChildren<SpawnLinePosition>().ToList();
        IsOnLignSpawned = false;
        IsRaceStart = false;
        countDown = timeRemaining;
    }

    private void Update()
    {
        if (!IsOnLignSpawned)
        {
            StartCoroutine(GameStart());
        } else {
            CountdownClientRPC();
        }
        
    }

    [ClientRpc]
    private void CountdownClientRPC()
    {
        if (!IsRaceStart)
        {
            if (!IsPlayerFreeze)
            {
                List<NetworkClient> networkClients = PartyManager.GetListOfPlayer();
                foreach (NetworkClient networkClient in networkClients)
                {
                    networkClient.PlayerObject.GetComponent<MovementController>().enabled = false;
                    networkClient.PlayerObject.transform.eulerAngles = new Vector3(0, 0, 0);
                }
                IsPlayerFreeze = true;
            }
            else
            {
                List<NetworkClient> networkClients = PartyManager.GetListOfPlayer();
                if (timeRemaining > 0f)
                {
                    timerText.text = timeRemaining.ToString("0");
                    timeRemaining = timeRemaining - Time.deltaTime;
                }
                else
                {
                    foreach (NetworkClient networkClient in networkClients)
                    {
                        networkClient.PlayerObject.GetComponent<PlayerManager>().UnFreezeAnimationClientRPC();
                    }
                    timerText.text = "";
                    IsPlayerFreeze = true;
                    IsPlayerFreeze = false;
                    IsRaceStart = true;
                }
            }
        }
    }
    
    public IEnumerator GameStart()
    {
        yield return StartCoroutine(AlignPlayers());
        // ShowTimerServerRPC();
    }

    public IEnumerator AlignPlayers()
    {
        if (PartyManager.GetPartyState() && !IsOnLignSpawned)
        {
            timerText.text = "Are you ready?";
            List<NetworkClient> networkClients = PartyManager.GetListOfPlayer();
            foreach (NetworkClient p in networkClients)
            {
                playerManagers.Add(p.PlayerObject.GetComponent<PlayerManager>());
            }
            TeleportPlayersToLign();
            playerManagers.Clear();
            
            yield return new WaitForSeconds(1); // Optional delay before starting countdown.

            IsOnLignSpawned = true;
        }
    }

    //[ServerRpc]
    public void TeleportPlayersToLign()
    {
        for (int i = 0; i < playerManagers.Count; i++)
        {
            playerManagers[i].SpawnPointClientRPC(spawnPositions[i].transform.position, true);
            playerManagers[i].FreezeAnimationClientRPC();
        }
    }
}
