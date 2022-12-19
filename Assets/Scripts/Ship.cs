using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : Selectable
{
    public int Team;
    public int Health;
    public Vector2 Velocity;

    public virtual void Damage(int damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            this.Kill();
        }
    }

    public virtual void Kill()
    {
        Destroy(this.gameObject);
    }
}
