using UnityEngine;
 
public class NewtonTrajectory : MonoBehaviour
{
    public Vector3 velocity = new Vector3(10, 10, 10);
    public Vector3 acceleration = new Vector3(0, -9.8f, 0);
    private const float TimeStep = 1.0f / 60.0f;
    private Vector3 startingPosition;
    private float nextUpdateTime;

    void Start()
    {
        startingPosition = transform.position;
    }
    
    void Update()
    {
        if (Time.time < nextUpdateTime) return;
        transform.position = CalculateNewPosition();
        nextUpdateTime += TimeStep;
    }

    private Vector3 CalculateNewPosition()
    {
        var newX = CalculateAxis(startingPosition.x, velocity.x, acceleration.x);
        var newY = CalculateAxis(startingPosition.y, velocity.y, acceleration.y);
        var newZ = CalculateAxis(startingPosition.z, velocity.z, acceleration.z);
        return new Vector3(newX, newY, newZ);
    }

    private float CalculateAxis(float startingPos, float velocityAxis, float acceleration)
    {
        return startingPos + velocityAxis * nextUpdateTime + acceleration * (nextUpdateTime * nextUpdateTime) / 2; 
    }
}
