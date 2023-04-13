using System;
using System.Collections;
using General;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///     Controller used for managing Scene Transistions.
///     Courtesy by Fire Totem Games, https://www.firetotemgames.com/
///     Adjusted a bit to reuse the Singleton<> Scheme
/// </summary>
public class SceneController : PersistentSingleton<SceneController>
{
    /* ======================================================================================================================== */
    /* VARIABLE DECLARATIONS                                                                                                    */
    /* ======================================================================================================================== */

    [Header("Animations")] [SerializeField]
    private FadeAnimation circleExpand;

    [SerializeField] private FadeAnimation holeShrink;
    [SerializeField] private FadeAnimation simpleFadeFast;
    [SerializeField] private FadeAnimation simpleFadeSlowWithLabel;
    [SerializeField] private FadeAnimation endlessTriangleShrinkAndRotate;


    /* ======================================================================================================================== */
    /* UNITY CALLBACKS                                                                                                          */
    /* ======================================================================================================================== */

    private void Start()
    {
#if UNITY_EDITOR
        UnityEditor.SceneVisibilityManager.instance?.Hide(gameObject, true);
#endif
    }

    /* ======================================================================================================================== */
    /* PRIVATE FUNCTIONS                                                                                                        */
    /* ======================================================================================================================== */

    private IEnumerator LoadSceneCoroutine(FadeAnimation fadeAnimation, string sceneName, bool isRespawn = false)
    {
        // start FadeOut animation and wait for 0.1 seconds
        fadeAnimation.StartFadeOutAnimation();
        yield return new WaitForSecondsRealtime(0.1f);

        // wait until the FadeOut animation has finished
        while (fadeAnimation.Animator1.GetCurrentAnimatorStateInfo(0).length >
               fadeAnimation.Animator1.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            yield return null;
        }

        // load the next scene asynchronously
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (asyncLoad.isDone == false)
        {
            yield return null;
        }

        // wait 0.1 seconds and start FadeIn animation
        yield return new WaitForSecondsRealtime(0.1f);
        fadeAnimation.StartFadeInAnimation();

        Time.timeScale = 1f;
    }

    /* ======================================================================================================================== */
    /* PUBLIC FUNCTIONS                                                                                                         */
    /* ======================================================================================================================== */

    public void LoadScene(string sceneName)
    {
        Instance.simpleFadeFast.SetLabel("");
        Instance.StartCoroutine(LoadSceneCoroutine(Instance.holeShrink, sceneName));
    }

    /// <summary>o
    /// Fade out of current scene and fade in to new scene
    /// </summary>
    /// <param name="sceneName">Scene to load</param>
    [UsedImplicitly]
    public void LoadScene(string sceneName, bool isRespawn = false)
    {
        Instance.simpleFadeFast.SetLabel("");
        Instance.StartCoroutine(LoadSceneCoroutine(Instance.holeShrink, sceneName, isRespawn));
    }

    [UsedImplicitly]
    public IEnumerator LoadSceneAsync(string sceneName)
    {
        Instance.simpleFadeFast.SetLabel("");
        yield return Instance.StartCoroutine(LoadSceneCoroutine(Instance.holeShrink, sceneName));
    }

    [UsedImplicitly]
    public void LoadScene(string sceneName, string sceneLabel)
    {
        Instance.simpleFadeSlowWithLabel.SetLabel(sceneLabel);
        Instance.StartCoroutine(LoadSceneCoroutine(Instance.holeShrink, sceneName));
    }

    [UsedImplicitly]
    public void LoadSceneWithId(int id)
    {
        // todo: stop Sound effect bus
        string sceneName = SceneManager.GetSceneAt(id).name;
        Instance.simpleFadeFast.SetLabel("");
        Instance.StartCoroutine(LoadSceneCoroutine(Instance.holeShrink, sceneName));
    }

    [UsedImplicitly]
    public void RestartScene(bool isRespawn = false)
    {
        LoadScene(SceneManager.GetActiveScene().name, isRespawn);
    }

    /// <summary>
    /// Exit Game Function
    /// </summary>
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBGL
        Application.OpenURL("about:blank");
#else
        Application.Quit ();
#endif
    }

    /* ======================================================================================================================== */
    /* EVENT CALLERS                                                                                                            */
    /* ======================================================================================================================== */

    /* ======================================================================================================================== */
    /* EVENT LISTENERS                                                                                                          */
    /* ======================================================================================================================== */
}