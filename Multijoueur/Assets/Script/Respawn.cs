using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public float treshold;
    public Vector3 spawnPosition;

    private void Start()
    {
        spawnPosition = transform.position;
    }


    // Start is called before the first frame update
    void FixedUpdate()
    {
        if(transform.position.y < treshold) {
            transform.position = spawnPosition;
        }
    }
    
}
