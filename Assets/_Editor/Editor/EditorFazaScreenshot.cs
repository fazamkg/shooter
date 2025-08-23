using System;
using UnityEditor;
using UnityEngine;

namespace Faza
{
    public class MomentumScreenshot
    {
        private static int _take;

        [MenuItem("Faza/Take a screenshot %e")]
        private static void Take()
        {
            var desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            var path = $@"{desktopPath}\faza_screenshot_{_take}.png";
            ScreenCapture.CaptureScreenshot(path);
            _take++;
        }
    }
}