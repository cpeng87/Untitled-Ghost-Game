using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [SerializeField] private GameObject obj;
    [SerializeField] private BoxCollider spawnBoundaries;
    // [SerializeField] private int numObjs = 15;
    [SerializeField] private float frequency = 0.01f;
    private float timer = 0;

    private List<Vector3> donutPositions = new List<Vector3>();

    void Update()
    {
        Vector3 center = spawnBoundaries.center + spawnBoundaries.transform.position;
        Vector3 size = spawnBoundaries.size;
        timer += Time.deltaTime;

        if (timer > frequency)
        {
            Debug.Log("placing boba");
            timer = 0;
            Vector3 randomPosition = new Vector3(
                ((Random.value * 2) - 1) * (size.x / 2f) + center.x,
                ((Random.value * 2) - 1) * (size.y / 2f) + center.y,
                ((Random.value * 2) - 1) * (size.y / 2f) + center.z
            );
            Quaternion rotation = Quaternion.Euler(-45f, 0f, 0f);
            GameObject currDonut = Instantiate(obj, randomPosition, rotation);
            donutPositions.Add(randomPosition);
        }
    }
}