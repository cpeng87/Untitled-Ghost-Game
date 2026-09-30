using System.Collections.Generic;
using UnityEngine;

public class LatteDraw : MonoBehaviour
{
    [SerializeField] float brushSize = 0.2f;
    [SerializeField] LayerMask surfaceLayer;
    private Mesh mesh;
    private readonly List<Vector3> tracedPoints = new List<Vector3>();
    private readonly List<Vector3> verts = new List<Vector3>();
    private readonly List<int> tris = new List<int>();
    private bool isDrawing = false;
    private Vector3 lastPoint;
    public bool isPaused = false;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    void Update()
    {
        if (isPaused) {
            return;
        }
        if (Input.GetMouseButtonDown(0)) {
            StartNewStroke();
        }
        if (Input.GetMouseButton(0)) {
            TryAddPoint();
        }
        if (Input.GetMouseButtonUp(0)) {
            isDrawing = false;
        }
    }

    public void SetPaused(bool paused)
    {
        isPaused = paused;
        if (isPaused)
        {
            isDrawing = false;
        }
    }

    void StartNewStroke()
    {
        if (RaycastToSurface(out Vector3 hitPoint))
        {
            isDrawing = true;
            lastPoint = hitPoint;
            AddPoint(hitPoint);
        }
    }

    void TryAddPoint()
    {
        if (!isDrawing || !RaycastToSurface(out Vector3 hitPoint) || Vector3.Distance(hitPoint, lastPoint) < 0.02) {
            return;
        }

        AddPoint(hitPoint);
        lastPoint = hitPoint;
    }

    bool RaycastToSurface(out Vector3 point)
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, surfaceLayer))
        {
            point = hit.point;
            return true;
        } else
        {
            point = Vector3.zero;
            return false;
        }
    }

    void AddPoint(Vector3 point)
    {
        tracedPoints.Add(point);
        AddCircleStamp(point);

        mesh.Clear();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    void AddCircleStamp(Vector3 center)
    {
        int baseIndex = verts.Count;

        verts.Add(center);

        for (int i = 0; i <= 15; i++)
        {
            float t = (2*i / (float)15f) * Mathf.PI;
            Vector3 offset = new Vector3(Mathf.Cos(t), 0f, Mathf.Sin(t)) * brushSize;
            verts.Add(center + offset);
        }

        for (int i = 0; i < 15; i++)
        {
            tris.Add(baseIndex);
            tris.Add(baseIndex + 2 + i);
            tris.Add(baseIndex + 1 + i);
        }
    }

    public void ClearDrawing()
    {
        tracedPoints.Clear();
        verts.Clear();
        tris.Clear();
        if (mesh != null) mesh.Clear();
    }

    public List<Vector3> GetTracedPoints()
    {
        return tracedPoints;
    } 
}