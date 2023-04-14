using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableClue : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private TextMeshProUGUI tmp;
    private Vector3 startPosition;
    private Transform startParent;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public ClueBlank InstalledIn { get; set; }
    public bool PositioningHandled { get; set; }
    public bool IgnoreDrop { get; set; }

    public string Text => tmp.text;

    // Start is called before the first frame update
    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPosition = rectTransform.anchoredPosition;
        startParent = transform.parent;
        canvasGroup = GetComponent<CanvasGroup>();
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
        
        transform.SetParent(startParent);
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        if (InstalledIn)
        {
            transform.SetParent(InstalledIn.transform);
        }
        else
        {
            transform.SetParent(startParent);
        }

        canvas = GetComponentInParent<Canvas>();


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