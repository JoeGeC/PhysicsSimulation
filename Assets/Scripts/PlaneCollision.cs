using UnityEngine;

public class PlaneCollision : MonoBehaviour
{
    private Sphere thisSphere;
    private Vector3 planeNormal;

    private void Start()
    {
        thisSphere = GetComponent<Sphere>();
    }

    public void CheckForCollision(GameObject plane)
    {
        planeNormal = PlaneNormal(plane);
        if (!thisSphere.Moving()) return;
        if (!HeadingTowardPlane()) return;
        if (!HasCollided(DistanceToCollision(plane))) return;
        thisSphere.velocity = BounceVelocity();
    }
    
    private static Vector3 PlaneNormal(GameObject plane)
    {
        var planePos = plane.transform.position;
        var a = new Vector3(planePos.x + 10, planePos.y, planePos.z + 10);
        var b = new Vector3(planePos.x - 10, planePos.y, planePos.z - 10);
        var c = new Vector3(planePos.x + 18, planePos.y, planePos.z + 15);
        return Vector3.Cross(b - a, c - a).normalized * -1;
    }

    private bool HeadingTowardPlane() { return Vector3.Angle(planeNormal, -thisSphere.velocity) <= 90.0f; }

    private bool HasCollided(double distanceToCollision) { return distanceToCollision <= thisSphere.velocity.magnitude * Time.fixedDeltaTime; }

    private float DistanceToCollision(GameObject plane)
    {
        var p = transform.position - plane.transform.position;
        var angleBetweenPandPlane = 90.0f - Vector3.Angle(p, planeNormal);
        var closestDistanceToPlane = ClosestDistanceBetween(angleBetweenPandPlane, p);
        var distanceToCollision = DistanceToCollisionPos(closestDistanceToPlane);
        if (distanceToCollision < float.Epsilon) distanceToCollision = 0.0f;
        return distanceToCollision;
    }
    
    private static float ClosestDistanceBetween(float q2, Vector3 p)
    {
        return Mathf.Sin(q2 * Mathf.Deg2Rad) * Vector3.Magnitude(p);
    }

    private float DistanceToCollisionPos(float closestDistanceFromSphereToCollision)
    {
        return (closestDistanceFromSphereToCollision - gameObject.Radius()) / Mathf.Cos(Vector3.Angle(thisSphere.velocity, -planeNormal) * Mathf.Deg2Rad);
    }
    
    private Vector3 BounceVelocity()
    {
        var bounceUnitVector = 2 * planeNormal * Vector3.Dot(planeNormal, -thisSphere.velocity * Time.fixedDeltaTime)
                               + thisSphere.velocity * Time.fixedDeltaTime;
        return bounceUnitVector * (thisSphere.velocity.magnitude * Time.fixedDeltaTime);
    }
}
