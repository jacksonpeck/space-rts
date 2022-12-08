using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    public int Team;
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
        // Velocity += Time.deltaTime * Thrust * (Vector2)this.transform.up;
        _rigidbody.MovePosition(_rigidbody.position + Time.deltaTime * Velocity);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Fighter" && other.GetComponent<Fighter>().Team != Team)
        {
            Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);

            Destroy(other.gameObject);
            Destroy(this.gameObject);

            return;
        }
        if (other.tag == "Projectile" && other.GetComponent<Missile>().Team != Team)
        {
            Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);

            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }
    }
}
