using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class SpawnArea : MonoBehaviour
{
    protected Collider teleportArea; // Collider de la zone de t�l�portation
    protected int numberOfPlayers; // Nombre de spawn � cr�er en fonction du nombre de joueur
    protected List<Vector3> spawnList;
    protected bool spawnCreate = false;

    private void Awake()
    {
        spawnList = new List<Vector3>();
    }

    private void Update()
    {
        if(PartyManager.GetPartyState() && !spawnCreate)
        {
            InitiateSpawnPoint();
            spawnCreate = true;
        }
    }

    private void InitiateSpawnPoint()
    {
        teleportArea = GetComponent<Collider>();
        CreateSpawnPoints();
    }

    public void CreateSpawnPoints()
    {
        numberOfPlayers = PartyManager.GetListOfPlayer().Count;
        // V�rifiez s'il y a suffisamment d'espace dans la zone de t�l�portation pour t�l�porter les joueurs
        if (numberOfPlayers > 0)
        {
            RandomSpawnPoints();
        }
        else
        {
            Debug.LogWarning("Aucun joueur � t�l�porter.");
        }
    }

    void RandomSpawnPoints()
    {
        for (int i = 1; i <= numberOfPlayers; i++)
        {
            Vector3 randomPosition = GetRandomPositionInTeleportArea();
            // Tant que la position al�atoire n'est pas � l'int�rieur de la zone de t�l�portation, r�essayez
            while (!IsPositionInTeleportArea(randomPosition))
            {
                randomPosition = GetRandomPositionInTeleportArea();
            }
            spawnList.Add(randomPosition);
        }
    }

    Vector3 GetRandomPositionInTeleportArea()
    {
        Vector3 teleportPosition = new Vector3(
            Random.Range(teleportArea.bounds.min.x, teleportArea.bounds.max.x),
            transform.position.y,
            Random.Range(teleportArea.bounds.min.z, teleportArea.bounds.max.z)
        );
        return teleportPosition;
    }

    bool IsPositionInTeleportArea(Vector3 position)
    {
        // V�rifiez si la position est � l'int�rieur du collider de la zone de t�l�portation
        return teleportArea.bounds.Contains(position);
    }
}
