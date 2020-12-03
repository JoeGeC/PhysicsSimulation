using UnityEngine;

public class SphereCollision : Collision
{
    public GameObject sphereToCollideWith;
    public Trajectory trajectory;
    private float radius;
    private float otherSphereRadius;

    void Start()
    {
        radius = gameObject.Radius();
        otherSphereRadius = sphereToCollideWith.Radius();
    }
    
    public override void CheckForCollision()
    {
        if (trajectory.velocity == Vector3.zero) return;
        var vectorBetweenSpheres = sphereToCollideWith.transform.position - transform.position;
        var sphereVelocityAngle = Angle(trajectory.velocity * Time.deltaTime, vectorBetweenSpheres);
        if (!SphereIsHeadingTowardCollider(sphereVelocityAngle)) return;
        var closestDistanceBetweenSpheres = Mathf.Sin(sphereVelocityAngle * Mathf.Deg2Rad) * Vector3.Magnitude(vectorBetweenSpheres);
        if (!SpheresCanCollide(closestDistanceBetweenSpheres)) return;
        var collisionAndClosestDistance = Mathf.Sqrt((radius + otherSphereRadius) * (radius + otherSphereRadius) - closestDistanceBetweenSpheres * closestDistanceBetweenSpheres);
        var distanceToCollision = Mathf.Cos(sphereVelocityAngle * Mathf.Deg2Rad) * Vector3.Magnitude(vectorBetweenSpheres) - collisionAndClosestDistance;
        if (distanceToCollision < float.Epsilon) distanceToCollision = 0.0f;
        if (SpheresHaveCollided(distanceToCollision))
        {
            var positionOfSphere = transform.position;
            var positionOfContact = positionOfSphere + radius * vectorBetweenSpheres;
            var g = sphereToCollideWith.transform.position - positionOfContact;
            var q = Angle(trajectory.velocity, g);
            var sphereMass = 1.0f;
            sphereToCollideWith.GetComponent<Trajectory>().velocity = Mathf.Cos(q * Mathf.Deg2Rad) * (trajectory.velocity / sphereMass);
            trajectory.velocity = (trajectory.velocity * sphereMass - sphereToCollideWith.GetComponent<Trajectory>().velocity * sphereMass) / sphereMass;
        }
    }

    private static float Angle(Vector3 vector1, Vector3 vector2)
    {
        return Mathf.Acos(Vector3.Dot(vector1, vector2) / (Vector3.Magnitude(vector1) * Vector3.Magnitude(vector2))) * Mathf.Rad2Deg;
    }
    
    private static bool SphereIsHeadingTowardCollider(float sphereVelocityAngle) { return sphereVelocityAngle < 90; }

    private bool SpheresCanCollide(float shortestDistanceBetweenSpheres) { return shortestDistanceBetweenSpheres <= radius + otherSphereRadius; }

    private bool SpheresHaveCollided(float distanceBetweenSpheres)
    {
        return distanceBetweenSpheres <= trajectory.velocity.magnitude * Time.deltaTime;
    }
}