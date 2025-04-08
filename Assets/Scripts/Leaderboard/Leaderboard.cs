using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Leaderboard
{
    public class Leaderboard : MonoBehaviour
    {
        [SerializeField] private List<TextMeshProUGUI> names;
        [SerializeField] private List<TextMeshProUGUI> scores;
        private float playerScore;
        [SerializeField] private TMP_InputField playerNameInput;
        [SerializeField] private GameObject submitButton;
        [SerializeField] private TextMeshProUGUI currentScoreText;
        [SerializeField] private int levelNum;

        private List<PlayerInfo> collectedScores = new();

        public void OpenLeaderboard(float score)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            
            gameObject.SetActive(true);
            submitButton.SetActive(true);
            playerNameInput.gameObject.SetActive(true);
        
            playerScore = score;
            TimeSpan completionTime = TimeSpanConverter.ConvertToTimeSpan(playerScore);
            currentScoreText.text = "You finished in: " + $"{completionTime.Minutes:D2}:{completionTime.Seconds:D2}:{completionTime.Milliseconds / 10:D2}";
            
            LoadLeaderboard();
        }
        
        private void LoadLeaderboard()
        {
            collectedScores.Clear();
            
            string loadedScores = PlayerPrefs.GetString("LeaderBoards" + levelNum, "");
        
            string[] splitScores = loadedScores.Split(',');

            for (int i = 0; i < splitScores.Length -2; i+=2)
            {
                PlayerInfo loadedInfo = new PlayerInfo(splitScores[i], float.Parse(splitScores[i + 1]));
            
                collectedScores.Add(loadedInfo);
            }
        
            UpdateLeaderBoardVisual();
        }
        
        private void UpdateLeaderBoardVisual()
        {
            for (int i = 0; i < names.Count; i++)
            {
                if (i < collectedScores.Count)
                {
                    names[i].text = collectedScores[i].PlayerName;
                    TimeSpan completionTime = TimeSpanConverter.ConvertToTimeSpan(collectedScores[i].PlayerScore);
                    scores[i].text =
                        $"{completionTime.Minutes:D2}:{completionTime.Seconds:D2}:{completionTime.Milliseconds / 10:D2}";
                }
                else
                {
                    names[i].text = "";
                    scores[i].text = "";
                }
            }
        }

        public void SubmitButton()
        {
            PlayerInfo stats = new PlayerInfo(playerNameInput.text, playerScore);
            collectedScores.Add(stats);

            playerNameInput.text = "";

            SortStats();
            submitButton.SetActive(false);
            playerNameInput.gameObject.SetActive(false);
        }

        private void SortStats()
        {
            for (int i = collectedScores.Count - 1; i > 0; i--)
            {
                if (collectedScores[i].PlayerScore < collectedScores[i - 1].PlayerScore)
                {
                    (collectedScores[i - 1], collectedScores[i]) = (collectedScores[i], collectedScores[i - 1]);
                }
            }

            UpdatePlayerPrefsString();
        }

        private void UpdatePlayerPrefsString()
        {
            string stats = "";

            for (int i = 0; i < collectedScores.Count; i++)
            {
                stats += collectedScores[i].PlayerName + ",";
                stats += collectedScores[i].PlayerScore + ",";
            }

            PlayerPrefs.SetString("LeaderBoards" + levelNum, stats);

            UpdateLeaderBoardVisual();
        }

        public void ClearPrefs()
        {
            PlayerPrefs.DeleteKey("LeaderBoards" + levelNum);
        
            collectedScores.Clear();
        
            for (int i = 0; i < names.Count; i++)
            {
                names[i].text = "";
                scores[i].text = "";
            }
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    
        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }

    public class PlayerInfo
    {
        public string PlayerName;
        public float PlayerScore;

        public PlayerInfo(string name, float score)
        {
            PlayerName = name;
            PlayerScore = score;
        }
    }
}
