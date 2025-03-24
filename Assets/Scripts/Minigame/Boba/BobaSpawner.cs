using UnityEngine;
using System.Collections;

public class BobaSpawner : MonoBehaviour
{
    [SerializeField] GameObject bobaPrefab;
    [SerializeField] private int totalPearls;
    [SerializeField] private float spawnHeight;
    private int currentPearls;
    private float radius;
    private float currentTime;
    [SerializeField] private float spawnTimeCooldown = 0.05f; // mainly to make sure the spawning happens without too many collisions
    private Vector3 spawnPoint;

    // Use this for initialization
    void Start()
    {
        currentPearls = 0;
        currentTime = 0;
        spawnPoint = GetComponent<Transform>().position;

        //get the radius of the pot for spawning the boba
        radius = transform.lossyScale.x / 2;

    }

    //spawn amount of boba given :)
    void SpawnBoba()
    {
        //calculate random x and z given the radius
        float x = transform.position.x + Random.Range(-radius, radius);
        float z = transform.position.z + Random.Range(-radius, radius);
        Vector3 bobaSpawnPoint = new Vector3(x, spawnPoint.y + spawnHeight, z);

        Instantiate(bobaPrefab, bobaSpawnPoint, Quaternion.identity);
        //todo - pick random spawn point around the radius of the pot and use that to make sure the boba doesn't all collide together
        currentPearls++;
    }

    private void Update()
    {
        currentTime += Time.deltaTime;
        if (currentTime > spawnTimeCooldown && currentPearls < totalPearls)
        {
            SpawnBoba();
            currentTime = 0;
        }

    }

    public bool bobaDoneSpawning()
    {
        return currentPearls == totalPearls;
    }
}
