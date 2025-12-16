using System;
using Core;
using UnityEngine;
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
    
        private void Awake()
        {
            AddListeners();
        }

        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.RegisterUIManager(this);
            }
            else
            {
                Debug.LogError("GameManager not found");
            }
        }

        private void AddListeners()
        {
            startGameButton.onClick.AddListener(OnStartGameButtonClicked);
        }

        #region MainMenu

        public void OnStartGameButtonClicked()
        {
            GameManager.Instance.StartGame();
            mainMenuCanvas.gameObject.SetActive(false);
            gameHUDCanvas.gameObject.SetActive(true);
        }

        #endregion    
    
    
    }
}

