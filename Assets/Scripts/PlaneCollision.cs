using UnityEngine;

public class PlaneCollision : Collision
{
    public GameObject planeToCollideWith;
    public Trajectory trajectory;
    private Vector3 planeNormal;

    private void Start()
    {
        var planePos = planeToCollideWith.transform.position;
        var a = new Vector3(planePos.x + 10, planePos.y, planePos.z + 10);
        var b = new Vector3(planePos.x - 10, planePos.y, planePos.z - 10);
        var c = new Vector3(planePos.x + 18, planePos.y, planePos.z + 15);
        planeNormal = Vector3.Cross(b - a, c - a).normalized * -1;
    }

    public override void CheckForCollision()
    {
        if (!trajectory.Moving()) return;
        if (!HeadingTowardPlane()) return;
        if (!HasCollided(DistanceToCollision())) return;
        trajectory.velocity = BounceVelocity();
    }

    private bool HeadingTowardPlane() { return Vector3.Angle(planeNormal, -trajectory.velocity) <= 90.0f; }

    private static float Angle(Vector3 vector1, Vector3 vector2)
    {
        return Mathf.Acos(Vector3.Dot(vector1, vector2) / (Vector3.Magnitude(vector1) * Vector3.Magnitude(vector2))) * Mathf.Rad2Deg;
    }
    
    private static float ClosestDistanceBetween(float q2, Vector3 p)
    {
        return Mathf.Sin(q2 * Mathf.Deg2Rad) * Vector3.Magnitude(p);
    }

    private float DistanceToCollisionPos(float closestDistanceFromSphereToCollision)
    {
        return (closestDistanceFromSphereToCollision - gameObject.Radius()) / Mathf.Cos(Angle(trajectory.velocity, -planeNormal) * Mathf.Deg2Rad);
    }

    private bool HasCollided(double distanceToCollision) { return distanceToCollision <= trajectory.velocity.magnitude * Time.deltaTime; }

    private float DistanceToCollision()
    {
        var p = transform.position - planeToCollideWith.transform.position;
        var angleBetweenPandPlane = 90.0f - Angle(p, planeNormal);
        var closestDistanceToPlane = ClosestDistanceBetween(angleBetweenPandPlane, p);
        var distanceToCollision = DistanceToCollisionPos(closestDistanceToPlane);
        if (distanceToCollision < float.Epsilon) distanceToCollision = 0.0f;
        return distanceToCollision;
    }
    
    private Vector3 BounceVelocity()
    {
        var bounceUnitVector = 2 * planeNormal * Vector3.Dot(planeNormal, -trajectory.velocity * Time.deltaTime) +
                               trajectory.velocity * Time.deltaTime;
        return bounceUnitVector * (trajectory.velocity * Time.deltaTime).magnitude;
    }
}
