using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Faza
{
    public class BuildReportExporter
    {
        [MenuItem("Tools/Export Build Report")]
        public static void ExportReport()
        {
            string outputPath = "BuildReported"; // Customize this
            BuildPlayerOptions buildOptions = new BuildPlayerOptions
            {
                scenes = EditorBuildSettings.scenes.Where(x => x.enabled)
                .Select(x => x.path)
                .ToArray(),
                locationPathName = outputPath,
                target = BuildTarget.WebGL,
                options = BuildOptions.DetailedBuildReport
            };

            BuildReport report = BuildPipeline.BuildPlayer(buildOptions);
            BuildSummary summary = report.summary;

            Debug.Log($"Build result: {summary.result}, Total size: {summary.totalSize / (1024f * 1024f):F2} MB");

            // Detailed file list
            foreach (var step in report.steps)
            {
                Debug.Log($"Step: {step.name}");
                foreach (var message in step.messages)
                {
                    Debug.Log($"  {message.content}");
                }
            }

            // List of all files in the build with sizes
            foreach (var file in report.GetFiles())
            {
                Debug.Log($"{file.path} — {file.size / 1024f:F2} KB");
            }
        }

        [MenuItem("Tools/Clear IL2CPP Args")]
        public static void ClearArgs()
        {
            Debug.Log("Before: " + PlayerSettings.GetAdditionalIl2CppArgs());
            PlayerSettings.SetAdditionalIl2CppArgs("");  // Clear it completely
            Debug.Log("After: " + PlayerSettings.GetAdditionalIl2CppArgs());
        }
    }



    //class IL2CPPStats : IPreprocessBuildWithReport
    //{
    //    public int callbackOrder => 0;

    //    public void OnPreprocessBuild(BuildReport report)
    //    {
    //        PlayerSettings.SetAdditionalIl2CppArgs("--print-stats");
    //    }
    //}
}