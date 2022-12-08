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
    public List<Selectable> Available = new List<Selectable>();
    
    private int _selection_index = 0;

    public void SelectOne()
    {
        if (Hovered.Count <= 0)
        {
            return;
        }
        _selection_index %= Hovered.Count;
        int index = 0;
        foreach (Selectable s in Hovered)
        {
            if (_selection_index == index++)
            {
                Select(s);
                break;
            }
        }
        _selection_index++;
    }
    public void SelectHovered()
    {
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

    public void DeselectAll()
    {
        foreach (Selectable s in Selected)
        {
            s.OnDeselect();
        }
        Selected.Clear();
    }
}
