using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squad : MonoBehaviour
{
    private Selectable _selectable;

    private Vector2 _destination;
    private Vector2 _direction;

    private void Awake()
    {
        _selectable = GetComponent<Selectable>();
    }

    private void FixedUpdate()
    {
        if (_destination == _selectable.Destination)
        {
            return;
        }

        _destination = _selectable.Destination;
        _direction = _selectable.Direction;

        Formation.Instance.SetDestination(_destination, _direction, _selectable.Nodes);
    }
}

// public class Squad : MonoBehaviour
// {
//     public List<SelectableNode> Units;

//     private bool isSelected = false;
//     private bool isHovered = false;

//     private void FixedUpdate()
//     {
//         List<Selectable> deadUnits = new List<Selectable>();

//         bool selectAll = false;
//         bool deselectAll = false;

//         foreach (Selectable unit in Units)
//         {
//             if (unit == null)
//             {
//                 deadUnits.Add(unit);
//                 continue;
//             }
            
//             if (isSelected)
//             {
//                 if (!unit.isSelected)
//                 {
//                     deselectAll = true;
//                     isSelected = false;
//                     break;
//                 }
//             }
//             else if (unit.isSelected)
//             {
//                 selectAll = true;
//                 isSelected = true;
//                 break;
//             }
//         }

//         bool hoverAll = false;
//         bool unhoverAll = false;

//         foreach (Selectable unit in deadUnits)
//         {
//             Units.Remove(unit);
//         }

//         foreach (Selectable unit in Units)
//         {
//             if (isHovered)
//             {
//                 if (!unit.isHovered)
//                 {
//                     unhoverAll = true;
//                     isHovered = false;
//                     break;
//                 }
//             }
//             else if (unit.isHovered)
//             {
//                 hoverAll = true;
//                 isHovered = true;
//                 break;
//             }
//         }

//         SelectionManager selectionManager = SelectionManager.Instance;

//         if (selectAll)
//         {
//             foreach (Selectable unit in Units)
//             {
//                 selectionManager.Select(unit);
//             }
//         }
//         if (deselectAll)
//         {
//             foreach (Selectable unit in Units)
//             {
//                 selectionManager.Deselect(unit);
//             }
//         }

//         if (hoverAll)
//         {
//             foreach (Selectable unit in Units)
//             {
//                 selectionManager.Hover(unit);
//             }
//         }
//         if (unhoverAll)
//         {
//             foreach (Selectable unit in Units)
//             {
//                 selectionManager.Unhover(unit);
//             }
//         }
//     }
// }
