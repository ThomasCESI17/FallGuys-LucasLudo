using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    protected Collider teleportArea; // Collider de la zone de téléportation
    public int checkpointId;

    private void Start()
    {
        teleportArea = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponentInParent<PlayerManager>();
        if (player.GetActualCheckPoint() < checkpointId)
        {
            player.CheckPointUpdate(checkpointId, RandomSpawnPoints());
        }
    }

    Vector3 RandomSpawnPoints()
    {
        Vector3 randomPosition = GetRandomPositionInTeleportArea();
        // Tant que la position aléatoire n'est pas à l'intérieur de la zone de téléportation, réessayez
        while (!IsPositionInTeleportArea(randomPosition))
        {
            randomPosition = GetRandomPositionInTeleportArea();
        }
        return randomPosition;
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
