using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioUtilities;

namespace Menu{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] protected AudioData audioData;  
        public void PlaySnake()
        {
            audioData.Play("Play");
            SceneManager.LoadScene(1);
        }
        public void Quit()
        {
            audioData.Play("Quit");
            Application.Quit();
        }
    }
}
