using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class FinishLine : MonoBehaviour
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
        
    }

 /*   
public class FinishLine : NetworkBehaviour
{
    public TextMeshProUGUI resultText;
    private List<string> results = new List<string>();
    private bool raceEnded = false;
    public GameObject[] players;
    private bool firstPlayerCrossed = true;
    private Coroutine countdownCoroutine;

    void Start()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

    }

    [ClientRpc]
    public void RpcShowTimerClientRpc()
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
        RpcRaceEnderClientRpc();
        DisplayResultsClientRpc();

    }

    [ClientRpc]
    private void DisplayResultsClientRpc()
    {
        // Trier les résultats
        results.Sort();
        foreach (GameObject player in players)
        {
            player.SetActive(false);
        }
        // Créer un classement distinct pour chaque joueur
        string resultsText = "Résultats de la course :\n";
        int rank = 1;


        foreach (string result in results)
        {
            if (result.Contains("1st:") || result.Contains("2nd:"))
            {
                resultsText += result.Replace("1st:", rank + "st:").Replace("2nd:", rank + "nd:") + "\n";
                rank++;
            }
            else
            {
                resultsText += result + "\n";
            }
        }

        resultText.text = resultsText;
    }

    [ClientRpc]
    public void RpcRaceEnderClientRpc()
    {
        raceEnded = true;
        resultText.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!raceEnded && other.CompareTag("Player"))
        {
            GameObject playerObject = other.transform.parent.parent.parent.gameObject;
            if (firstPlayerCrossed)
            {
                results.Add("1st: " + playerObject.name);
                firstPlayerCrossed = false;
                RpcShowTimerClientRpc();
            }
            else
            {
                results.Add("2nd: " + playerObject.name);
            }

            playerObject.SetActive(false);
        }
    }
}*/
}
