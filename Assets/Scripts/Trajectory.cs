using UnityEngine;

public abstract class Trajectory : MonoBehaviour
{
    public Vector3 velocity = new Vector3(10, 10, 10);
    public Vector3 acceleration = new Vector3(0, -9.8f, 0);
    public float timeStep = 1.0f / 60.0f;

    public void Stop()
    {
        var empty = new Vector3(0, 0, 0);
        velocity = empty;
        acceleration = empty;
    }
}