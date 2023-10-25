using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.Experimental.GraphView.GraphView;

public class StartPlateform : MonoBehaviour
{
    public float timeRemaining = 10;
    public Vector3 spawnPosition;
    public GameObject[] players; // Tableau contenant les objets des joueurs
    public TextMeshProUGUI timerText;
    public List<Respawn> respawnControllers = new List<Respawn>();

    void Start()
    {
        Respawn[] respawnScripts = FindObjectsOfType<Respawn>();
        respawnControllers.AddRange(respawnScripts);
        spawnPosition = transform.position + Vector3.up * 1f;
        foreach (Respawn respawnController in respawnControllers)
        {
            // Mise à jour de la position de respawn pour chaque joueur
            respawnController.spawnPosition = gameObject.transform.position;
        }
        TeleportPlayersToPlatform(spawnPosition);
        ShowTimer();
    }

    private void ShowTimer()
    {
        StartCoroutine(Countdown());
    }

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
            remainingTime -= Time.deltaTime;
        }

        timerText.gameObject.SetActive(false);
        foreach (GameObject player in players)
        {
            player.SetActive(true);
        }
        
    }

    public static void TeleportPlayersToPlatform(Vector3 destination)
    {
        // Téléporte tous les joueurs vers la plateforme de destination
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach (GameObject player in players)
        {
            player.transform.position = destination;
        }
    }
}
