using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class TextSizeFitter : MonoBehaviour
{
    public TMPro.TextMeshProUGUI text;
    public RectTransform rect;
    public Vector2 margin = Vector2.zero;


    void Update()
    {
        if (text && rect)
        {
            var size = text.GetPreferredValues();
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, size.x + margin.x);
            rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, size.y + margin.y);
        }
    }
}