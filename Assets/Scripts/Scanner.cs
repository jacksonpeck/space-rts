using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scanner : MonoBehaviour
{
    [SerializeField] private Fighter _ship;

    private List<Fighter> _enemies = new List<Fighter>();
    
    private void FixedUpdate()
    {
        if (_ship.Target == null && _enemies.Count > 0)
        {
            List<Fighter> toRemove = new List<Fighter>();
            foreach (Fighter f in _enemies)
            {
                if (f == null)
                {
                    toRemove.Add(f);
                }
            }
            foreach (Fighter f in toRemove)
            {
                _enemies.Remove(f);
            }
            if (_enemies.Count > 0)
            {
                _ship.Target = _enemies[0];
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Fighter" && other.GetComponent<Fighter>().Team != _ship.Team)
        {
            _enemies.Add(other.GetComponent<Fighter>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Fighter" && other.GetComponent<Fighter>().Team != _ship.Team)
        {
            _enemies.Remove(other.GetComponent<Fighter>());
        }
    }

}
