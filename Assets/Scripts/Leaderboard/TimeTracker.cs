using System;
using TMPro;
using UnityEngine;

namespace Leaderboard
{
    public class TimeTracker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;
        
        private DateTime timeStarted;
        private TimeSpan totalTime;

        private void OnValidate()
        {
            if (timeText == null)
                timeText = GetComponent<TextMeshProUGUI>();
        }

        public void StartCounting()
        {
            timeStarted = DateTime.Now;
        }

        private void Start()
        {
            StartCounting();
        }

        private void Update()
        {
            totalTime = DateTime.Now - timeStarted;
            timeText.text = $"$trace_time {totalTime.Minutes:D2}:{totalTime.Seconds:D2}.{totalTime.Milliseconds / 10:D2}";
        }
        
        public TimeSpan GetTotalTime()
        {
            return totalTime;
        }
    }
}
