using System;
using General;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     Courtesy by Fire Totem Games, https://www.firetotemgames.com/
/// </summary>
public class AutomaticCameraSize : MonoBehaviour
{
    /* ======================================================================================================================== */
    /* VARIABLE DECLARATIONS                                                                                                    */
    /* ======================================================================================================================== */

    [SerializeField] private float defaultOrthographicSize = 5f;
    [SerializeField] private int defaultScreenWidth = 1920;
    [SerializeField] private int defaultScreenHeight = 1080;


    private float _defaultRatio = 1920f / 1080f;
    private Camera _camera;

    /* ======================================================================================================================== */
    /* UNITY CALLBACKS                                                                                                          */
    /* ======================================================================================================================== */

    private void Start()
    {
        _defaultRatio = (float) defaultScreenWidth / defaultScreenHeight;
        _camera = GetComponent<Camera>();

        UpdateFieldOfFiew();
    }

    /* ======================================================================================================================== */
    /* PRIVATE FUNCTIONS                                                                                                        */
    /* ======================================================================================================================== */

    private void UpdateFieldOfFiew()
    {
        float multiplier = _defaultRatio / ((float) Screen.width / Screen.height);

        if (multiplier > 1f)
        {
            _camera.fieldOfView =
                2 * Mathf.Atan2(defaultOrthographicSize * multiplier, Mathf.Abs(transform.position.z)) *
                Mathf.Rad2Deg;
        }
        else
        {
            _camera.fieldOfView = 2 * Mathf.Atan2(defaultOrthographicSize, Mathf.Abs(transform.position.z)) *
                                  Mathf.Rad2Deg;
        }

        Debug.Log(_camera.fieldOfView);
    }
#if UNITY_EDITOR
    private int _cameraBoundsInSceneViewMode = 0;
    private bool _oWasPressedBefore = false;

    private void OnDrawGizmos()
    {
        var oPressed = Keyboard.current?.oKey.isPressed == true;
        if (oPressed && !_oWasPressedBefore && UnityEditor.EditorWindow.focusedWindow.titleContent.text == "Scene")
        {
            _cameraBoundsInSceneViewMode = (_cameraBoundsInSceneViewMode + 1) % 4;
        }

        _oWasPressedBefore = oPressed;

        if (_cameraBoundsInSceneViewMode == 0)
            return;

        var sceneView = UnityEditor.SceneView.currentDrawingSceneView;
        if (!sceneView)
            return;

        var center = sceneView.pivot;
        center.z = 0;
        var aspect = (float) defaultScreenWidth / defaultScreenHeight;

        if (_cameraBoundsInSceneViewMode >= 3)
        {
            var lu = sceneView.camera.ViewportToWorldPoint(Vector2.up);
            lu.z = 0;
            var ru = sceneView.camera.ViewportToWorldPoint(Vector2.one);
            ru.z = 0;
            var ll = sceneView.camera.ViewportToWorldPoint(Vector2.zero);
            ll.z = 0;
            var rl = sceneView.camera.ViewportToWorldPoint(Vector2.right);
            rl.z = 0;

            Gizmos.color = Color.black;
            DrawRectGizmo(ll, center + new Vector3(-defaultOrthographicSize * aspect, defaultOrthographicSize, 0));
            DrawRectGizmo(lu, center + new Vector3(defaultOrthographicSize * aspect, defaultOrthographicSize, 0));
            DrawRectGizmo(ru, center + new Vector3(defaultOrthographicSize * aspect, -defaultOrthographicSize, 0));
            DrawRectGizmo(rl, center + new Vector3(-defaultOrthographicSize * aspect, -defaultOrthographicSize, 0));
        }

        var c = Color.white;
        c.a = 0.15f;
        Gizmos.color = c;
        var size = new Vector3(aspect * defaultOrthographicSize * 2, defaultOrthographicSize * 2, 0.01f);
        Gizmos.DrawWireCube(center, size);

        var style = new GUIStyle {normal = {textColor = c}};
        UnityEditor.Handles.BeginGUI();
        var textPos = center +
                      new Vector3(aspect * defaultOrthographicSize - 0.75f, defaultOrthographicSize + 0.5f, 0);
        UnityEditor.Handles.Label(textPos, "CAM", style);
        UnityEditor.Handles.EndGUI();

        if (_cameraBoundsInSceneViewMode >= 2)
        {
            var oct = GameObject.FindWithTag(Constants.Namings.Player);
            var sirCum = oct ? oct.transform.lossyScale.x : 0.7f;
            c = Color.yellow;
            Gizmos.color = c;
            Gizmos.DrawSphere(center, sirCum / 2);

            style.normal.textColor = c;
            textPos = center + Vector3.left * 0.1f + (sirCum / 2) * Vector3.down;
            style.alignment = TextAnchor.UpperCenter;
            UnityEditor.Handles.Label(textPos, "OCTO", style);
        }
    }

    private void DrawRectGizmo(Vector3 lu, Vector3 rl, float sizeZ = 0.01f)
    {
        var center = (lu + rl) / 2;
        center.z = 0;
        var d = rl - lu;
        Gizmos.DrawCube(center, new Vector3(Math.Abs(d.x), Math.Abs(d.y), sizeZ));
    }
#endif

    /* ======================================================================================================================== */
    /* PRIVATE FUNCTIONS                                                                                                        */
    /* ======================================================================================================================== */

    /* ======================================================================================================================== */
    /* PUBLIC FUNCTIONS                                                                                                         */
    /* ======================================================================================================================== */

    /* ======================================================================================================================== */
    /* EVENT CALLERS                                                                                                            */
    /* ======================================================================================================================== */

    /* ======================================================================================================================== */
    /* EVENT LISTENERS                                                                                                          */
    /* ======================================================================================================================== */
}