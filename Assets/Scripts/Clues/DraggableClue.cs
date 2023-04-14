using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DraggableClue : MonoBehaviour
{
    private string text;

    [SerializeField] private TextMeshProUGUI tmp;

    public string Text => text;

    // Start is called before the first frame update
    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        text = tmp.text;
    }
}