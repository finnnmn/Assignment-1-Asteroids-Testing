using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AsteroidGame
{
    public class MenuControl : MonoBehaviour
    {
        [SerializeField] GameObject startScreen;
        [SerializeField] GameObject gameOverScreen;
        [SerializeField] Text scoreText;

        private void Start()
        {
            startScreen.SetActive(true);
            gameOverScreen.SetActive(false);
        }
        public void StartGame()
        {
            CloseMenus();
            GameControl.instance.StartGame();
        }

        public void QuitGame()
        {
            Application.Quit();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
        }

        public void ReturnToMenu()
        {
            startScreen.SetActive(true);
            gameOverScreen.SetActive(false);
            scoreText.gameObject.SetActive(false);
        }

        public void GameOver()
        {
            gameOverScreen.SetActive(true);
            startScreen.SetActive(false);
        }

        public void UpdateScoreText(int _score)
        {
            scoreText.gameObject.SetActive(true);
            scoreText.text = "Score: " + _score.ToString();
        }

        public void CloseMenus()
        {
            startScreen.SetActive(false);
            gameOverScreen.SetActive(false);
        }

    }
}
