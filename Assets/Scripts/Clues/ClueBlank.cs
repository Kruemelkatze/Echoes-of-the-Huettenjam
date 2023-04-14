using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClueBlank : MonoBehaviour, IDropHandler
{
    [SerializeField] private string shouldBeText = "Clueless";
    [SerializeField] private DraggableClue installedClue;

    [SerializeField] private DraggableClue quicklySetCorrectClue;

    public bool IsFulfilled()
    {
        return installedClue && TextRoughlyEquals(installedClue.Text, shouldBeText);
    }

    public static bool TextRoughlyEquals(string text1, string text2)
    {
        return text1?.ToLower().Replace(" ", "").Trim() == text2?.ToLower().Replace(" ", "").Trim();
    }

    private void OnValidate()
    {
        if (!quicklySetCorrectClue)
            return;

        shouldBeText = quicklySetCorrectClue.Text;
        quicklySetCorrectClue = null;
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        var draggableClue = eventData.pointerDrag.GetComponent<DraggableClue>();
        if (draggableClue == null)
            return;

        if (installedClue == draggableClue)
        {
            installedClue.IgnoreDrop = true;
        }

        if (installedClue != null)
        {
            installedClue.ResetPosition();
            installedClue = null;
        }

        draggableClue.RemoveFromPreviousBlank();
        draggableClue.InstalledIn = this;
        installedClue = draggableClue;
        draggableClue.PositioningHandled = true;

        var rectTransform = draggableClue.GetComponent<RectTransform>();
        rectTransform.position = GetComponent<RectTransform>().position;
    }

    public void Remove(DraggableClue clue)
    {
        if (installedClue == clue)
            installedClue = null;
    }
}