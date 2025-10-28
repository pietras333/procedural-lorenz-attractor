using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class LorenzAttractor : MonoBehaviour
{
    public LorenzAttractorSettings settings;

    private Mesh mesh;
    private Vector3 lastPosition;

    private Vector3[] points;
    private Color[] colors;
    private int count = 0;
    private int head = 0;

    private Vector3[] verts;
    private Color[] vertsColors;
    private int[] indices;

    private bool meshInitialized = false;

    void Start()
    {
        if (settings == null)
        {
            Debug.LogError("No LorenzAttractorSettings assigned!");
            enabled = false;
            return;
        }

        lastPosition = settings.GetInitialPosition();

        mesh = new Mesh();
        mesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        GetComponent<MeshFilter>().mesh = mesh;

        var mr = GetComponent<MeshRenderer>();
        mr.material = new Material(Shader.Find("Custom/VertexColor"));

        points = new Vector3[settings.maxPoints];
        colors = new Color[settings.maxPoints];
        verts = new Vector3[settings.maxPoints];
        vertsColors = new Color[settings.maxPoints];
        indices = new int[(settings.maxPoints - 1) * 2];

        // Pre-build indices once
        for (int i = 0; i < settings.maxPoints - 1; i++)
        {
            indices[i * 2] = i;
            indices[i * 2 + 1] = i + 1;
        }
    }

    void Update()
    {
        // Integrate
        lastPosition = LorenzAttractorEquationsSolver.Integrate(lastPosition, settings.timeStep);

        // Compute color
        Vector3 velocity = LorenzAttractorEquationsSolver.GetDerivative(lastPosition);
        float speedNorm = Mathf.Clamp01(velocity.magnitude / settings.maxVelocity);
        Color color = settings.velocityGradient.Evaluate(speedNorm);

        // Add to circular buffer
        points[head] = lastPosition * settings.visualizationScale;
        colors[head] = color;

        head = (head + 1) % settings.maxPoints;
        if (count < settings.maxPoints) count++;

        if (count < 2) return;

        // Copy circular buffer to linear arrays
        for (int i = 0; i < count; i++)
        {
            int idx = (head + i) % settings.maxPoints;
            verts[i] = points[idx];
            vertsColors[i] = colors[idx];
        }

        // Initialize mesh once
        if (!meshInitialized)
        {
            mesh.Clear();
            mesh.vertices = verts;
            mesh.colors = vertsColors;
            mesh.SetIndices(indices, 0, (count - 1) * 2, MeshTopology.Lines, 0, calculateBounds: false);
            mesh.UploadMeshData(false);
            meshInitialized = true;
        }
        else
        {
            mesh.vertices = verts;
            mesh.colors = vertsColors;

            if (count * 2 - 2 != mesh.GetIndexCount(0))
            {
                mesh.SetIndices(indices, 0, (count - 1) * 2, MeshTopology.Lines, 0, calculateBounds: false);
            }
        }

        mesh.RecalculateBounds();
    }
    
    public Vector3 GetCenter()
    {
        if (count == 0) return transform.position;

        Vector3 sum = Vector3.zero;
        for (int i = 0; i < count; i++)
        {
            int idx = (head + i) % settings.maxPoints;
            sum += points[idx];
        }
        return sum / count; // average of active points
    }

}
