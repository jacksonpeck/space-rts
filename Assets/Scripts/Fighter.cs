using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter: Ship
{
    [SerializeField] private float _thrustPrimary;
    [SerializeField] private float _thrustSecondary;
    [SerializeField] private float _torque;
    [SerializeField] private float _dodgeImpulse;
    [SerializeField] private float _dodgeRate;
    [SerializeField] private float _dodgeDelay;
    [SerializeField] private float _fireImpulse;
    [SerializeField] private float _fireDelay;
    [SerializeField] private float _speedMin;
    [SerializeField] private GameObject _missile;
    [SerializeField] private GameObject _afterburner;
    [SerializeField] private Scanner _scanner;

    public Ship Target;

    private Rigidbody2D _rigidbody;
    private float _turnTime;
    private float _inverseThrustPrimary;
    private float _inverseThrustSecondary;
    private float _inverseFireSpeed;
    private float _dodgeBuffer;
    private float _fireBuffer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        
        _turnTime = 180f / _torque;
        _inverseThrustPrimary = 1.0f / _thrustPrimary;
        _inverseThrustSecondary = 1.0f / _thrustSecondary;
        _inverseFireSpeed = 1.0f / _fireImpulse;

        _dodgeBuffer = -1.0f;
        _fireBuffer = -1.0f;
    }

    private void FixedUpdate()
    {
        if (0.0f <= _fireBuffer) _fireBuffer -= Time.deltaTime;
        if (0.0f <= _dodgeBuffer) _dodgeBuffer -= Time.deltaTime;

        if (_scanner.Enemies.Count > 0)
        {
            Target = _scanner.Enemies[0];
        }

        bool attack = Target != null;

        Vector2 difference;
        Vector2 relativeVelocity;

        if (attack)
        {
            if (_dodgeBuffer < 0.0f && Random.value < _dodgeRate * Time.deltaTime)
            {
                _dodgeBuffer = _dodgeDelay;
                Velocity += (Vector2)(Quaternion.Euler(0.0f, 0.0f, Random.Range(0.0f, 360.0f)) * (_dodgeImpulse * Vector2.up));
            }

            difference = (Vector2)Target.transform.position - _rigidbody.position;
            relativeVelocity = Velocity - Target.Velocity;
        }
        else
        {
            difference = Destination - _rigidbody.position;
            relativeVelocity = Velocity;
        }
        
        float distance = difference.magnitude;
        float speed = Velocity.magnitude;

        bool moving = speed > _speedMin;
        bool nearTarget = distance < 2f;
        bool onTarget = distance < 0.5f;

        Vector2 direction;

        if (attack)
        {
            direction = (difference - (distance * _inverseFireSpeed * relativeVelocity)).normalized;

            float divisor = Vector2.Dot(relativeVelocity, direction) + _fireImpulse;

            if (divisor < 0.0f)
            {
                direction = -relativeVelocity;
            }
            else
            {
                float time = distance / divisor;
                direction = (difference - time * relativeVelocity).normalized;
            }
        }
        else if (onTarget)
        {
            Velocity = Vector2.MoveTowards(Velocity, Vector2.zero, Time.deltaTime * _thrustSecondary);
            direction = Direction.normalized;
            nearTarget = true;
        }
        else
        {
            float stopDistance = speed * (speed * _inverseThrustSecondary * 0.5f);
            Vector2 stopPosition = stopDistance * relativeVelocity.normalized;
            Vector2 stopDifference = difference - stopPosition;
            direction = stopDifference.normalized;

            if (nearTarget || stopDifference.magnitude < 0.5f)
            {
                Velocity += Time.deltaTime * _thrustSecondary * direction;
                direction = Direction.normalized;
                nearTarget = true;
            }
            else
            {
                // Velocity += Time.deltaTime * _thrustSecondary * direction;
            }
        }
        // else if (nearTarget)
        // {
        //     direction = Direction.normalized;
        //     float stopDistance = speed * (speed * _inverseThrustSecondary + 0.5f);
        //     Vector2 stopPosition = stopDistance * relativeVelocity.normalized;
        //     Vector2 stopDifference = difference - stopPosition;
        //     Velocity += Time.deltaTime * _thrustSecondary * stopDifference.normalized;
        // }
        // else
        // {
        //     float stopDistance = speed * (_turnTime + speed * _inverseThrustPrimary + 0.5f);
        //     // float stopDistance = speed * (speed * _inverseThrustSecondary + 0.5f);
        //     Vector2 stopPosition = stopDistance * relativeVelocity.normalized;
        //     Vector2 stopDifference = difference - stopPosition;
        //     direction = stopDifference.normalized;
        //     Velocity += Time.deltaTime * _thrustSecondary * direction;
        // }
        
        float rotation = Time.deltaTime * _torque;
        float angle = Vector2.Angle(this.transform.up, direction);

        bool lookTarget = angle < rotation;

        if (!lookTarget)
        {
            if (0f < Vector2.Dot(this.transform.right, direction))
                rotation *= -1f;

            _rigidbody.MoveRotation(_rigidbody.rotation + rotation);
        }

        if (lookTarget && (attack || !nearTarget))
        {
            Velocity += Time.deltaTime * _thrustPrimary * (Vector2)this.transform.up;
            _afterburner.SetActive(true);

            if (attack && _fireBuffer < 0.0f)
            {
                _fireBuffer = _fireDelay;
                GameObject missile = Instantiate(_missile, this.transform.position, this.transform.rotation);
                Missile component = missile.GetComponent<Missile>();
                component.Team = Team;
                component.Velocity = Velocity + _fireImpulse * (Vector2)missile.transform.up;
                Destroy(missile, 5.0f);
            }
        }
        else
        {
            _afterburner.SetActive(false);
        }

        _rigidbody.MovePosition(_rigidbody.position + Time.deltaTime * Velocity);
    }
}
