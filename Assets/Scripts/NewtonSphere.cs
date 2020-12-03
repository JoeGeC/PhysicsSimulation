using UnityEngine;
 
public class NewtonSphere : Sphere
{
    private Vector3 startingPosition;
    private float nextUpdateTime;

    protected override void Start()
    {
        base.Start();
        startingPosition = transform.position;
    }
    
    void FixedUpdate()
    {
        nextUpdateTime += Time.fixedDeltaTime;
        transform.position = CalculateNewPosition();
    }

    private Vector3 CalculateNewPosition()
    {
        var newX = CalculateAxis(startingPosition.x, velocity.x, Acceleration.x);
        var newY = CalculateAxis(startingPosition.y, velocity.y, Acceleration.y);
        var newZ = CalculateAxis(startingPosition.z, velocity.z, Acceleration.z);
        return new Vector3(newX, newY, newZ);
    }

    private float CalculateAxis(float startingPos, float velocityAxis, float axisAcceleration)
    {
        return startingPos + velocityAxis * nextUpdateTime + axisAcceleration * (nextUpdateTime * nextUpdateTime) / 2; 
    }
}
