using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public int Team;
    public int Damage;
    public float Thrust;
    public Vector2 Velocity;

    [SerializeField] GameObject _explosionPrefab;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Velocity += Time.deltaTime * Thrust * (Vector2)this.transform.up;
        _rigidbody.MovePosition(_rigidbody.position + Time.deltaTime * Velocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ship ship = other.GetComponent<Ship>();

        if (ship != null && ship.Team != this.Team)
        {
            ship.Damage(Damage);
            
            GameObject explosion = Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);
            explosion.GetComponent<Explosion>().Velocity = ship.Velocity;

            Destroy(this.gameObject);
        }
    }
}
