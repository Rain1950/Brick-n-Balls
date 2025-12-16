using System;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace Core
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance {get; private set;}
        private UIManager _uiManager;
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void RegisterUIManager(UIManager uiManager)
        {
            _uiManager = uiManager;
            Debug.Log("Registered UI Manager");
        }

        public void StartGame()
        {
            SceneManager.UnloadSceneAsync("MenuScene");
            SceneManager.LoadSceneAsync("GameScene",  LoadSceneMode.Additive);
        }
    }

}
