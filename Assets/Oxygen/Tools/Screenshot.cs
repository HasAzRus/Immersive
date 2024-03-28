using System.IO;
using UnityEngine;

namespace Oxygen
{
    public class Screenshot
    {
        public static void Capture(string name, int superSize)
        {
            var path = Application.persistentDataPath + "/Screenshots";
            
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            ScreenCapture.CaptureScreenshot(path + $"/{name}.png", superSize);
        }
    }
}