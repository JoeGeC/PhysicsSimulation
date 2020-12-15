using UnityEngine;

public class SphereCollision : MonoBehaviour
{
    private Sphere thisSphere;
    private static float SphereMass { get; } = 1.0f;

    void Start()
    {
        thisSphere = GetComponent<Sphere>();
    }
    
    public void CheckForCollision(Sphere colliderSphere)
    {
        if (!thisSphere.Moving()) return;
        var vectorBetweenSpheres = colliderSphere.transform.position - transform.position;
        var sphereVelocityAngle = Angle(thisSphere.velocity * Time.fixedDeltaTime, vectorBetweenSpheres);
        if (!SphereIsHeadingTowardCollider(sphereVelocityAngle)) return;
        var closestDistanceBetweenSpheres = Mathf.Sin(sphereVelocityAngle * Mathf.Deg2Rad) * Vector3.Magnitude(vectorBetweenSpheres);
        if (!SpheresCanCollide(closestDistanceBetweenSpheres, colliderSphere)) return;
        if (!SpheresHaveCollided(colliderSphere, closestDistanceBetweenSpheres, sphereVelocityAngle, vectorBetweenSpheres)) return;
        Bounce(colliderSphere, vectorBetweenSpheres);
    }

    private static float Angle(Vector3 vector1, Vector3 vector2)
    {
        return Mathf.Acos(Vector3.Dot(vector1, vector2) / (Vector3.Magnitude(vector1) * Vector3.Magnitude(vector2))) * Mathf.Rad2Deg;
    }
    
    private static bool SphereIsHeadingTowardCollider(float sphereVelocityAngle) { return sphereVelocityAngle < 90; }

    private bool SpheresCanCollide(float shortestDistanceBetweenSpheres, Sphere colliderSphere) { return shortestDistanceBetweenSpheres <= thisSphere.radius + colliderSphere.radius; }

    private bool SpheresHaveCollided(Sphere colliderSphere, float closestDistanceBetweenSpheres, float sphereVelocityAngle, Vector3 vectorBetweenSpheres)
    {
        var sumOfRadii = thisSphere.radius + colliderSphere.radius;
        var collisionAndClosestDistance = Mathf.Sqrt(sumOfRadii * sumOfRadii - closestDistanceBetweenSpheres * closestDistanceBetweenSpheres);
        var distanceToCollision = Mathf.Cos(sphereVelocityAngle * Mathf.Deg2Rad) * Vector3.Magnitude(vectorBetweenSpheres) - collisionAndClosestDistance;
        if (distanceToCollision < float.Epsilon) distanceToCollision = 0.0f;
        return distanceToCollision <= thisSphere.velocity.magnitude * Time.fixedDeltaTime;
    }

    private void Bounce(Sphere colliderSphere, Vector3 vectorBetweenSpheres)
    {
        colliderSphere.velocity = ColliderBounceVelocity(colliderSphere, vectorBetweenSpheres);
        thisSphere.velocity = BounceVelocity(colliderSphere);
    }

    private Vector3 ColliderBounceVelocity(Sphere colliderSphere, Vector3 vectorBetweenSpheres)
    {
        var positionOfSphere = transform.position;
        var positionOfContact = positionOfSphere + thisSphere.radius * vectorBetweenSpheres;
        var colliderPositionOfContact = colliderSphere.transform.position - positionOfContact;
        var angleBetweenContactPositions = Angle(thisSphere.velocity, colliderPositionOfContact);
        return Mathf.Cos(angleBetweenContactPositions * Mathf.Deg2Rad) * (thisSphere.velocity / SphereMass);
    }

    private Vector3 BounceVelocity(Sphere colliderSphere)
    {
        return (thisSphere.velocity * SphereMass - colliderSphere.velocity * SphereMass) / SphereMass;
    }
}