﻿using UnityEngine;

public abstract class Trajectory : MonoBehaviour
{
    public Vector3 velocity = new Vector3(10, 10, 10);
    public Vector3 acceleration = new Vector3(0, -9.8f, 0);
}