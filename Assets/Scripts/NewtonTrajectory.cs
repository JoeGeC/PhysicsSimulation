using UnityEngine;
 
public class NewtonTrajectory : Trajectory
{
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
        nextUpdateTime += timeStep;
    }

    private Vector3 CalculateNewPosition()
    {
        var newX = CalculateAxis(startingPosition.x, velocity.x, acceleration.x);
        var newY = CalculateAxis(startingPosition.y, velocity.y, acceleration.y);
        var newZ = CalculateAxis(startingPosition.z, velocity.z, acceleration.z);
        return new Vector3(newX, newY, newZ);
    }

    private float CalculateAxis(float startingPos, float velocityAxis, float axisAcceleration)
    {
        return startingPos + velocityAxis * nextUpdateTime + axisAcceleration * (nextUpdateTime * nextUpdateTime) / 2; 
    }
}
