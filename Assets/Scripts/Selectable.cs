using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Task
{
    public Selectable target;
    public Vector2 destination;
    public Vector2 direction;
    public bool halt;
    public bool repeat;
}

public class Selectable : MonoBehaviour
{
    public Selectable Root;
    public bool isRoot;
    public List<Selectable> Nodes = new List<Selectable>();
    public Task CurrentTask = new Task();
    public Queue<Task> Tasks = new Queue<Task>();

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color _colorHover;
    [SerializeField] private Color _colorSelect;

    public bool isReady {get; protected set;} = false;
    public bool isHovered {get; private set;} = false;
    public bool isSelected {get; private set;} = false;

    private void Start()
    {
        if (Root == null)
        {
            Root = this;
            isRoot = true;
        }

        CurrentTask.target = null;
        CurrentTask.destination = Root.transform.position;
        CurrentTask.direction = Root.transform.up;
        CurrentTask.halt = true;
        CurrentTask.repeat = false;

        StartTask();
    }

    public virtual bool Notify()
    {
        foreach (Selectable node in Nodes)
        {
            if (!node.isReady)
            {
                return false;
            }
        }
        if (RequestTask())
        {
            StartTask();
        }
        return true;
    }

    public virtual void StartTask()
    {
        if (CurrentTask.direction == Vector2.zero)
        {
            if (Tasks.Count > 0)
            {
                CurrentTask.direction = (Tasks.Peek().destination - CurrentTask.destination).normalized;
            }
            else
            {
                CurrentTask.direction = (CurrentTask.destination - (Vector2)this.transform.position).normalized;
            }
        }
    }

    public void Assign(Task task, bool keepExisting)
    {
        if (!keepExisting)
        {
            Tasks.Clear();
            isReady = true;
        }

        if (isReady)
        {
            isReady = false;
            CurrentTask = task;
            StartTask();
        }
        else
        {
            Tasks.Enqueue(task);
        }
    }

    public bool RequestTask()
    {
        if (Tasks.Count <= 0)
        {
            isReady = true;
            return false;
        }
        if (CurrentTask.repeat)
        {
            Tasks.Enqueue(CurrentTask);
        }
        CurrentTask = Tasks.Dequeue();
        return true;
    }

    public void OnSelect()
    {
        isSelected = true;
        _renderer.enabled = true;
        _renderer.color = _colorSelect;

        if (Nodes.Count != 0)
        {
            KillDeadNodes();
            foreach (Selectable node in Nodes)
            {
                node.OnSelect();
            }
        }
    }
    public void OnDeselect()
    {
        isSelected = false;
        if (isHovered)
        {
            _renderer.color = _colorHover;
        }
        else
        {
            _renderer.enabled = false;
        }
        if (Nodes.Count != 0)
        {
            KillDeadNodes();
            foreach (Selectable node in Nodes)
            {
                node.OnDeselect();
            }
        }
    }

    public void OnHover()
    {
        isHovered = true;
        _renderer.enabled = true;
        _renderer.color = _colorHover;
        
        if (Nodes.Count != 0)
        {
            KillDeadNodes();
            foreach (Selectable node in Nodes)
            {
                node.OnHover();
            }
        }
    }
    public void OnUnhover()
    {
        isHovered = false;
        if (isSelected)
        {
            _renderer.enabled = true;
            _renderer.color = _colorSelect;
        }
        else
        {
            _renderer.enabled = false;
        }
        if (Nodes.Count != 0)
        {
            KillDeadNodes();
            foreach (Selectable node in Nodes)
            {
                node.OnUnhover();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "SelectionBox")
        {
            SelectionManager.Instance.FocusedProspect = this;
            SelectionManager.Instance.Hover(Root);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "SelectionBox")
        {
            SelectionManager.Instance.Unhover(Root);
        }
    }

    public void KillDeadNodes()
    {
        List<Selectable> deadNodes = new List<Selectable>();

        foreach (Selectable node in Nodes)
        {
            if (node == null)
            {
                deadNodes.Add(node);
            }
        }

        foreach (Selectable node in deadNodes)
        {
            Nodes.Remove(node);
        }

        if (!isRoot && Nodes.Count <= 0)
        {
            Destroy(this);
        }
    }

    private void OnDisable()
    {
        if (!isRoot) return;

        if (isSelected) SelectionManager.Instance.Selected.Remove(this);
        if (isHovered) SelectionManager.Instance.Hovered.Remove(this);
    }
}
