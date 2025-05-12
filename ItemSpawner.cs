using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    public GameObject[] itemsToSpawn;
    public float spawnInterval = 1.5f;
    public float spawnRangeX = 8f;

    private float elapsedTime = 0f;
    private float timeToNextSpeedUp = 15f;
    private float minSpawnInterval = 0.5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnItem), 1f, spawnInterval);
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= timeToNextSpeedUp && spawnInterval > minSpawnInterval)
        {
            elapsedTime = 0f;
            spawnInterval -= 0.2f; 
            spawnInterval = Mathf.Max(spawnInterval, minSpawnInterval); 

            CancelInvoke(nameof(SpawnItem));
            InvokeRepeating(nameof(SpawnItem), 0f, spawnInterval);
        }
    }

    void SpawnItem()
    {
        int index = Random.Range(0, itemsToSpawn.Length);
        float xPos = Random.Range(-spawnRangeX, spawnRangeX);
        Vector2 spawnPos = new Vector2(xPos, transform.position.y);

        Instantiate(itemsToSpawn[index], spawnPos, Quaternion.identity);
    }
}
