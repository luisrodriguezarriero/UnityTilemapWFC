using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using AudioUtilities;

namespace Menu{
    public class MainMenu : MonoBehaviour
    {
        public AudioData audioData;
        public void PlaySnake()
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            StartCoroutine(PlayGame(audio.clip.length, 1));
        }
        
        public void Demo()
        {
            SceneManager.LoadScene(1);
        }

        public void Quit()
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.Play();
            StartCoroutine(QuitGame(audio.clip.length));
        }

        private IEnumerator PlayGame(float timeToWait, int sceneID)
        {
            yield return new WaitForSeconds(timeToWait);
            SceneManager.LoadScene(sceneID);
        }
        
        private IEnumerator QuitGame(float timeToWait)
        {
            yield return new WaitForSeconds(timeToWait);
            Application.Quit();
        } 
    }
}
