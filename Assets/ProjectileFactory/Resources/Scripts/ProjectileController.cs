using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileController : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] public ParticleSystem _projectileParticles;
    [SerializeField] AudioClip _projectileFire;
    [SerializeField] public ParticleSystem _impactParticles;
    [SerializeField] AudioClip _impactSound;

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

        if (_impactSound != null)
        {
            AudioHelper.PlayClip2D(_projectileFire, 1f);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    protected void Move()
    {
        Vector3 moveOffset = transform.TransformDirection(Vector3.forward) * _speed * Time.fixedDeltaTime;
        _rb.MovePosition(_rb.position + moveOffset);
    }

    void OnCollisionEnter(Collision other)
    {
        ProjectileController otherProjectile = other.gameObject.GetComponent<ProjectileController>();

        if (otherProjectile != null)
        {
            Physics.IgnoreCollision(otherProjectile.GetComponent<Collider>(), GetComponent<Collider>());
            return;
        }

        ImpactFeedback(Quaternion.LookRotation(other.contacts[0].normal));

        Destroy(gameObject);
    }

    void ImpactFeedback(Quaternion impactRotation)
    {
        if (_impactParticles != null)
        {
            _impactParticles = Instantiate(_impactParticles, transform.position, impactRotation);
            _impactParticles.Play();
        }

        if (_impactSound != null)
        {
            AudioHelper.PlayClip2D(_impactSound, 1f);
        }
    }
}
