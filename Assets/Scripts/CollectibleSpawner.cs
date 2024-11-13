using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleSpawner : MonoBehaviour
{
    public GameObject collectiblePrefab;  // Prefab to spawn
    public float spawnInterval = 2f;
    public Vector3 spawnAreaSize = new Vector3(10, 0, 10);
    
    // Add a list of predefined colors (RGB only)
    private Color[] colors = { Color.red, Color.green, Color.blue };

    private void Start()
    {
        InvokeRepeating("SpawnCollectible", 0f, spawnInterval);
    }

    void SpawnCollectible()
    {
        Vector3 spawnPos = new Vector3(
            Random.Range(-spawnAreaSize.x / 2, spawnAreaSize.x / 2),
            1f, 
            Random.Range(-spawnAreaSize.z / 2, spawnAreaSize.z / 2)
        );

        GameObject collectible = Instantiate(collectiblePrefab, spawnPos, Quaternion.identity);

        // Assign a random color
        Color randomColor = colors[Random.Range(0, colors.Length)];
        collectible.GetComponent<Renderer>().material.color = randomColor;

        // Set the collectible's color
        collectible.GetComponent<Collectible>().collectibleColor = randomColor;
    }
}
