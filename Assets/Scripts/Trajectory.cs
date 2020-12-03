using UnityEngine;

public abstract class Trajectory : MonoBehaviour
{
    public Vector3 velocity = new Vector3(10, 10, 10);
    protected Vector3 Acceleration = new Vector3(0, -9.8f, 0);

    public bool Moving() { return velocity != Vector3.zero; }
}