using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;
using UnityEngine.SceneManagement;

public class GameController : Singleton<GameController>
{
    [SerializeField] private GameState gameState;

    [Header("UI")] [SerializeField] private GameObject gameUi;
    [SerializeField] private GameObject pauseUi;

    [SerializeField] private GameObject clueUi;
    [SerializeField] private FirstPersonController firstPersonController;

    [Header("Game State Properties")]
    [SerializeField] private int gameDurationS = 60;
    float gameTimer = 0.0f;
    private bool hasTriggeredRespawnOnce = false;

    public GameState GameState => gameState;

    private GameState _prePauseState = GameState.Starting;

    private void Awake()
    {
        if (!ThisIsTheSingletonInstance())
        {
            return;
        }
    }

    private void Start()
    {
        AudioController.Instance.PlayDefaultMusic();

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

        gameTimer += Time.deltaTime;
        int seconds = (int) gameTimer;
        // Debug.Log(seconds);

        if (!hasTriggeredRespawnOnce && (seconds >= gameDurationS)) {
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