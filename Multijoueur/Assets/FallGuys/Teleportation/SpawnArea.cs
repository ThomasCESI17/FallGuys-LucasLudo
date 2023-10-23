using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public abstract class SpawnArea : MonoBehaviour
{
    protected Collider teleportArea; // Collider de la zone de téléportation
    protected int numberOfPlayers; // Nombre de spawn à créer en fonction du nombre de joueur
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
        // Vérifiez s'il y a suffisamment d'espace dans la zone de téléportation pour téléporter les joueurs
        if (numberOfPlayers > 0)
        {
            RandomSpawnPoints();
        }
        else
        {
            Debug.LogWarning("Aucun joueur à téléporter.");
        }
    }

    void RandomSpawnPoints()
    {
        for (int i = 1; i <= numberOfPlayers; i++)
        {
            Vector3 randomPosition = GetRandomPositionInTeleportArea();
            // Tant que la position aléatoire n'est pas à l'intérieur de la zone de téléportation, réessayez
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
        // Vérifiez si la position est à l'intérieur du collider de la zone de téléportation
        return teleportArea.bounds.Contains(position);
    }
}
