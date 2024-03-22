using UnityEngine;
using AudioHandling;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.EnhancedTouch;


namespace SnakeGame
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] protected AudioClip onEnabledClip;
        [SerializeField] protected AudioClip onDisabledClip;
        [SerializeField] protected AudioClip onQuitClip;        
        
        void OnDisable()
        {
            Time.timeScale = 1f;

            SoundManager.SoundInstance.Play(onDisabledClip);
        }

        void OnEnable()
        {
            Time.timeScale = 0f;

            SoundManager.SoundInstance.Play(onEnabledClip);
        }

        void Quit()
        {
            SoundManager.SoundInstance.Play(onQuitClip);

            SceneManager.LoadScene(0);
        }


    }

    public class GameOverController : UIController 
    {
        void Awake(){
            //GET PUNCTUATIONS

        }

    }

    public static class UIControllerExtension 
    {
        public static void DeActivate(this UIController UI) => UI.gameObject.SetActive(false);
        public static void Activate(this UIController UI) => UI.gameObject.SetActive(true);

    }
}
