using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public void LoadLevel1()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
    
    public void LoadLevel2()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level2");
    }
    
    public void QuitGame()
    {
        Application.Quit();
    }
}
