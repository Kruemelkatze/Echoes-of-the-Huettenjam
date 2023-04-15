using System;
using System.Linq;
using Extensions;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Clue : MonoBehaviour
{
    [SerializeField] private bool canBeInteractedWith = true;
    [SerializeField] private string clueName;

    private bool _lookedAt;

    public bool LookedAt
    {
        get => _lookedAt;
        set
        {
            _lookedAt = value;
            GetComponent<Outline>().enabled = value && canBeInteractedWith;
        }
    }

    private void Start()
    {
        DeactivateIfClueAlreadyUnlocked();
    }

    private void OnValidate()
    {
        var outline = GetComponent<Outline>();

        if (!outline)
            return;

        outline.enabled = false;
        outline.OutlineColor = new Color(1, 0.2029252f, 0);
        outline.OutlineWidth = 10;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
    }

    public void Interact()
    {
        if (!canBeInteractedWith)
            return;

        canBeInteractedWith = false;
        LookedAt = false;
        Unlock();
    }

    private void Unlock()
    {
        var allDraggableClues = FindObjectsOfType<DraggableClue>(true);
        var draggableClue = allDraggableClues.FirstOrDefault(x => clueName.RoughlyEquals(x.Text));

        if (!draggableClue)
            return;

        draggableClue.gameObject.SetActive(true);
    }

    private void DeactivateIfClueAlreadyUnlocked()
    {
        var allDraggableClues = FindObjectsOfType<DraggableClue>(true);
        var draggableClue = allDraggableClues.FirstOrDefault(x => clueName.RoughlyEquals(x.Text));

        if (!draggableClue)
            return;

        if (draggableClue.gameObject.activeSelf)
        {
            canBeInteractedWith = false;
            LookedAt = false;
        }
    }
}