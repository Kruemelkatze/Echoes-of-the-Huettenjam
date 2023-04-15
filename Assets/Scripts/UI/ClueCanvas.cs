using System;
using System.Linq;
using General;
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
            Hub.Register(this);
        }
    }

    public void CheckCompletion()
    {
        var storyTexts = GetComponentsInChildren<StoryText>();
        var allFullfilled = storyTexts.All(x => x.IsFulfilled());

        if (allFullfilled)
        {
            Win();
        }
    }

    public void Win()
    {
        Debug.Log("WIN");
        SceneController.Instance.LoadScene("WinScene");
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