// File: LevelGenerator.cs
using UnityEngine;
using System.Collections.Generic;

public class MapGenerator : MonoBehaviour
{
    [Header("Setup")]
    public Transform player;              
    public GameObject[] tilePrefabs;       
    public int poolSize = 10;               
    public float tileLength = 30f;

    private Queue<GameObject> tilePool = new Queue<GameObject>();
    private float spawnZ = 0f;  

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject tile = Instantiate(GetRandomTile(), Vector3.forward * spawnZ, tilePrefabs[0].transform.rotation);
            spawnZ += tileLength;
            tilePool.Enqueue(tile);
        }
    }

    void Update()
    {
        // Controlla se il player ha superato la posizione del primo tile
        GameObject firstTile = tilePool.Peek();
        if (player.position.z - firstTile.transform.position.z >= tileLength)
        {
            RecycleTile();
        }
    }

    void RecycleTile()
    {
        GameObject oldTile = tilePool.Dequeue();

        // Riposiziona davanti
        oldTile.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;

        tilePool.Enqueue(oldTile);
    }

    GameObject GetRandomTile()
    {
        int index = Random.Range(0, tilePrefabs.Length);
        return tilePrefabs[index];
    }
}
