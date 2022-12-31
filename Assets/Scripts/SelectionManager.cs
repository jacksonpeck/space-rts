using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager
{
    private static SelectionManager _instance;
    public static SelectionManager Instance
    {
        get
        { 
            if (_instance == null)
            {
                _instance = new SelectionManager();
            }
            return _instance;
        }
        private set
        {
            _instance = value;
        }
    }

    public HashSet<Selectable> Selected = new HashSet<Selectable>();
    public HashSet<Selectable> Hovered = new HashSet<Selectable>();
    public Selectable Focused;
    public Selectable FocusedProspect;
    
    private int _selectionIndex = 0;

    public void SelectOne()
    {
        Focused = null;
        if (Hovered.Count <= 0)
        {
            DeselectAll();
            return;
        }
        _selectionIndex %= Hovered.Count;
        Selectable toSelect = null;
        int index = 0;
        foreach (Selectable h in Hovered)
        {
            if (_selectionIndex == index++)
            {
                toSelect = h;
                break;
            }
        }
        if (FocusedProspect != null && FocusedProspect.isSelected)
        {
            Focused = FocusedProspect;
        }
        DeselectAll();
        Select(toSelect);
        _selectionIndex++;
    }

    public void SelectAnother()
    {
        Focused = null;
        if (Hovered.Count <= 0)
        {
            return;
        }
        _selectionIndex %= Hovered.Count;
        Selectable toSelect = null;
        int index = 0;
        foreach (Selectable h in Hovered)
        {
            if (_selectionIndex == index++)
            {
                toSelect = h;
                break;
            }
        }
        if (FocusedProspect != null && FocusedProspect.isSelected)
        {
            Focused = FocusedProspect;
        }
        Select(toSelect);
        _selectionIndex++;
    }

    public void SelectHovered()
    {
        Focused = null;
        foreach (Selectable s in Hovered)
        {
            Select(s);
        }
    }

    public void SelectMoreHovered()
    {
        Focused = null;
        DeselectAll();

        foreach (Selectable s in Hovered)
        {
            Select(s);
        }
    }

    public void Select(Selectable toSelect)
    {
        toSelect.OnSelect();
        Selected.Add(toSelect);
    }
    public void Deselect(Selectable toDeselect)
    {
        toDeselect.OnDeselect();
        Selected.Remove(toDeselect);
    }
    public void Hover(Selectable toHover)
    {
        toHover.OnHover();
        Hovered.Add(toHover);
    }
    public void Unhover(Selectable toUnhover)
    {
        toUnhover.OnUnhover();
        Hovered.Remove(toUnhover);
    }

    public void DeselectAll()
    {
        foreach (Selectable s in Selected)
        {
            s.OnDeselect();
        }
        Selected.Clear();
    }

    public void Assign(Task task, bool keepExisting)
    {
        foreach (Selectable s in Selected)
        {
            s.Assign(task, keepExisting);
        }
    }
}
