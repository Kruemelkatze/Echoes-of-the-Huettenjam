using System;
using System.Linq;
using Clues;
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

    private void OnEnable()
    {
        var tooltip = Hub.Get<ClueTooltip>();
        if (tooltip)
            tooltip.SetActive(false);
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

    public void Continue()
    {
        GameController.Instance.ContinueGame();
    }

    public void Restart()
    {
        SceneController.Instance.RestartScene(true);
    }

    public void BackToMenu()
    {
        SceneController.Instance.LoadScene("MainMenu");
    }
}