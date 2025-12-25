using NUnit.Framework.Constraints;
using System;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Vector3 ScrollVelocity { get; private set; }
    [SerializeField] private float _moveSpeed = 2f;

    void Update()
    {
        ScrollVelocity = new Vector3(0f, 0f, _moveSpeed);
        transform.position += ScrollVelocity * Time.deltaTime;
    }
}
