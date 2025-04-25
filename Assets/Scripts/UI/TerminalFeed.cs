using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TerminalFeed : MonoBehaviour
    {
        public TextMeshProUGUI TerminalText; // Assign this in the Inspector

        private readonly List<string> lines = new List<string>();
        private const int maxLines = 10;
        private const float updateInterval = 0.25f;

        private void Awake()
        {
            // Fill initial lines
            for (int i = 0; i < maxLines; i++)
            {
                lines.Add(GenerateLine());
            }

            // Start scrolling coroutine
            StartCoroutine(ScrollFeed());
        }

        private IEnumerator ScrollFeed()
        {
            while (true)
            {
                yield return new WaitForSeconds(updateInterval);

                // Add a new line at the top
                lines.Insert(0, GenerateLine());

                // Keep only the latest 10
                if (lines.Count > maxLines)
                    lines.RemoveAt(lines.Count - 1);

                // Rebuild display with prompt at the bottom
                StringBuilder sb = new StringBuilder();
                foreach (string line in lines)
                {
                    sb.AppendLine(line);
                }

                sb.AppendLine("■"); // Terminal prompt symbol

                // Update UI
                if (TerminalText != null)
                    TerminalText.text = sb.ToString();
            }
        }

        private static string GenerateLine()
        {
            int prefix = Random.Range(1000000, 10000000); // 7-digit
            StringBuilder digits = new StringBuilder();
            for (int i = 0; i < 5; i++)
            {
                int number = Random.Range(10, 100); // 2-digit
                digits.Append(number);
                if (i < 4) digits.Append(" ");
            }
            return $"{prefix}  {digits}";
        }
    }
}
