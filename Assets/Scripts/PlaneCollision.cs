using UnityEngine;

public class PlaneCollision : Collision
{
    public GameObject planeToCollideWith;
    public Trajectory trajectory;
    private float r;
    private Vector3 n;

    private void Start()
    {
        r = gameObject.Radius();
        var planePos = planeToCollideWith.transform.position;
        var a = new Vector3(planePos.x + 10, planePos.y, planePos.z + 10);
        var b = new Vector3(planePos.x - 10, planePos.y, planePos.z - 10);
        var c = new Vector3(planePos.x + 18, planePos.y, planePos.z + 15);
        n = Vector3.Cross(b - a, c - a).normalized * -1;
    }

    public override float CheckForCollision()
    {
        if (trajectory.velocity == Vector3.zero) return 0.0f;
        if (!SphereIsHeadingTowardPlane()) return 1.0f;
        var p = transform.position - planeToCollideWith.transform.position;
        var q2 = 90.0f - Angle(p, n);
        var d = ClosestDistanceBetweenSphereAndPlane(q2, p);
        var vc = DistanceFromSphereToCollisionPos(d);
        if (vc < float.Epsilon) vc = 0.0f;
        if (HasCollided(vc)) { return vc / (trajectory.velocity.magnitude * Time.deltaTime); }
        return 1.0f;
    }

    private bool SphereIsHeadingTowardPlane() { return Vector3.Angle(n, -trajectory.velocity) <= 90.0f; }

    private static float Angle(Vector3 vector1, Vector3 vector2)
    {
        return Mathf.Acos(Vector3.Dot(vector1, vector2) / (Vector3.Magnitude(vector1) * Vector3.Magnitude(vector2))) * Mathf.Rad2Deg;
    }
    
    private static float ClosestDistanceBetweenSphereAndPlane(float q2, Vector3 p)
    {
        return Mathf.Sin(q2 * Mathf.Deg2Rad) * Vector3.Magnitude(p);
    }

    private float DistanceFromSphereToCollisionPos(float closestDistanceFromSphereToCollision)
    {
        return (closestDistanceFromSphereToCollision - r) / Mathf.Cos(Angle(trajectory.velocity, -n) * Mathf.Deg2Rad);
    }

    private bool HasCollided(double distanceToCollision) { return distanceToCollision <= trajectory.velocity.magnitude * Time.deltaTime; }
}
