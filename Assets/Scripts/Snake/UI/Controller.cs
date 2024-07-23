using UnityEngine;
using AudioUtilities;
using UnityEngine.SceneManagement;

namespace Snake.UI
{
    public class Controller : MonoBehaviour
    {
        static readonly string[] audioElements = {"Pick", "Back"};
        [SerializeField] protected AudioData clips;      
        void OnDisable()
        {
            Time.timeScale = 1f;

            SoundManager.Instance.Play(clips.getClip(audioElements[1]));
        }

        void OnEnable()
        {
            Time.timeScale = 0f;
        }

        void Quit()
        {
            SoundManager.Instance.Play(clips.getClip(audioElements[1]));

            SceneManager.LoadScene(0);
        }

        void Play(string name)
        {
            var clip = clips.getClip(name);

            if(clip!=null) SoundManager.Instance.Play(clip);
        }

        public void DeActivate() => this.gameObject.SetActive(false);
        public void Activate() => this.gameObject.SetActive(true);
    }
}
