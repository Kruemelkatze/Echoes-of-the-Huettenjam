﻿using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class GameController : Singleton<GameController>
{
    [SerializeField] private GameState gameState;

    [Header("UI")] [SerializeField] private GameObject gameUi;
    [SerializeField] private GameObject pauseUi;

    [SerializeField] private GameObject clueUi;
    [SerializeField] private FirstPersonController firstPersonController;

    [SerializeField] private AnimationCurve vignetteAnimationCurve;

    [Header("Game State Properties")] [SerializeField]
    private int gameDurationS = 60;

    [SerializeField] private TextMeshProUGUI gameTimeMesh;
    float gameTimer = 0.0f;
    private bool hasTriggeredRespawnOnce = false;
    private bool hasStartedMoving = false;

    public GameState GameState => gameState;

    private GameState _prePauseState = GameState.Starting;

    private Volume _volume;
    private Vignette _vignette;

    private void Awake()
    {
        _volume = FindObjectOfType<Volume>();
        if (_volume)
        {
            _volume.profile.TryGet(out _vignette);
        }

        if (gameTimeMesh)
            gameTimeMesh.text = "";
        if (!ThisIsTheSingletonInstance())
        {
            return;
        }
    }

    private void Start()
    {
        AudioController.Instance.PlayDefaultMusic();

        var clueCanvas = FindObjectOfType<ClueCanvas>(true);
        if (clueCanvas)
            clueUi = clueCanvas.gameObject;

        gameState = GameState.Starting;

        // Do load Stuff
        gameState = GameState.Playing;

        SetPause(false);
    }

    private void Update()
    {
        HandleGameTime();

        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            SetPause(gameState != GameState.Paused);
        }

        if (firstPersonController && clueUi)
        {
            firstPersonController.CanMove = !clueUi.activeInHierarchy;
            Cursor.visible = clueUi.activeInHierarchy;
            Cursor.lockState = Cursor.visible ? CursorLockMode.None : CursorLockMode.Locked;
        }
    }


    //  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ PUBLIC  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    public void PauseGame() => SetPause(true);
    public void ContinueGame() => SetPause(false);

    public void Finished()
    {
        gameState = GameState.Finished;
        Debug.Log("Finished");
    }


    //  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~ PRIVATE  ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
    private void SetPause(bool paused)
    {
        if (paused)
        {
            _prePauseState = gameState;
            gameState = GameState.Paused;
        }
        else
        {
            gameState = _prePauseState;
        }

        if (gameUi)
        {
            gameUi.SetActive(!paused);
        }

        if (pauseUi)
        {
            pauseUi.SetActive(paused);
        }

        if (clueUi)
        {
            clueUi.SetActive(paused);
        }

        // Stopping time depends on your game! Turn-based games maybe don't need this
        Time.timeScale = paused ? 0 : 1;

        // Whatever else there is to do...
        // Deactivate other UI, etc.
    }

    private void HandleGameTime()
    {
        if (!hasStartedMoving && firstPersonController.isMoving) hasStartedMoving = true;

        if (!hasStartedMoving) return;

        gameTimer += Time.deltaTime;
        int seconds = (int)gameTimer;
        // Debug.Log(seconds);
        if (gameTimeMesh)
            gameTimeMesh.text = (gameDurationS - seconds).ToString();

        if (_vignette)
        {
            _vignette.intensity.value = vignetteAnimationCurve.Evaluate(gameTimer / gameDurationS);
        }

        if (!hasTriggeredRespawnOnce && (seconds >= gameDurationS))
        {
            SceneController.Instance.RestartScene(true);
            hasTriggeredRespawnOnce = true;
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(GameController))]
    public class GameControlTestEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var gct = target as GameController;

            if (gct == null)
                return;

            if (!Application.isPlaying)
                return;

            if (GUILayout.Button("Restart"))
            {
                SceneController.Instance.RestartScene();
            }
        }
    }
#endif
}