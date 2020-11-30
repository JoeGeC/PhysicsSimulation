using UnityEngine;

public class EulerTrajectory : Trajectory
{
    private float velocityModifier = 1.0f;
    private Collision sphereCollider;
    private Collision planeCollider;

    void Start()
    {
        sphereCollider = GetComponent<SphereCollision>();
        planeCollider = GetComponent<PlaneCollision>();
    }
    
    private void Update()
    {
        if(sphereCollider) velocityModifier = sphereCollider.CheckForCollision();
        if(planeCollider) velocityModifier = planeCollider.CheckForCollision();
        velocity += acceleration * Time.deltaTime;
        velocity *= velocityModifier;
        transform.position += velocity * Time.deltaTime;
    }
}