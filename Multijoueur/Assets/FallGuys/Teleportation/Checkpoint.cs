using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Unity.Netcode;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    protected Collider teleportArea; // Collider de la zone de t�l�portation
    public int checkpointId;
    private GameObject checkpointLight;
    public Material checkpointMaterial;
    Color colorStart = Color.green;
    Color colorCheck = Color.cyan;

    private void Start()
    {
        checkpointLight = GetComponentInChildren<CheckpointValidated>().gameObject;
        checkpointLight.GetComponentInChildren<Renderer>().material.color = colorStart;
        teleportArea = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        PlayerManager player = other.GetComponentInParent<PlayerManager>();
        if (player.GetActualCheckPoint() < checkpointId)
        {
            player.CheckPointUpdate(checkpointId, RandomSpawnPoints());
        }
        checkpointLight.GetComponentInChildren<Renderer>().material.color = colorCheck;
        StartCoroutine(CheckPointTimer());
    }

    private IEnumerator CheckPointTimer()
    {
        yield return new WaitForSecondsRealtime(0.5f);
        checkpointLight.GetComponentInChildren<Renderer>().material.color = colorStart;
    }

    Vector3 RandomSpawnPoints()
    {
        Vector3 randomPosition = GetRandomPositionInTeleportArea();
        // Tant que la position al�atoire n'est pas � l'int�rieur de la zone de t�l�portation, r�essayez
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
            0,
            Random.Range(teleportArea.bounds.min.z, teleportArea.bounds.max.z)
        );
        return teleportPosition;
    }

    bool IsPositionInTeleportArea(Vector3 position)
    {
        // V�rifiez si la position est � l'int�rieur du collider de la zone de t�l�portation
        return teleportArea.bounds.Contains(position);
    }

    public int GetCheckPointID()
    {
        return checkpointId;
    }
}
