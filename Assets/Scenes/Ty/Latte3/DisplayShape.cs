using System.Collections.Generic;
using UnityEngine;

// Attach this to an empty GameObject with a MeshFilter and MeshRenderer,
// parented under the drawing surface. It builds a static outline mesh for
// the shape the player is meant to trace, using the same "verts + tris,
// assign to mesh" ribbon technique as TraceDrawer/CreateMesh.cs, except the
// path is generated once instead of grown from input.

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class DisplayShape : MonoBehaviour
{
    [SerializeField] enum ShapeType { Circle, Heart }
    [SerializeField] ShapeType shapeType;
    [SerializeField] float radius = 0.8f;

    [SerializeField] int segments = 100;

    [SerializeField] float lineWidth = 0.2f;

    private Mesh mesh;
    private readonly List<Vector3> shapePointsLocal = new List<Vector3>();

    void Awake()
    {
        mesh = new Mesh { name = "TargetShapeMesh" };
        GetComponent<MeshFilter>().mesh = mesh;
        BuildShape();
    }

    void OnValidate()
    {
        if (mesh != null)
            BuildShape();
    }

    public void BuildShape()
    {
        shapePointsLocal.Clear();

        switch (shapeType)
        {
            case ShapeType.Circle:
                for (int i = 0; i <= segments; i++)
                {
                    float t = (i / (float)segments) * Mathf.PI * 2f;
                    shapePointsLocal.Add(new Vector3(Mathf.Cos(t) * radius, 0f, Mathf.Sin(t) * radius));
                }
                break;

            case ShapeType.Heart:
                for (int i = 0; i <= segments; i++)
                {
                    float t = (i / (float)segments) * Mathf.PI * 2f;
                    
                    // Parametric heart curve (scaled to fit in radius)
                    float x = 16f * Mathf.Pow(Mathf.Sin(t), 3f);
                    float y = 13f * Mathf.Cos(t) - 5f * Mathf.Cos(2f * t) - 2f * Mathf.Cos(3f * t) - Mathf.Cos(4f * t);
                    
                    // Scale by radius (divide by max extent ~13)
                    float scale = radius / 13f;
                    x *= scale;
                    y *= scale;
                    
                    shapePointsLocal.Add(new Vector3(x, 0f, y));
                }
                break;
        }

        BuildRibbonMesh();
    }

    // Same technique as MakeQuad() in CreateMesh.cs and AddSegment() in
    // TraceDrawer: four corner points per segment, two triangles per quad.
    // Here it runs once over the whole path instead of once per frame.
    void BuildRibbonMesh()
    {
        var verts = new List<Vector3>();
        var tris = new List<int>();

        int pointCount = shapePointsLocal.Count;
        if (pointCount < 2)
        {
            mesh.Clear();
            return;
        }

        int segmentCount = pointCount;
        float overlapAmount = 0.05f; // Small overlap between segments

        for (int i = 0; i < segmentCount; i++)
        {
            Vector3 from = shapePointsLocal[i];
            Vector3 to = shapePointsLocal[(i + 1) % pointCount];

            Vector3 direction = (to - from).normalized;
            
            // Extend slightly to create overlap with adjacent segments
            Vector3 extendedFrom = from - direction * overlapAmount;
            Vector3 extendedTo = to + direction * overlapAmount;
            
            Vector3 side = Vector3.Cross(direction, Vector3.up).normalized * (lineWidth * 0.5f);

            int baseIndex = verts.Count;

            verts.Add(extendedFrom - side);
            verts.Add(extendedFrom + side);
            verts.Add(extendedTo - side);
            verts.Add(extendedTo + side);

            tris.Add(baseIndex + 0);
            tris.Add(baseIndex + 1);
            tris.Add(baseIndex + 2);

            tris.Add(baseIndex + 1);
            tris.Add(baseIndex + 3);
            tris.Add(baseIndex + 2);
        }

        mesh.Clear();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }

    public List<Vector3> GetShapePointsWorld()
    {
        var worldPoints = new List<Vector3>(shapePointsLocal.Count);
        foreach (Vector3 p in shapePointsLocal)
        {
            worldPoints.Add(transform.TransformPoint(p));
        }
            
        return worldPoints;
    }
}