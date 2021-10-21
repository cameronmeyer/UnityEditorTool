using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(fileName = "New Projectile Data", menuName = "Projectile Data")]
public class ProjectileData : ScriptableObject
{
    public enum projectileModel
    {
        Sphere,
        Capsule,
        Cube,
        None
    }

    public string name;
    public projectileModel model;
    public bool containsParticles;
    public Material particleMaterial;
    public bool containsTrail;
    public Material trailMaterial;
    public bool containsImpactParticles;
    public Material impactParticleMaterial;
    public float speed;
}
