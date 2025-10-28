using UnityEngine;

public struct AttractedPoint
{
    public Vector3 Position { get; private set; }
    
    public void SetPosition(Vector3 position)
    {
        Position = position;
    }
}