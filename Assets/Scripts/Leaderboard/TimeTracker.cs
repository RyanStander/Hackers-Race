using System;
using TMPro;
using UnityEngine;

namespace Leaderboard
{
    public class TimeTracker : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI timeText;
        
        private static DateTime timeStarted;
        private static TimeSpan totalTime;

        private void OnValidate()
        {
            if (timeText == null)
                timeText = GetComponent<TextMeshProUGUI>();
        }

        public static void StartCounting()
        {
            timeStarted = DateTime.Now;
        }

        private void Start()
        {
            StartCounting();
        }

        public void Update()
        {
            totalTime = DateTime.Now - timeStarted;
            timeText.text = $"{totalTime.Minutes:D2}:{totalTime.Seconds:D2}:{totalTime.Milliseconds / 10:D2}";
        }
    }
}
