using UnityEngine;

[CreateAssetMenu(fileName = "LorenzAttractorSettings", menuName = "Lorenz Attractor/Settings")]
public class LorenzAttractorSettings : ScriptableObject
{
    [Header("Integration")]
    public float timeStep = 0.01f;
    public Vector3 initialPosition = new Vector3(5f, 5f, 15f);
    public float visualizationScale = 0.1f;
    [Range(3, 15000)] public int maxPoints = 10000;

    [Header("Color")]
    public Gradient velocityGradient;
    public float maxVelocity = 150f;

    [Header("Butterfly Effect")]
    public bool enableButterflyEffect = false;
    [Tooltip("Number of decimal digits to round the initial position to.")]
    public int roundingDigits = 3;

    public Vector3 GetInitialPosition()
    {
        if (!enableButterflyEffect) return initialPosition;

        float factor = Mathf.Pow(10f, roundingDigits);
        return new Vector3(
            Mathf.Round(initialPosition.x * factor) / factor,
            Mathf.Round(initialPosition.y * factor) / factor,
            Mathf.Round(initialPosition.z * factor) / factor
        );
    }
}