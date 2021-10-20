using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : MonoBehaviour
{
    private float _speed;
    //private float _damage;

    public float Speed
    {
        get => _speed;
        set => _speed = value;
    }

    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _rb.useGravity = false;
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected void Move()
    {
        Vector3 moveOffset = Vector3.forward * _speed;
        _rb.MovePosition(_rb.position + moveOffset);
    }
}
