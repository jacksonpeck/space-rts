using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterSquad : Selectable
{
    private Ship _target;

    public override void StartTask()
    {
        if (Nodes.Count <= 0) return;

        if (CurrentTask.direction == Vector2.zero)
        {
            if (Tasks.Count > 0)
            {
                CurrentTask.direction = (Tasks.Peek().destination - CurrentTask.destination).normalized;
            }
            else
            {
                CurrentTask.direction = (CurrentTask.destination - (Vector2)Nodes[0].transform.position).normalized;
            }
        }

        Vector2 destination = CurrentTask.destination;
        Vector2 direction = CurrentTask.direction;

        bool left = true;
        Vector2 rowPosition = destination - 2 * direction.normalized;
        Vector2 orthogonal = 2 * (Quaternion.AngleAxis(90f, Vector3.forward) * direction.normalized);
        Vector2 offset = orthogonal;
        Vector2 directionNormal = direction.normalized;

        Nodes[0].Assign(CurrentTask, true);
        
        Task task = CurrentTask;

        for (int i = 1; i < Nodes.Count; i++)
        {
            Selectable node = Nodes[i];
            if (left)
            {
                task.destination = rowPosition + offset;
            }
            else
            {
                task.destination = rowPosition - offset;
                rowPosition -= 2 * direction;
                offset += orthogonal;
            }
            left = !left;
            node.Assign(task, true);
        }
    }
}
