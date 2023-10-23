using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnLine : MonoBehaviour
{
    public Transform startLine; // La position de la ligne de départ
    public float spacing = 2.0f; // Espacement entre les joueurs

    /*void Start()
    {
        // Recherchez tous les joueurs dans la scène
        Player[] players = FindObjectsOfType<Player>();

        // Placez chaque joueur le long de la ligne de départ
        int numPlayers = players.Length;

        // Calculez la position du premier joueur
        Vector3 startLineCenter = startLine.position;
        Vector3 startPlayerPosition = startLineCenter - startLine.forward * (spacing * (numPlayers - 1) / 2.0f);

        for (int i = 0; i < numPlayers; i++)
        {
            Transform playerTransform = players[i].transform;
            Vector3 playerPosition = startPlayerPosition + startLine.forward * (spacing * i);
            playerTransform.position = playerPosition;
        }
    }*/
}

