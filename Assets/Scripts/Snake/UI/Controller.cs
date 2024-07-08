using UnityEngine;
using AudioUtilities;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.EnhancedTouch;
using System.Collections.Generic;


namespace Snake.UI
{
    public class Controller : MonoBehaviour
    {
        static readonly string[] audioElements = {"onEnabled", "onDisabled", "onQuit"};
        [SerializeField] protected AudioData clips;      
        void OvValidate(){

        }
        void OnDisable()
        {
            Time.timeScale = 1f;

            SoundManager.Instance.Play(clips.getClip("onEnabled"));
        }

        void OnEnable()
        {
            Time.timeScale = 0f;

            SoundManager.Instance.Play(clips.getClip("onDisabled"));
        }

        void Quit()
        {
            SoundManager.Instance.Play(clips.getClip("onQuit"));

            SceneManager.LoadScene(0);
        }

        void Play(string name)
        {
            var clip = clips.getClip(name);

            if(clip!=null) SoundManager.Instance.Play(clip);
        }


    }


    public class GameOverController : Controller 
    {
        void Awake(){
            //GET PUNCTUATIONS

        }

    }

    public static class ControllerExtension 
    {
        public static void DeActivate(this Controller UI) => UI.gameObject.SetActive(false);
        public static void Activate(this Controller UI) => UI.gameObject.SetActive(true);

    }
}
