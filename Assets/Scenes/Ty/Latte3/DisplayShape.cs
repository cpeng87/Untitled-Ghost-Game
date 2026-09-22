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
    [SerializeField, Range(0f, 1f)] float patternAlpha = 0.5f;

    [SerializeField] int segments = 100;

    [SerializeField] float lineWidth = 0.2f;

    private Mesh mesh;
    private readonly List<Vector3> shapePointsLocal = new List<Vector3>();

    void Awake()
    {
        mesh = new Mesh { name = "TargetShapeMesh" };
        GetComponent<MeshFilter>().mesh = mesh;
        var rend = GetComponent<Renderer>();
        if (rend != null && rend.material != null)
        {
            Color c = rend.material.color;
            c.a = patternAlpha;
            rend.material.color = c;
        }
        BuildShape();
    }

    void OnValidate()
    {
        var rend = GetComponent<Renderer>();
        if (rend != null)
        {
            if (Application.isPlaying)
            {
                if (rend.material != null)
                {
                    Color c = rend.material.color;
                    c.a = patternAlpha;
                    rend.material.color = c;
                }
            }
            else
            {
                if (rend.sharedMaterial != null)
                {
                    Color c = rend.sharedMaterial.color;
                    c.a = patternAlpha;
                    rend.sharedMaterial.color = c;
                }
            }
        }

        if (mesh != null)
            BuildShape();
    }

    public void BuildShape()
    {
        shapePointsLocal.Clear();

        switch (shapeType)
        {
            case ShapeType.Circle:
                for (int i = 0; i < segments; i++)
                {
                    float t = (i / (float)segments) * Mathf.PI * 2f;
                    shapePointsLocal.Add(new Vector3(Mathf.Cos(t) * radius, 0f, Mathf.Sin(t) * radius));
                }
                break;

            case ShapeType.Heart:
                for (int i = 0; i < segments; i++)
                {
                    // Offset the parameter so the closing seam lands on the heart's side,
                    // away from the tip and the inward notch where stacked quads become too bright.
                    float t = (i / (float)segments) * Mathf.PI * 2f + Mathf.PI * 0.5f;

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

        for (int i = 0; i < segmentCount; i++)
        {
            Vector3 from = shapePointsLocal[i];
            Vector3 to = shapePointsLocal[(i + 1) % pointCount];

            Vector3 delta = to - from;
            float segmentLength = delta.magnitude;
            if (segmentLength < 0.0001f)
                continue;

            Vector3 direction = delta / segmentLength;
            Vector3 side = Vector3.Cross(direction, Vector3.up);
            if (side.sqrMagnitude < 0.0001f)
            {
                side = Vector3.Cross(direction, Vector3.right);
            }
            side = side.normalized * (lineWidth * 0.5f);

            int baseIndex = verts.Count;

            verts.Add(from - side);
            verts.Add(from + side);
            verts.Add(to - side);
            verts.Add(to + side);

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