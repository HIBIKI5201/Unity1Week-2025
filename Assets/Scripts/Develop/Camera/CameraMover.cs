using NUnit.Framework.Constraints;
using System;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Vector3 ScrollVelocity { get; private set; }
    [SerializeField] private Vector3 _scrollDirection = Vector3.forward;
    [SerializeField] private float _moveSpeed = 2f;

    void Update()
    {
        ScrollVelocity = _scrollDirection.normalized * _moveSpeed;
        transform.position += ScrollVelocity * Time.deltaTime;
    }
}
