using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableClue : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI tmp;
    private string text;
    private Vector3 startPosition;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public ClueBlank InstalledIn { get; set; }
    public bool PositioningHandled { get; set; }
    public bool IgnoreDrop { get; set; }

    public string Text => text = tmp.text;

    // Start is called before the first frame update
    void Start()
    {
        text = tmp.text;
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void OnValidate()
    {
        text = tmp.text;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        PositioningHandled = false;
        IgnoreDrop = false;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if (PositioningHandled || IgnoreDrop)
            return;

        ResetPosition();
    }

    public void ResetPosition()
    {
        RemoveFromPreviousBlank();
        rectTransform.anchoredPosition = startPosition;
    }

    public void RemoveFromPreviousBlank()
    {
        if (InstalledIn)
        {
            InstalledIn.Remove(this);
            InstalledIn = null;
        }
    }
}