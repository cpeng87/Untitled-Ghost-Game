using UnityEngine;
using System.Collections.Generic;

public class DonutSpawner : MonoBehaviour
{
    [SerializeField] private GameObject donut;
    [SerializeField] private BoxCollider spawnBoundaries;
    [SerializeField] private int numDonuts = 15;
    //[SerializeField] private float donutRadius = 0.25f;
    [SerializeField] private int maxAttemptsPerDonut = 200;

    [SerializeField] private float donutRadius;
    private List<Vector3> donutPositions = new List<Vector3>();

    void Start()
    {
        Vector3 center = spawnBoundaries.bounds.center;
        Vector3 size = spawnBoundaries.bounds.size;

        donutRadius = GetScaledDonutRadius(donut);

        for (int i = 0; i < numDonuts; i++)
        {
            for (int attempt = 0; attempt < maxAttemptsPerDonut; attempt++)
            {
                float x = Random.Range(center.x - size.x / 2f + donutRadius, center.x + size.x / 2f - donutRadius);
                float y = Random.Range(center.y - size.y / 2f + donutRadius, center.y + size.y / 2f - donutRadius);

                Vector3 randomPosition = new Vector3(x, y, center.z);

                if (IsPositionValid(randomPosition))
                {

                    Quaternion rotation = Quaternion.Euler(-45f, 0f, 0f);
                    GameObject currDonut = Instantiate(donut, randomPosition, rotation);
                    currDonut.transform.localScale = new Vector3(950f, 950f, 950f);
                    donutPositions.Add(randomPosition);
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

    float GetScaledDonutRadius(GameObject prefab)
    {
        GameObject temp = Instantiate(prefab);
        temp.transform.localScale = new Vector3(950f, 950f, 950f);
        float radius = temp.GetComponent<Renderer>().bounds.extents.x;
        Destroy(temp);
        return radius;
    }
}
