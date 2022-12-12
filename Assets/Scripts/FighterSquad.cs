using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSquad : Selectable
{
    // private Vector2 _destination;
    // private Vector2 _direction;

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

    public override void setDestination(Vector2 destination, Vector2 direction)
    {
        base.setDestination(destination, direction);

        Formation.Instance.SetDestination(destination, direction, Nodes);
    }
}
