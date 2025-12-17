using System;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance {get; private set;}

        public int ballCount = 20;
        public int currentBallCount;
        public event Action OnGameFinished;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            ScoreManager.OnBallDied += ScoreManagerOnBallDied;
            currentBallCount = ballCount;
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
        }

        private void SceneManagerOnsceneLoaded(Scene scene, LoadSceneMode mode)
        {
            currentBallCount = ballCount;
        }


        private void ScoreManagerOnBallDied()
        {
            currentBallCount--;
            if (currentBallCount <= 0)
            {
                GameOver();
            }
        }

        private void GameOver()
        {
            OnGameFinished?.Invoke();
        }

        public void RestartGame()
        {
            SceneManager.UnloadSceneAsync("GameScene");
           var loadOp =   SceneManager.LoadSceneAsync("MenuScene", LoadSceneMode.Additive);
           loadOp.completed += operation =>
           {
               SceneManager.SetActiveScene(SceneManager.GetSceneByName("MenuScene"));
           };

        }
     

        public void StartGame()
        {
            SceneManager.UnloadSceneAsync("MenuScene");
            var loadOp =  SceneManager.LoadSceneAsync("GameScene",  LoadSceneMode.Additive);
            loadOp.completed += operation =>
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName("GameScene"));
            };


        }
    }

}
