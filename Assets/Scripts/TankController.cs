using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TankController : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 0.25f;

    [SerializeField] GameObject _projectileSpawn;
    [SerializeField] GameObject _projectile;
    [SerializeField] float _fireDelay = 0.2f;
    private bool _canFire = false;
    private float _timeLastFired = 0f;
    [SerializeField] ParticleSystem _muzzleFlash;

    [SerializeField] GameObject _base;
    [SerializeField] GameObject _turret;
    [SerializeField] GameObject _turretPivot;

    [SerializeField] GameObject _ground;

    public float MoveSpeed
    {
        get => _moveSpeed;
        set => _moveSpeed = value;
    }

    Rigidbody _rb = null;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Exit();
        Reset();
        Fire();
    }

    private void FixedUpdate()
    {
        MoveTank();
        TurnTurret();
    }

    public void MoveTank()
    {
        if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
        {
            Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
            transform.position += move * _moveSpeed;

            _base.transform.rotation = Quaternion.LookRotation(move);
        }
    }

    private Vector3 temp;

    public void TurnTurret()
    {
        float distanceToTank = Vector3.Distance(Camera.main.transform.position, _turret.transform.position); 
        Vector3 cameraPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distanceToTank);   
        Vector3 localPoint = transform.InverseTransformPoint(Camera.main.ScreenToWorldPoint(cameraPoint));
        localPoint.y = _turret.transform.localPosition.y;

        _turretPivot.transform.rotation = Quaternion.LookRotation(localPoint, Vector3.up);
    }

    public void Fire()
    {
        //if (_canFire)
        //{
            if (Input.GetKeyUp("space") || Input.GetMouseButtonUp(0))
            {
                //_canFire = false;
                _timeLastFired = Time.time;

                GameObject projectile = Instantiate(_projectile, _projectileSpawn.transform.position, _projectileSpawn.transform.rotation);
                ProjectileController pProjectile = projectile.GetComponent<ProjectileController>();

                _muzzleFlash.Play();

                Physics.IgnoreCollision(projectile.GetComponent<Collider>(), GetComponent<Collider>());
                Physics.IgnoreCollision(projectile.GetComponent<Collider>(), _ground.GetComponent<Collider>());
            }
        //}
    }

    public void Exit()
    {
        if (Input.GetKeyDown("escape"))
        {
            Debug.Log("Quitting Game");
            Application.Quit();
        }
    }

    public void Reset()
    {
        if(Input.GetKeyDown("backspace"))
        {
            Debug.Log("Reset Scene");
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
