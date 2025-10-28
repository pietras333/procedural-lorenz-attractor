using UnityEngine;

public static class LorenzAttractorEquationsSolver
{
    public static float sigma = 10f;
    public static float rho = 28f;
    public static float beta = 8f / 3f;

    // The Lorenz system: returns dx/dt, dy/dt, dz/dt as a Vector3
    private static Vector3 F(Vector3 p)
    {
        return new Vector3(
            sigma * (p.y - p.x),
            p.x * (rho - p.z) - p.y,
            p.x * p.y - beta * p.z
        );
    }
    
    public static Vector3 Integrate(Vector3 p, float dt)
    {
        Vector3 k1 = F(p);
        Vector3 k2 = F(p + 0.5f * dt * k1);
        Vector3 k3 = F(p + 0.5f * dt * k2);
        Vector3 k4 = F(p + dt * k3);

        return p + (dt / 6f) * (k1 + 2f * k2 + 2f * k3 + k4);
    }
    
    public static Vector3 GetDerivative(Vector3 p)
    {
        return new Vector3(
            sigma * (p.y - p.x),
            p.x * (rho - p.z) - p.y,
            p.x * p.y - beta * p.z
        );
    }

}