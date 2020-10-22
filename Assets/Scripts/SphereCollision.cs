using UnityEngine;

public class SphereCollision : Collision
{
    public GameObject sphereToCollideWith;
    public Trajectory trajectory;
    private float r1;
    private float r2;

    void Start()
    {
        r1 = gameObject.Radius();
        r2 = sphereToCollideWith.Radius();
    }
    
    public override float CheckForCollision()
    {
        var vectorBetweenSpheres = sphereToCollideWith.transform.position - transform.position;
        var sphereVelocityAngle = Angle(trajectory.velocity, vectorBetweenSpheres);
        if (SphereIsHeadingTowardCollider(sphereVelocityAngle)) return 1.0f;
        var closestDistanceBetweenSpheres = Mathf.Sin(sphereVelocityAngle * Mathf.Deg2Rad) * Vector3.Magnitude(vectorBetweenSpheres);
        if (!SpheresCanCollide(closestDistanceBetweenSpheres)) return 1.0f;
        var collisionAndClosestDistance = Mathf.Sqrt((r1 + r2) * (r1 + r2) - closestDistanceBetweenSpheres * closestDistanceBetweenSpheres);
        var distanceToCollision = Mathf.Cos(sphereVelocityAngle * Mathf.Deg2Rad) * Vector3.Magnitude(vectorBetweenSpheres) - collisionAndClosestDistance;
        if (SpheresHaveCollided(distanceToCollision)) return distanceToCollision / (trajectory.velocity.magnitude * trajectory.timeStep);
        return 1.0f;
    }

    private static float Angle(Vector3 vector1, Vector3 vector2)
    {
        return Mathf.Acos(Vector3.Dot(vector1, vector2) / (Vector3.Magnitude(vector1) * Vector3.Magnitude(vector2))) * Mathf.Rad2Deg;
    }
    
    private static bool SphereIsHeadingTowardCollider(float sphereVelocityAngle) { return sphereVelocityAngle < 90; }

    private bool SpheresCanCollide(float shortestDistanceBetweenSpheres) { return shortestDistanceBetweenSpheres <= r1 + r2; }

    private bool SpheresHaveCollided(float distanceBetweenSpheres) { return distanceBetweenSpheres <= trajectory.velocity.magnitude * trajectory.timeStep; }
}