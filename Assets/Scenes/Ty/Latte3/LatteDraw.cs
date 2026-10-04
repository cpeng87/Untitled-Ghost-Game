using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class LatteDraw : MonoBehaviour
{
    [SerializeField] float brushSize = 0.2f;
    [SerializeField] LayerMask surfaceLayer;
    [SerializeField] float surfaceLift = 0.002f;   // avoids z-fighting with the surface
    [SerializeField] float surfaceRadius = 3.8f;   // 3.8^2 = 14.44

    [Header("Particles")]
    [SerializeField] private ParticleSystem ps;
    [SerializeField] int particlesPerPoint = 1;
    [SerializeField] float minParticleSpeed = 0.1f;
    [SerializeField] float maxParticleSpeed = 0.4f;

    private Mesh mesh;
    private readonly List<Vector3> tracedPoints = new List<Vector3>();
    private readonly List<Vector3> verts = new List<Vector3>();
    private readonly List<Vector2> uvs = new List<Vector2>();
    private readonly List<int> tris = new List<int>();
    private bool isDrawing = false;
    private Vector3 lastPoint;
    public bool isPaused = false;

    void Start()
    {
        mesh = new Mesh();
        mesh.indexFormat = IndexFormat.UInt32; // lets the mesh exceed 65k vertices
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
        if (!isDrawing || !RaycastToSurface(out Vector3 hitPoint) || Vector3.Distance(hitPoint, lastPoint) < 0.02f) {
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
        mesh.SetUVs(0, uvs);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();

        EmitParticles(point);
    }

    // not relevant rn
    void EmitParticles(Vector3 point)
    {
        if (ps == null) return;

        for (int i = 0; i < particlesPerPoint; i++)
        {
            var emitParams = new ParticleSystem.EmitParams
            {
                position = point + Vector3.up * surfaceLift,
                velocity = Random.onUnitSphere * Random.Range(minParticleSpeed, maxParticleSpeed),
                // startLifetime = particleLifetime
            };
            ps.Emit(emitParams, 1);
        }
    }

    Vector2 WorldToUV(Vector3 p)
    {
        return new Vector2(p.x, p.z) / (surfaceRadius * 2f) + new Vector2(0.5f, 0.5f);
    }

    void AddCircleStamp(Vector3 center)
    {
        if ((center.x * center.x) + (center.z * center.z) >= 14.44f)
        {
            return;
        }

        center += Vector3.up * surfaceLift;

        int baseIndex = verts.Count;

        verts.Add(center);
        uvs.Add(WorldToUV(center));

        for (int i = 0; i <= 15; i++)
        {
            float t = (2 * i / 15f) * Mathf.PI;
            Vector3 offset = new Vector3(Mathf.Cos(t), 0f, Mathf.Sin(t)) * brushSize;
            Vector3 p = center + offset;
            verts.Add(p);
            uvs.Add(WorldToUV(p));
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
        uvs.Clear();
        tris.Clear();
        if (mesh != null) mesh.Clear();
    }

    public List<Vector3> GetTracedPoints()
    {
        return tracedPoints;
    }
}