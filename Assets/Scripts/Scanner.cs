using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Ship _ship;

    public List<Ship> Enemies = new List<Ship>();
    
    // private void FixedUpdate()
    // {
    //     if (_ship.Target == null && _enemies.Count > 0)
    //     {
    //         List<Fighter> toRemove = new List<Fighter>();
    //         foreach (Fighter f in _enemies)
    //         {
    //             if (f == null)
    //             {
    //                 toRemove.Add(f);
    //             }
    //         }
    //         foreach (Fighter f in toRemove)
    //         {
    //             _enemies.Remove(f);
    //         }
    //         if (_enemies.Count > 0)
    //         {
    //             _ship.Target = _enemies[0];
    //         }
    //     }
    // }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Ship enemy = other.GetComponent<Ship>();
        if (enemy != null && enemy.Team != _ship.Team)
        {
            Enemies.Add(other.GetComponent<Ship>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Ship enemy = other.GetComponent<Ship>();
        if (enemy != null && enemy.Team != _ship.Team)
        {
            Enemies.Remove(other.GetComponent<Ship>());
        }
    }
}
