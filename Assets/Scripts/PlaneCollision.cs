using UnityEngine;

public class PlaneCollision : MonoBehaviour
{
    public GameObject planeToCollideWith;
    private float r;
    private Vector3 v;
    private Vector3 n;

    private void Start()
    {
        r = 0.5f;
        v = GetComponent<EulerTrajectory>().velocity;
        var planePos = planeToCollideWith.transform.position;
        var a = new Vector3(planePos.x + 10, planePos.y, planePos.z + 10);
        var b = new Vector3(planePos.x - 10, planePos.y, planePos.z - 10);
        var c = new Vector3(planePos.x + 18, planePos.y, planePos.z + 15);
        n = Vector3.Cross(b - a, c - a).normalized * -1;
    }

    void Update()
    {
        if (!SphereIsHeadingTowardPlane()) return;
        var p = transform.position - planeToCollideWith.transform.position;
        var q2 = 90.0f - Angle(p, n);
        var d = ClosestDistanceBetweenSphereAndPlane(q2, p);
        var vc = DistanceFromSphereToCollisionPos(d);
        if (CollisionDistanceIsLessThanLengthOfV(vc)) { GetComponent<EulerTrajectory>().velocity = new Vector3(0, 0, 0); }
    }

    private bool SphereIsHeadingTowardPlane() { return Vector3.Angle(n, -v) <= 90.0f; }

    private static float Angle(Vector3 vector1, Vector3 vector2)
    {
        return Mathf.Acos(Vector3.Dot(vector1, vector2) / (Vector3.Magnitude(vector1) * Vector3.Magnitude(vector2))) * Mathf.Rad2Deg;
    }
    
    private static float ClosestDistanceBetweenSphereAndPlane(float q2, Vector3 p)
    {
        return Mathf.Sin(q2 * Mathf.Deg2Rad) * Vector3.Magnitude(p);
    }

    private double DistanceFromSphereToCollisionPos(double closestDistanceFromSphereToCollision)
    {
        return (closestDistanceFromSphereToCollision - r) / Mathf.Cos(Angle(v, -n) * Mathf.Deg2Rad);
    }

    private bool CollisionDistanceIsLessThanLengthOfV(double collisionDistance) { return collisionDistance <= r; }
}
