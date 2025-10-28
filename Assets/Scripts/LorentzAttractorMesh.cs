using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LorenzAttractorMesh : MonoBehaviour
{
    [Header("Attractor Settings")]
    public float timeStep = 0.01f;
    public Vector3 initialPosition = new Vector3(1f, 1f, 1f);
    public float scale = 0.1f;
    public int maxPoints = 1000;
    public float maxVelocity = 40f; // For color normalization

    [Header("Color Settings")]
    public Gradient velocityGradient; // blue -> teal -> yellow -> red

    private Vector3 lastPosition;
    private List<Vector3> points = new List<Vector3>();
    private List<Color> colors = new List<Color>();
    
    private Mesh mesh;

    void Start()
    {
        lastPosition = initialPosition;

        mesh = new Mesh();
        mesh.name = "Lorenz Attractor Mesh";

        GetComponent<MeshFilter>().mesh = mesh;

        // Assign a vertex-color-capable material
        var mr = GetComponent<MeshRenderer>();
        mr.material = new Material(Shader.Find("Custom/VertexColor"));
    }


    void Update()
    {
        // Integrate next point
        lastPosition = LorenzAttractorEquationsSolver.Integrate(lastPosition, timeStep);

        // Add new point
        points.Add(lastPosition * scale);

        // Compute velocity-based color
        Vector3 velocity = LorenzAttractorEquationsSolver.GetDerivative(lastPosition);
        float speedNorm = Mathf.Clamp01(velocity.magnitude / maxVelocity);
        colors.Add(velocityGradient.Evaluate(speedNorm));

        // Keep max number of points
        if (points.Count > maxPoints)
        {
            points.RemoveAt(0);
            colors.RemoveAt(0);
        }

        UpdateMesh();
    }

    void UpdateMesh()
    {
        if (points.Count < 2) return;

        mesh.Clear();

        // Create vertices
        Vector3[] vertices = points.ToArray();

        // Create colors
        Color[] vertexColors = colors.ToArray();

        // Create lines as segments using indices
        int[] indices = new int[(points.Count - 1) * 2];
        for (int i = 0; i < points.Count - 1; i++)
        {
            indices[i * 2] = i;
            indices[i * 2 + 1] = i + 1;
        }

        mesh.vertices = vertices;
        mesh.colors = vertexColors;
        mesh.SetIndices(indices, MeshTopology.Lines, 0);
    }
}
