using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fighter : SelectableNode
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
    [SerializeField] private SpriteRenderer _afterburner;

    public int Team;
    public Vector2 Velocity;
    public Fighter Target;

    private Rigidbody2D _rigidbody;
    private float _turnTime;
    private float _inverseThrust;
    private float _inverseFireSpeed;
    private float _dodgeBuffer;
    private float _fireBuffer;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        
        _turnTime = 180f / _torque;
        _inverseThrust = 1.0f / _thrustPrimary;
        _inverseFireSpeed = 1.0f / _fireImpulse;

        _dodgeBuffer = -1.0f;
        _fireBuffer = -1.0f;
    }

    private void FixedUpdate()
    {
        if (0.0f <= _fireBuffer) _fireBuffer -= Time.deltaTime;
        if (0.0f <= _dodgeBuffer) _dodgeBuffer -= Time.deltaTime;

        bool attack = Target != null;

        Vector2 difference;
        Vector2 relativeVelocity;

        if (attack)
        {
            if (_dodgeBuffer < 0.0f && Random.value < _dodgeRate * Time.deltaTime)
            {
                Debug.Log("Dodged");
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
        bool onTarget = distance < 1f;

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
        }
        else
        {
            float stopDistance = speed * (_turnTime + speed * _inverseThrust + 0.5f);
            Vector2 stopPosition = stopDistance * relativeVelocity.normalized;
            Vector2 stopDifference = difference - stopPosition;
            direction = stopDifference.normalized;

            // float deccelerationTime = speed * _inverseThrust;
            // float deccelerationDistance = 0.5f * speed;
            // // float deccelerationDistance = speed - 0.5f * _thrustPrimary * deccelerationTime;
            // float totalStopTime = _turnTime + deccelerationTime;
            // float stopDistance = speed * totalStopTime + deccelerationDistance;
            // Vector2 stopPosition = stopDistance * Velocity.normalized;
        }
        
        float rotation = Time.deltaTime * _torque;
        float angle = Vector2.Angle(this.transform.up, direction);

        bool lookTarget = angle < rotation;

        if (lookTarget && (attack || !onTarget))
        {
            Velocity += Time.deltaTime * _thrustPrimary * (Vector2)this.transform.up;
            _afterburner.enabled = true;

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
            _afterburner.enabled = false;
        }

        if (!lookTarget)
        {
            if (0f < Vector2.Dot(this.transform.right, direction))
                rotation *= -1f;

            _rigidbody.MoveRotation(_rigidbody.rotation + rotation);
        }

        _rigidbody.MovePosition(_rigidbody.position + Time.deltaTime * Velocity);
    }
}
