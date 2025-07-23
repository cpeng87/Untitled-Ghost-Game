using UnityEngine;
using System.Collections.Generic;

public class DonutSpawner : MonoBehaviour
{
    [SerializeField] private GameObject donut;
    [SerializeField] private BoxCollider spawnBoundaries;
    [SerializeField] private int numDonuts = 15;
    [SerializeField] private float donutRadius = 0.25f;
    [SerializeField] private int maxAttemptsPerDonut = 100;

    private List<Vector3> donutPositions = new List<Vector3>();

    void Start()
    {
        Vector3 center = spawnBoundaries.center + spawnBoundaries.transform.position;
        Vector3 size = spawnBoundaries.size;

        for (int i = 0; i < numDonuts; i++)
        {
            bool placed = false;
            for (int attempt = 0; attempt < maxAttemptsPerDonut; attempt++)
            {
                Vector3 randomPosition = new Vector3(
                    ((Random.value * 2) - 1) * (size.x / 2f) + center.x,
                    ((Random.value * 2) - 1) * (size.y / 2f) + center.y,
                    this.transform.position.z
                );

                if (IsPositionValid(randomPosition))
                {

                    Quaternion rotation = Quaternion.Euler(-45f, 0f, 0f);
                    GameObject currDonut = Instantiate(donut, randomPosition, rotation);
                    currDonut.transform.localScale = new Vector3(1000f, 1000f, 1000f);
                    donutPositions.Add(randomPosition);
                    placed = true;
                    break;
                }
            }
        }
    }

    bool IsPositionValid(Vector3 newPosition)
    {
        foreach (Vector3 pos in donutPositions)
        {
            if (Vector3.Distance(pos, newPosition) < donutRadius * 2f)
                return false;
        }
        return true;
    }
}
