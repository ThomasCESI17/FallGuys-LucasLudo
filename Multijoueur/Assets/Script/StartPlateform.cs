using UnityEngine;

public class StartPlateform : MonoBehaviour
{
    public string message = "saucisse";
    public float timeRemaining = 10;
    private bool timerIsRunning = true;
    public Vector3 spawnPosition;
    private GUIStyle guiStyle = new GUIStyle();

    void Start()
    {
        guiStyle.fontSize = 36;
        spawnPosition = transform.position + Vector3.up * 3f;
        TeleportPlayersToPlatform(spawnPosition);
        Debug.Log(spawnPosition);
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void OnGUI()
    {
        if (timeRemaining > 0)
        {
            float minutes = Mathf.FloorToInt(timeRemaining / 60);
            float seconds = Mathf.FloorToInt(timeRemaining % 60);
            float milliseconds = (timeRemaining * 1000) % 1000;

            string timeString = string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);

            GUI.Label(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 75, 300, 150), timeString, guiStyle);
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
