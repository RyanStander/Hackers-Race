using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayButton : MonoBehaviour
    {
        [SerializeField] private Image fadeImage;
        private bool isFading;
        
        //When the button is clicked, fade the image in and load scene
        private IEnumerator StartGame()
        {
            var color = fadeImage.color;
            while (color.a < 1)
            {
                color.a += Time.deltaTime;
                fadeImage.color = color;
                yield return null;
            }
            UnityEngine.SceneManagement.SceneManager.LoadScene("Game");
        }
        
        public void PlayGame()
        {
            if (isFading)
                return;
            
            isFading = true;
            StartCoroutine(StartGame());
        }
    }
}
