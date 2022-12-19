using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public int Team;
    public Vector2 Velocity;
    public Ship Target;

    [SerializeField] private GameObject _projectilePrefab;
    
    [SerializeField] private float _fireImpulse;
    [SerializeField] private float _fireDelay;
    [SerializeField] private float _torque;

    private float _fireBuffer;
    private float _inverseFireSpeed;

    private void Start()
    {
        _inverseFireSpeed = 1.0f / _fireImpulse;
        _fireBuffer = -1.0f;
    }

    private void FixedUpdate()
    {
        if (0.0f <= _fireBuffer) _fireBuffer -= Time.deltaTime;
        
        bool attack = Target != null;

        Vector2 direction;

        if (attack)
        {
            Vector2 difference = (Vector2)Target.transform.position - (Vector2)this.transform.position;
            Vector2 relativeVelocity = Velocity - Target.Velocity;
            float distance = difference.magnitude;
            direction = (difference - (distance * _inverseFireSpeed * relativeVelocity)).normalized;
        }
        else
        {
            direction = Vector3.up;
        }

        float rotation = Time.deltaTime * _torque;
        float angle = Vector2.Angle(this.transform.up, direction);

        bool lookTarget = angle < rotation;

        if (!lookTarget)
        {
            if (0f < Vector2.Dot(this.transform.right, direction))
                rotation *= -1f;

            this.transform.Rotate(Vector3.forward, rotation);
        }

        if (attack && lookTarget)
        {
            if (attack && _fireBuffer < 0.0f)
            {
                _fireBuffer = _fireDelay;
                GameObject projectile = Instantiate(_projectilePrefab, this.transform.position, this.transform.rotation);
                Missile component = projectile.GetComponent<Missile>();
                component.Team = Team;
                component.Velocity = Velocity + _fireImpulse * (Vector2)projectile.transform.up;
                Destroy(projectile, 5.0f);
            }
        }
    }
}
