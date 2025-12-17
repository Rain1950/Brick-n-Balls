using System;
using Core;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace  UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("MainMenu")]
        [SerializeField] private Canvas mainMenuCanvas;
        [SerializeField] private Button startGameButton;
        [Header("GameHUD")]
        [SerializeField] private Canvas gameHUDCanvas;
        [SerializeField] private TMP_Text shotsLeftText; 
        [Header("GameFinished")]
        [SerializeField] private Canvas gameFinishedCanvas;
        [SerializeField] private TMP_Text endScoreText;
        [SerializeField] private Button goBackToMenuButton;
        
        
    
        private void Awake()
        {
            AddListeners();
            SceneManager.sceneLoaded += SceneManagerOnsceneLoaded;
        }

        private void SceneManagerOnsceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "GameScene")
            {
                InitializeGameHUD();
            }
        }

        private void Start()
        {
            
            GameManager.Instance.OnGameFinished += OnGameFinished;
            CameraShooter.OnShoot += CameraShooterOnShoot;
            InitializeGameHUD();
            
            
        }

       

        private void OnGameFinished()
        {
            gameHUDCanvas.gameObject.SetActive(false);
            gameFinishedCanvas.gameObject.SetActive(true);
            endScoreText.text = $"Score: {ScoreManager.Instance.score}";
        }

        private void AddListeners()
        {
            startGameButton.onClick.AddListener(OnStartGameButtonClicked);
            goBackToMenuButton.onClick.AddListener(OnGoBackToMenuButtonClicked);
        }

 

        #region MainMenu

        public void OnStartGameButtonClicked()
        {
            GameManager.Instance.StartGame();
            mainMenuCanvas.gameObject.SetActive(false);
            gameHUDCanvas.gameObject.SetActive(true);
        }

        #endregion    
        
        #region GameHUD

        private void InitializeGameHUD()
        {
            shotsLeftText.text = $"Shots Left\n{GameManager.Instance.currentBallCount}";
        }
        
        
        private void CameraShooterOnShoot(int ammoCount)
        {
            shotsLeftText.text = $"Shots Left\n{ammoCount}";
        }
            
        
        #endregion
        
        #region GameFinished
        private void OnGoBackToMenuButtonClicked()
        {
            GameManager.Instance.RestartGame();
            gameFinishedCanvas.gameObject.SetActive(false);
            mainMenuCanvas.gameObject.SetActive(true);
        }

        #endregion
    
    
    }
}

