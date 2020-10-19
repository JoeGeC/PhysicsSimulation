﻿using UnityEngine;

public class EulerTrajectory : MonoBehaviour
{
    private const float TimeStep = 1.0f / 60.0f;
    public Vector3 velocity = new Vector3(10, 10, 10);
    public Vector3 acceleration = new Vector3(0, -9.8f, 0);

    private void Update()
    {
        velocity += acceleration * TimeStep;
        transform.position += velocity * TimeStep;
    }
}