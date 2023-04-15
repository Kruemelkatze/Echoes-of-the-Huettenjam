using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using General;
using UnityEngine;

public class Appear : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float delay = 3;
    [SerializeField] private float duration = 1;

    private bool _appeared = false;

    private void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        
        var clueCanvas = Hub.Get<ClueCanvas>();
        if (clueCanvas)
        {
            Hub.Unregister<ClueCanvas>();
            Destroy(clueCanvas.gameObject);
        }
        
        if (canvasGroup)
        {
            canvasGroup.alpha = 0;
        }

        StartCoroutine(AppearAfterDelay());
    }

    private IEnumerator AppearAfterDelay()
    {
        yield return new WaitForSeconds(delay);
        DoAppear();
    }

    private void DoAppear()
    {
        if (_appeared || !canvasGroup)
            return;

        canvasGroup.DOFade(1, duration);
        _appeared = true;
    }
}