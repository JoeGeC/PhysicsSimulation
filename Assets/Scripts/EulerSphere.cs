using UnityEngine;

public class EulerSphere : Sphere
{
    private SphereCollision sphereCollider;
    private PlaneCollision planeCollider;
    public EntityHandler entityHandler;

    protected override void Start()
    {
        base.Start();
        sphereCollider = gameObject.AddComponent<SphereCollision>();
        planeCollider = gameObject.AddComponent<PlaneCollision>();
    }
    
    private void FixedUpdate()
    {
        foreach(var sphere in entityHandler.spheres) sphereCollider.CheckForCollision(sphere);
        foreach(var plane in entityHandler.planes) planeCollider.CheckForCollision(plane);
        if(Moving()) velocity += Acceleration * Time.fixedDeltaTime;
        transform.position += velocity * Time.fixedDeltaTime;
    }
}