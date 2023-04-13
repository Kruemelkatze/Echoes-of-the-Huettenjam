using UnityEngine;
using UnityEngine.UI;

/// <summary>
///     Courtesy by Fire Totem Games, https://www.firetotemgames.com/
/// </summary>
public class AutomaticCanvasScale : MonoBehaviour
{
    /* ======================================================================================================================== */
    /* VARIABLE DECLARATIONS                                                                                                    */
    /* ======================================================================================================================== */

    [SerializeField] private ScreenOrientation screenOrientation;
    [SerializeField] private float width = 1920f;
    [SerializeField] private float height = 1080f;

    private enum ScreenOrientation
    {
        Horizontal,
        Vertical
    }
    
        /* ======================================================================================================================== */
    /* UNITY CALLBACKS                                                                                                          */
    /* ======================================================================================================================== */

    private void Start ()
    {
        SetScaleMode();
    }
    
    /* ======================================================================================================================== */
    /* PRIVATE FUNCTIONS                                                                                                        */
    /* ======================================================================================================================== */
    
    /* ======================================================================================================================== */
    /* PRIVATE FUNCTIONS                                                                                                        */
    /* ======================================================================================================================== */

    private void SetScaleMode()
    {
        CanvasScaler canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        canvasScaler.referenceResolution = new Vector2(width, height);
        
        float ratio = 0f;
        float defaultRatio = 0f;

        switch (screenOrientation)
        {
            case ScreenOrientation.Horizontal:
                defaultRatio = width / height;
                ratio = ((float)Screen.width / Screen.height) / defaultRatio;
                break;
            case ScreenOrientation.Vertical:
                defaultRatio = height / width;
                ratio = ((float)Screen.height / Screen.width) / defaultRatio;
                break;
            default:
                break;
        }

        float matchWidthOrHeight = ratio < 1f ? 0.0f : 1.0f;

        gameObject.GetComponent<CanvasScaler>().matchWidthOrHeight = matchWidthOrHeight;
    }

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
