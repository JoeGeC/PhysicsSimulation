using UnityEngine;

public class EulerTrajectory : Trajectory
{
    private Collision sphereCollider;
    private Collision planeCollider;

    void Start()
    {
        sphereCollider = GetComponent<SphereCollision>();
        planeCollider = GetComponent<PlaneCollision>();
    }
    
    private void Update()
    {
        if(sphereCollider) sphereCollider.CheckForCollision();
        if(planeCollider) planeCollider.CheckForCollision();
        if(Moving()) velocity += acceleration * Time.deltaTime;
        transform.position += velocity * Time.deltaTime;
    }
}