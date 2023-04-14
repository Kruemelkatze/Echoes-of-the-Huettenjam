using System.Collections;
using System.Collections.Generic;
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

    public bool PositioningHandled { get; set; }

    public string Text => text;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponentInChildren<TextMeshProUGUI>();
        text = tmp.text;
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        PositioningHandled = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (PositioningHandled)
            return;

        rectTransform.anchoredPosition = startPosition;
    }
}