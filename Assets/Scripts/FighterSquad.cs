using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSquad : Selectable
{
    // private Scanner _scanner;
    private Ship _target;

    public void Awake()
    {
        // _scanner = Nodes[0].GetComponent<Scanner
    }

    // private void FixedUpdate()
    // {
    //     if (_destination == Destination)
    //     {
    //         return;
    //     }

    //     _destination = Destination;
    //     _direction = Direction;

    //     Formation.Instance.SetDestination(_destination, _direction, Nodes);
    // }
    // private void FixedUpdate()
    // {
    //     if (Nodes.Count <= 0)
    //     {
    //         Destroy(this);
    //     }
    //     if (Target == null && )
    //     {
            
    //     }
    // }

    public override void setDestination(Vector2 destination, Vector2 direction)
    {
        base.setDestination(destination, direction);

        if (Nodes.Count <= 0) return;

        List<Selectable> units = new List<Selectable>();
        foreach (Selectable s in Nodes)
        {
            units.Add(s);
        }

        if (direction == new Vector2())
        {
            direction = (destination - (Vector2)units[0].transform.position).normalized;
        }

        bool left = true;
        Vector2 rowPosition = destination - 2 * direction.normalized;
        Vector2 orthogonal = 2 * (Quaternion.AngleAxis(90f, Vector3.forward) * direction.normalized);
        Vector2 offset = orthogonal;
        Vector2 directionNormal = direction.normalized;

        units[0].setDestination(destination, directionNormal);

        for (int i = 1; i < Nodes.Count; i++)
        {
            Selectable unit = units[i];
            if (left)
            {
                unit.setDestination(rowPosition + offset, directionNormal);
            }
            else
            {
                unit.setDestination(rowPosition - offset, directionNormal);
                rowPosition -= 2 * direction;
                offset += orthogonal;
            }
            left = !left;
        }
    }
}
