using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;

public class Screenshot : MonoBehaviour
{
    [SerializeField] private int superSize = 1;
    
    public void Update()
    {
#if UNITY_EDITOR
        if (Keyboard.current.numpadPlusKey.wasPressedThisFrame)
        {
            // create screenshot folder if it doesn't exists
            string folderpath = Application.dataPath + "/../Screenshots";
            if (Directory.Exists(folderpath) == false)
            {
                Directory.CreateDirectory(folderpath);
            }

            // save screenshot with date, time and resolution
            string resolution = Screen.width * superSize + "x" + Screen.height * superSize + "_";
            string dateAndTime = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            string filename = folderpath + "/screen_" + dateAndTime + "_" + resolution + ".png";
            ScreenCapture.CaptureScreenshot(filename, superSize);
            Debug.Log("Screenshot saved to: " + filename);
        }
#endif
    }
}