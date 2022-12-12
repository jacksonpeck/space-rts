using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public List<SelectableNode> Nodes;

    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color _colorHover;
    [SerializeField] private Color _colorSelect;
    
    protected Vector2 Destination;
    protected Vector2 Direction;

    public bool isHovered {get; private set;} = false;
    public bool isSelected {get; private set;} = false;

    private void Awake()
    {
        setDestination(this.transform.position, this.transform.up);
    }

    public virtual void setDestination(Vector2 destination, Vector2 direction)
    {
        Destination = destination;
        Direction = direction;
    }

    public void OnSelect()
    {
        isSelected = true;
        _renderer.enabled = true;
        _renderer.color = _colorSelect;
        if (Nodes.Count != 0)
        {
            KillDeadNodes();
            foreach (SelectableNode node in Nodes)
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
            foreach (SelectableNode node in Nodes)
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
            foreach (SelectableNode node in Nodes)
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
            foreach (SelectableNode node in Nodes)
            {
                node.OnUnhover();
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "SelectionBox")
        {
            SelectionManager.Instance.Hover(this);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "SelectionBox")
        {
            SelectionManager.Instance.Unhover(this);
        }
    }

    public void KillDeadNodes()
    {
        List<SelectableNode> deadNodes = new List<SelectableNode>();

        foreach (SelectableNode node in Nodes)
        {
            if (node == null)
            {
                deadNodes.Add(node);
            }
        }

        foreach (SelectableNode node in deadNodes)
        {
            Nodes.Remove(node);
        }
    }

    private void OnDisable()
    {
        if (isSelected) SelectionManager.Instance.Selected.Remove(this);
        if (isHovered) SelectionManager.Instance.Hovered.Remove(this);
    }
}
