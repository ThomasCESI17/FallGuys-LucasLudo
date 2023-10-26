using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using Unity.VisualScripting;
using UnityEngine;

/*public class FinishLine : MonoBehaviour
{
    private Collider m_Line;
    private List<FinishPoint> arrivedPositions = new List<FinishPoint>();
    private int placeCount = 0;


    // Start is called before the first frame update
    void Start()
    {
        m_Line = GetComponent<Collider>();
        arrivedPositions = GetComponentsInChildren<FinishPoint>().ToList();
    }

    private void OnTriggerExit(Collider other)
    {
        GameObject player = other.gameObject.GetComponentInParent<PlayerManager>().gameObject;
        player.transform.position = arrivedPositions[placeCount].transform.position;
        player.GetComponent<MovementController>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }*/


public class FinishLine : NetworkBehaviour
{
    private List<PlayerManager> playersArrived;
    private bool raceEnded = false;
    private bool firstPlayerCrossed = false;
    private Coroutine countdownCoroutine;
    private string ranking;

    private List<FinishPoint> arrivedPositions = new List<FinishPoint>();


    void Start()
    {
        playersArrived = new List<PlayerManager>();
        arrivedPositions = GetComponentsInChildren<FinishPoint>().ToList();
    }

    private void Update()
    {
    }

    public void RpcShowTimer()
    {
        if (countdownCoroutine != null)
        {
            StopCoroutine(countdownCoroutine);
        }

        StartCoroutine(CountdownCoroutine());
    }

    private IEnumerator CountdownCoroutine()
    {
        yield return new WaitForSeconds(10f); // Attendre 10 secondes après la fin de la course
        RpcRaceEnderClientRPC();
        // DisplayResultsClientRpc();

    }

    private void RefreshRanking()
    {
        // Créer un classement distinct pour chaque joueur
        ranking = "Résultats de la course :\n";

        for (int i=0 ; i < playersArrived.Count ; i++)
        { 
            int number = i + 1 ;
            ranking = ranking + number.ToString() + ". " + playersArrived[i].GetPlayerName() +"\n";
        }
    }

    [ClientRpc]
    public void RpcRaceEnderClientRPC()
    {
        raceEnded = true;
        // Teleportation des autres joueurs
        List<NetworkClient> networkClients = PartyManager.GetListOfPlayer();

        foreach (NetworkClient player in networkClients)
        {
            if (!ranking.Contains(player.PlayerObject.GetComponent<PlayerManager>().GetPlayerName()))
            {
                PlayerManager playerManager = player.PlayerObject.GetComponent<PlayerManager>();
                TeleportPlayersToLign(playerManager);
                ranking = ranking + playerManager.GetPlayerName() + "\n";
                playersArrived.Add(playerManager);
            }
        }
        foreach (PlayerManager playerArrived in playersArrived)
        {
            playerArrived.UpdateRankingClientRPC(ranking);
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (!raceEnded && other.CompareTag("Player"))
        {
            PlayerManager playerObject = other.GetComponentInParent<PlayerManager>();

            CrossLignClientRPC(playerObject.GetPlayerName());

            
        }
    }

    [ClientRpc]
    public void CrossLignClientRPC(string playerFromCollider)
    {
        List<NetworkClient> networkClients = PartyManager.GetListOfPlayer();
        PlayerManager playerObject = new PlayerManager();


        foreach (NetworkClient player in networkClients)
        {
            if (player.PlayerObject.GetComponent<PlayerManager>().GetPlayerName() == playerFromCollider)
            {
                playerObject = player.PlayerObject.GetComponent<PlayerManager>();
            }
        }
        Debug.Log(playerObject);

        /*foreach (PlayerManager player in playerList)
        {
            if (player.GetPlayerName() == playerFromCollider)
            {
                playerObject = player;
            }
        }*/
        TeleportPlayersToLign(playerObject);
        if (!firstPlayerCrossed)
        {
            firstPlayerCrossed = true;
            RpcShowTimer();
        }
        playersArrived.Add(playerObject);
        Debug.Log(playersArrived.Count);

        RefreshRanking(/*string playerFromCollider*/);
        foreach (PlayerManager playerArrived in playersArrived)
        {
            playerArrived.UpdateRankingClientRPC(ranking);
        }
        
    }

    private void TeleportPlayersToLign(PlayerManager player)
    {
        player.SpawnPointClientRPC(arrivedPositions[playersArrived.Count].transform.position, false);
        player.FreezeAnimationClientRPC();
    }
}

