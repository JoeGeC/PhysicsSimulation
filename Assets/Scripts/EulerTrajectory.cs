public class EulerTrajectory : Trajectory
{
    private const float TimeStep = 1.0f / 60.0f;

    private void Update()
    {
        velocity += acceleration * TimeStep;
        transform.position += velocity * TimeStep;
    }
}