using System;
using System.Linq;
using UnityEngine;

public class ClueCanvas : MonoBehaviour
{
    private void Awake()
    {
        var anyCanvas = FindObjectsOfType<ClueCanvas>(true).Any(x => x.gameObject != gameObject);

        if (anyCanvas)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public static void ResetAllClueTracking()
    {
        var canvases = FindObjectsOfType<ClueCanvas>(true);
        foreach (var canvas in canvases)
        {
            Destroy(canvas.gameObject);
        }
    }
}