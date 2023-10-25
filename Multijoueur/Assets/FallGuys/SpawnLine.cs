using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;


public class AlignPlayersOnStartLine : NetworkBehaviour
{
    private bool IsOnLignSpawned = false;

    public float timeRemaining = 10;
    public TextMeshProUGUI timerText;
    private List<SpawnLinePosition> spawnPositions = new List<SpawnLinePosition>();
    List<PlayerManager> playerManagers = new List<PlayerManager>();


    private void Start()
    {     
        spawnPositions = GetComponentsInChildren<SpawnLinePosition>().ToList();
        IsOnLignSpawned = false;
    }

    private void Update()
    {
        if (!IsOnLignSpawned)
        {
            StartCoroutine(AlignPlayers());
        }
    }
    
    public IEnumerator AlignPlayers()
    {
        if (PartyManager.GetPartyState() && !IsOnLignSpawned)
        {
            List<NetworkClient> networkClients = PartyManager.GetListOfPlayer();
            Debug.Log(networkClients.Count);
            foreach (NetworkClient p in networkClients)
            {
                playerManagers.Add(p.PlayerObject.GetComponent<PlayerManager>());
            }
            TeleportPlayersToLignClientRPC();
            playerManagers.Clear();
            IsOnLignSpawned = true;
            yield return new WaitForSeconds(1); // Optional delay before starting countdown.
            StartCoroutine(Countdown());
        }
    }
    /*
    [ClientRpc]
    public void AlignPlayersOnLineClientRpc()
    {

    }*/

    private IEnumerator Countdown()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        timerText.gameObject.SetActive(true);

        float remainingTime = timeRemaining;

        foreach (GameObject player in players)
        {
            player.SetActive(false);
        }

        while (remainingTime > 0)
        {
            timerText.text = "Temps restant : " + remainingTime.ToString("F1");
            yield return null;
            remainingTime = timeRemaining - Time.time;
        }
        

        timerText.gameObject.SetActive(false);
        foreach (GameObject player in players)
        {
            player.SetActive(true);
        }

    }

    [ClientRpc]
    public void TeleportPlayersToLignClientRPC()
    {
        Debug.Log(playerManagers.Count);
        for (int i = 0; i < playerManagers.Count; i++)
        {
            playerManagers[i].SpawnPoint(spawnPositions[i].transform.position);
        }
    }


    private void ShowTimer()
    {
        StartCoroutine(Countdown());
    }
}
