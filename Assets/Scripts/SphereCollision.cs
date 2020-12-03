using UnityEngine;

public class SphereCollision : Collision
{
    public GameObject sphereToCollideWith;
    private Trajectory sphereToCollideWithTrajectory;
    private Trajectory trajectory;
    private float radius;
    private float otherSphereRadius;
    private static float SphereMass { get; } = 1.0f;

    void Start()
    {
        sphereToCollideWithTrajectory = sphereToCollideWith.GetComponent<Trajectory>();
        trajectory = GetComponent<Trajectory>();
        radius = gameObject.Radius();
        otherSphereRadius = sphereToCollideWith.Radius();
    }
    
    public override void CheckForCollision()
    {
        if (!trajectory.Moving()) return;
        var vectorBetweenSpheres = sphereToCollideWith.transform.position - transform.position;
        var sphereVelocityAngle = Angle(trajectory.velocity * Time.deltaTime, vectorBetweenSpheres);
        if (!SphereIsHeadingTowardCollider(sphereVelocityAngle)) return;
        var closestDistanceBetweenSpheres = Mathf.Sin(sphereVelocityAngle * Mathf.Deg2Rad) * Vector3.Magnitude(vectorBetweenSpheres);
        if (!SpheresCanCollide(closestDistanceBetweenSpheres)) return;
        if (!SpheresHaveCollided(closestDistanceBetweenSpheres, sphereVelocityAngle, vectorBetweenSpheres)) return;
        Bounce(vectorBetweenSpheres);
    }

    private static float Angle(Vector3 vector1, Vector3 vector2)
    {
        return Mathf.Acos(Vector3.Dot(vector1, vector2) / (Vector3.Magnitude(vector1) * Vector3.Magnitude(vector2))) * Mathf.Rad2Deg;
    }
    
    private static bool SphereIsHeadingTowardCollider(float sphereVelocityAngle) { return sphereVelocityAngle < 90; }

    private bool SpheresCanCollide(float shortestDistanceBetweenSpheres) { return shortestDistanceBetweenSpheres <= radius + otherSphereRadius; }

    private bool SpheresHaveCollided(float closestDistanceBetweenSpheres, float sphereVelocityAngle, Vector3 vectorBetweenSpheres)
    {
        var collisionAndClosestDistance = Mathf.Sqrt((radius + otherSphereRadius) * (radius + otherSphereRadius) - closestDistanceBetweenSpheres * closestDistanceBetweenSpheres);
        var distanceToCollision = Mathf.Cos(sphereVelocityAngle * Mathf.Deg2Rad) * Vector3.Magnitude(vectorBetweenSpheres) - collisionAndClosestDistance;
        if (distanceToCollision < float.Epsilon) distanceToCollision = 0.0f;
        return distanceToCollision <= trajectory.velocity.magnitude * Time.deltaTime;
    }

    private void Bounce(Vector3 vectorBetweenSpheres)
    {
        sphereToCollideWithTrajectory.velocity = ColliderBounceVelocity(vectorBetweenSpheres);
        trajectory.velocity = BounceVelocity();
    }

    private Vector3 ColliderBounceVelocity(Vector3 vectorBetweenSpheres)
    {
        var positionOfSphere = transform.position;
        var positionOfContact = positionOfSphere + radius * vectorBetweenSpheres;
        var g = sphereToCollideWith.transform.position - positionOfContact;
        var q = Angle(trajectory.velocity, g);
        return Mathf.Cos(q * Mathf.Deg2Rad) * (trajectory.velocity / SphereMass);
    }

    private Vector3 BounceVelocity()
    {
        return (trajectory.velocity * SphereMass - sphereToCollideWithTrajectory.velocity * SphereMass) / SphereMass;
    }
}