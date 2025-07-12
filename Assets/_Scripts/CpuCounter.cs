using UnityEngine;
using TMPro;

namespace Faza
{
    public class CpuCounter : MonoBehaviour
    {
        [SerializeField] private TMP_Text _cpu;
        [SerializeField] private TMP_Text _gpu;

        private void Update()
        {

            var timings = new FrameTiming[1];
            FrameTimingManager.CaptureFrameTimings();

            FrameTimingManager.GetLatestTimings(1, timings);
            _cpu.text = timings[0].cpuFrameTime.ToString();
            _gpu.text = timings[0].gpuFrameTime.ToString();
        }
    }
}
