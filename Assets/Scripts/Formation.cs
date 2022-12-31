using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Formation
{
    private static Formation _instance;
    public static Formation Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new Formation();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    // public void SetDestination(Vector2 destination, Vector2 direction, HashSet<Selectable> selected)
    // {
    //     if (selected.Count <= 0) return;

    //     List<Selectable> units = new List<Selectable>();
    //     foreach (Selectable s in selected)
    //     {
    //         s.KillDeadNodes();
    //         units.Add(s);
    //     }

    //     if (direction == new Vector2())
    //     {
    //         if (units[0].Nodes.Count == 0)
    //         {
    //             direction = (destination - (Vector2)units[0].transform.position).normalized;
    //         }
    //         else
    //         {
    //             direction = (destination - (Vector2)units[0].Nodes[0].transform.position).normalized;
    //         }
    //     }

    //     bool left = true;
    //     Vector2 orthogonal = 12.0f * (Quaternion.AngleAxis(90f, Vector3.forward) * direction.normalized);
    //     Vector2 offset = orthogonal;
    //     Vector2 directionNormal = direction.normalized;

    //     units[0].setDestination(destination, directionNormal);

    //     for (int i = 1; i < selected.Count; i++)
    //     {
    //         Selectable unit = units[i];
    //         if (left)
    //         {
    //             unit.setDestination(destination + offset, directionNormal);
    //         }
    //         else
    //         {
    //             unit.setDestination(destination - offset, directionNormal);
    //             offset += orthogonal;
    //         }
    //         left = !left;
    //     }
    // }

    // public void SetDestination(Vector2 destination, Vector2 direction, HashSet<Selectable> selected)
    // {
    //     if (selected.Count <= 0) return;

    //     List<Selectable> units = new List<Selectable>();
    //     foreach (Selectable s in selected)
    //     {
    //         units.Add(s);
    //     }

    //     if (direction == new Vector2())
    //     {
    //         direction = (destination - (Vector2)units[0].transform.position).normalized;
    //     }

    //     units[0].Destination = destination;
    //     units[0].Direction = direction.normalized;

    //     bool left = true;
    //     Vector2 rowPosition = destination - 2 * direction.normalized;
    //     Vector2 orthogonal = 2 * (Quaternion.AngleAxis(90f, Vector3.forward) * direction.normalized);
    //     Vector2 offset = orthogonal;

    //     for (int i = 1; i < selected.Count; i++)
    //     {
    //         Selectable unit = units[i];
    //         unit.Direction = direction.normalized;
    //         if (left)
    //         {
    //             unit.Destination = rowPosition + offset;
    //         }
    //         else
    //         {
    //             unit.Destination = rowPosition - offset;
    //             rowPosition -= 2 * direction;
    //             offset += orthogonal;
    //         }
    //         left = !left;
    //     }
    // }

    // public void SetDestination(Vector2 destination, Vector2 direction, List<SelectableNode> selected)
    // {
    //     if (selected.Count <= 0) return;

    //     List<SelectableNode> units = new List<SelectableNode>();
    //     foreach (SelectableNode s in selected)
    //     {
    //         units.Add(s);
    //     }

    //     if (direction == new Vector2())
    //     {
    //         direction = (destination - (Vector2)units[0].transform.position).normalized;
    //     }

    //     bool left = true;
    //     Vector2 rowPosition = destination - 2 * direction.normalized;
    //     Vector2 orthogonal = 2 * (Quaternion.AngleAxis(90f, Vector3.forward) * direction.normalized);
    //     Vector2 offset = orthogonal;
    //     Vector2 directionNormal = direction.normalized;

    //     units[0].setDestination(destination, directionNormal);

    //     for (int i = 1; i < selected.Count; i++)
    //     {
    //         SelectableNode unit = units[i];
    //         if (left)
    //         {
    //             unit.setDestination(rowPosition + offset, directionNormal);
    //         }
    //         else
    //         {
    //             unit.setDestination(rowPosition - offset, directionNormal);
    //             rowPosition -= 2 * direction;
    //             offset += orthogonal;
    //         }
    //         left = !left;
    //     }
    // }

    // public void SetDestination(Vector2 destination, Vector2 direction, HashSet<Selectable> selected)
    // {
    //     if (selected.Count <= 0) return;

    //     List<Selectable> units = new List<Selectable>();
    //     foreach (Selectable s in selected)
    //     {
    //         units.Add(s);
    //     }

    //     if (direction == new Vector2())
    //     {
    //         direction = (destination - (Vector2)units[0].transform.position).normalized;
    //     }

    //     Vector2 rowPosition = destination;
    //     Vector2 orthogonal = 2 * (Quaternion.AngleAxis(-90f, Vector3.forward) * direction.normalized);
    //     int rowWidth = 1;
    //     int rowIndex = 1;

    //     foreach (Selectable s in units)
    //     {
    //         if (rowIndex <= 0)
    //         {
    //             rowWidth += 2;
    //             rowIndex = rowWidth;
    //             rowPosition -= 2 * direction.normalized;
    //         }
    //         s.Destination = rowIndex % 2 == 0 ? rowPosition + rowIndex / 2 * -orthogonal
    //             : rowPosition + rowIndex / 2 * orthogonal;
    //         rowIndex--;
    //     }
    // }
}
