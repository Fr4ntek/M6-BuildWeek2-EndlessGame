// File: ItemSpawner.cs
using UnityEngine;
using System.Collections.Generic;

public class ItemSpawner : MonoBehaviour
{
    [Header("Setup")]
    public Transform player;
    public GameObject[] coinPrefabs;
    public GameObject[] meteorPrefabs;

    [Header("Pool Settings")]
    public int poolSize = 20;
    public float spawnDistance = 50f;
    public float recycleDistance = 15f;
    public float spawnInterval = 1.5f;

    [Header("Offsets")]
    public float coinYOffset = 1.0f;      // monete sopra al terreno
    public float meteorYOffset = 0.3f;    // asteroidi incastrati va bene

    private List<GameObject> coinPool = new List<GameObject>();
    private List<GameObject> meteorPool = new List<GameObject>();

    private readonly float[] lanes = { -3f, 0f, 3f };
    private float spawnTimer;

    // Pattern di esempio: [sinistra, centro, destra]
    // 0 = vuoto, 1 = moneta, 2 = meteorite
    private int[][] patterns =
    {
        new int[] {1, 1, 1}, // fila di monete
        new int[] {2, 0, 0}, // meteorite sinistra
        new int[] {0, 2, 1}, // meteorite centro + moneta destra
        new int[] {0, 0, 0}, // vuoto (pausa)
        new int[] {1, 0, 2}, // moneta sinistra + meteorite destra
        new int[] {2, 2, 2}, // tutti meteoriti
    };

    void Start()
    {
        InitPool(coinPrefabs, coinPool);
        InitPool(meteorPrefabs, meteorPool);
        spawnTimer = spawnInterval;
    }

    void Update()
    {
        spawnTimer -= Time.deltaTime;
        if (spawnTimer <= 0f)
        {
            SpawnPattern();
            spawnTimer = spawnInterval;
        }

        RecycleObjects(coinPool);
        RecycleObjects(meteorPool);
    }

    void InitPool(GameObject[] prefabs, List<GameObject> pool)
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject prefab = prefabs[Random.Range(0, prefabs.Length)];
            GameObject obj = Instantiate(prefab, Vector3.one * 9999, Quaternion.identity);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    void SpawnPattern()
    {
        int[] chosenPattern = patterns[Random.Range(0, patterns.Length)];
        float spawnZ = player.position.z + spawnDistance;

        for (int lane = 0; lane < lanes.Length; lane++)
        {
            if (chosenPattern[lane] == 1)
                SpawnFromPool(coinPool, lanes[lane], spawnZ, coinYOffset);
            else if (chosenPattern[lane] == 2)
                SpawnFromPool(meteorPool, lanes[lane], spawnZ, meteorYOffset);
        }
    }

    void SpawnFromPool(List<GameObject> pool, float laneX, float spawnZ, float yOffset)
    {
        foreach (GameObject obj in pool)
        {
            if (!obj.activeInHierarchy)
            {
                obj.transform.position = new Vector3(laneX, yOffset, spawnZ);
                obj.SetActive(true);
                break;
            }
        }
    }

    void RecycleObjects(List<GameObject> pool)
    {
        foreach (GameObject obj in pool)
        {
            if (obj.activeInHierarchy &&
                player.position.z - obj.transform.position.z > recycleDistance)
            {
                obj.SetActive(false);
            }
        }
    }
}
