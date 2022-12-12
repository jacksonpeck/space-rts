using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectableNode : MonoBehaviour
{
    [SerializeField] private Selectable _selectable;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private Color _colorHover;
    [SerializeField] private Color _colorSelect;
    
    protected Vector2 Destination;
    protected Vector2 Direction;

    public bool isHovered {get; private set;} = false;
    public bool isSelected {get; private set;} = false;

    private void Awake()
    {
        Destination = this.transform.position;
        Direction = this.transform.up;
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
    }

    public void OnHover()
    {
        isHovered = true;
        _renderer.enabled = true;
        _renderer.color = _colorHover;
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
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "SelectionBox")
        {
            SelectionManager.Instance.Hover(_selectable);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "SelectionBox")
        {
            SelectionManager.Instance.Unhover(_selectable);
        }
    }
}
