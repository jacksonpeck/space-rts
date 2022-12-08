using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    [SerializeField] private Color _colorHover;
    [SerializeField] private Color _colorSelect;
    
    public Vector2 Destination;
    public Vector2 Direction;

    private SpriteRenderer _renderer;
    public bool _hovered = false;
    public bool _selected = false;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();

        Destination = this.transform.position;
    }

    public void OnSelect()
    {
        _selected = true;
        _renderer.enabled = true;
        _renderer.color = _colorSelect;
    }
    public void OnDeselect()
    {
        _selected = false;
        if (_hovered)
        {
            _renderer.color = _colorHover;
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
            _hovered = true;
            _renderer.enabled = true;
            _renderer.color = _colorHover;
            SelectionManager.Instance.Hovered.Add(this);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "SelectionBox")
        {
            _hovered = false;
            SelectionManager.Instance.Hovered.Remove(this);
            if (_selected)
            {
                _renderer.enabled = true;
                _renderer.color = _colorSelect;
            }
            else
            {
                _renderer.enabled = false;
            }
        }
    }

    private void OnEnable()
    {
        SelectionManager.Instance.Available.Add(this);
    }
    private void OnDisable()
    {
        if (_selected) SelectionManager.Instance.Selected.Remove(this);
        if (_hovered) SelectionManager.Instance.Hovered.Remove(this);
        SelectionManager.Instance.Available.Remove(this);
    }
}
