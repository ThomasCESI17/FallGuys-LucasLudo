using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using static UnityEditor.Experimental.GraphView.GraphView;

public class FinishLine : MonoBehaviour
{
    public TextMeshProUGUI resultText;
    public TextMeshProUGUI timerText;
    private List<string> results = new List<string>();
    private List<float> playerTimers = new List<float>();
    private bool raceEnded = false;
    private float raceEndTime = 0f;
    public bool firstPlayerCrossed = true;
    public GameObject[] players; // Tableau contenant les objets des joueurs


    private void Start()
    {
        resultText.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!raceEnded && other.CompareTag("Player"))
        {
            if (firstPlayerCrossed)
            {
                results.Add("1st: " + other.name);
                firstPlayerCrossed = false;
                raceEndTime = Time.time + 10f;
                ShowTimer();
            }
            else
            {
                results.Add("2nd: " + other.name);
            }

            other.gameObject.SetActive(false);
        }
    }

    private void ShowTimer()
    {
        StartCoroutine(Countdown());
    }

    private IEnumerator Countdown()
    {
        timerText.gameObject.SetActive(true);

        float remainingTime = raceEndTime - Time.time;

        while (remainingTime > 0)
        {
            timerText.text = "Temps restant : " + remainingTime.ToString("F1");
            yield return null;
            remainingTime = raceEndTime - Time.time;
        }

        timerText.gameObject.SetActive(false);
        RaceEnder();
    }

    private void RaceEnder()
    {
        raceEnded = true;
        resultText.gameObject.SetActive(true);

        results.Sort();

        string resultsText = "Résultats de la course :\n";
        foreach (string result in results)
        {
            resultsText += result + "\n";
        }

        resultText.text = resultsText;

        foreach (GameObject player in players)
        {
            player.SetActive(false);
        }
    }
}
