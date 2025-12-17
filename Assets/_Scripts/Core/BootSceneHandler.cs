using UnityEngine;
using UnityEngine.SceneManagement;


namespace Core
{
    public class BootSceneHandler : MonoBehaviour
    {
        [SerializeField] private string uiSceneName = "UIScene";
        [SerializeField] private string mainMenuSceneName = "MenuScene";
   
   
        private void Awake()
        {
            InitBootScene(); 
        }
   
   
        private void InitBootScene()
        {
            SceneManager.LoadSceneAsync(mainMenuSceneName);
            SceneManager.LoadSceneAsync(uiSceneName, LoadSceneMode.Additive);

        }
    }

}
