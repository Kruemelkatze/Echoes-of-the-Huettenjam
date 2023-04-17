using System;
using System.Linq;
using Extensions;
using UnityEngine;

[RequireComponent(typeof(Outline))]
public class Clue : MonoBehaviour
{
    [SerializeField] private bool canBeInteractedWith = true;
    [SerializeField] private string clueName;

    [SerializeField] private DraggableClue quicklySetCorrectClue;

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
        if (quicklySetCorrectClue)
        {
            clueName = quicklySetCorrectClue.Text;
            quicklySetCorrectClue = null;
        }

        var outline = GetComponent<Outline>();

        if (!outline)
            return;

        outline.enabled = false;
        outline.OutlineColor = new Color(1, 0.2029252f, 0);
        outline.OutlineWidth = 10;
        outline.OutlineMode = Outline.Mode.OutlineVisible;
    }

    public virtual void Interact()
    {
        if (!canBeInteractedWith)
            return;

        canBeInteractedWith = false;
        LookedAt = false;
        DisableLight();
        Unlock();
        AudioController.Instance.PlaySound("clue");
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
            DisableLight();
        }
    }

    private void DisableLight()
    {
        var l = GetComponentInChildren<Light>();
        if (l)
            l.enabled = false;
    }
}