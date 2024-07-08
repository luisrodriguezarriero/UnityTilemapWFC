using System.Collections;
using UnityEngine;

namespace AudioUtilities
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        private static SoundManager _instance;
        public static SoundManager Instance { get { return _instance; } }
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            } else {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        public IEnumerator Play(AudioClip clip)
        {
            if(!clip) Debug.Log("Clip not found");
            else{
                _audioSource.Play();
                yield return null;
            }
        }

        private void OnDestroy() { if (this == _instance) { _instance = null; } }
    }
}
