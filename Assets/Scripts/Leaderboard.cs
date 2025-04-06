using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leaderboard : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;
    private float playerScore;
    [SerializeField] private TMP_InputField playerNameInput;
    [SerializeField] private GameObject submitButton;
    [SerializeField] private TextMeshProUGUI currentScoreText;

    private List<PlayerInfo> collectedScores = new();

    public void OpenLeaderboard(float score)
    {
        gameObject.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        
        playerScore = score;
        currentScoreText.text = "You finished in: " + playerScore.ToString();
        submitButton.SetActive(true);
        playerNameInput.gameObject.SetActive(true);
        
        LoadLeaderboard();
    }

    private void Start()
    {
        LoadLeaderboard();
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
            if (collectedScores[i].PlayerScore > collectedScores[i - 1].PlayerScore)
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

        PlayerPrefs.SetString("LeaderBoards", stats);

        UpdateLeaderBoardVisual();
    }

    private void UpdateLeaderBoardVisual()
    {
        for (int i = 0; i < names.Count; i++)
        {
            if (i < collectedScores.Count)
            {
                names[i].text = collectedScores[i].PlayerName;
                scores[i].text = collectedScores[i].PlayerScore.ToString();
            }
            else
            {
                names[i].text = "";
                scores[i].text = "";
            }
        }
    }

    private void LoadLeaderboard()
    {
        string scores = PlayerPrefs.GetString("LeaderBoards", "");
        
        string[] scores2 = scores.Split(',');

        for (int i = 0; i < scores2.Length -2; i+=2)
        {
            PlayerInfo loadedInfo = new PlayerInfo(scores2[i], float.Parse(scores2[i + 1]));
            
            collectedScores.Add(loadedInfo);
        }
        
        UpdateLeaderBoardVisual();
    }

    public void ClearPrefs()
    {
        PlayerPrefs.DeleteKey("LeaderBoards");
        
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
