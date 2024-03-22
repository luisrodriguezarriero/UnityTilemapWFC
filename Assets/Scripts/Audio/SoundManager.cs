using System.Collections;
using UnityEngine;

namespace AudioHandling
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        private static SoundManager _instance;
        public static SoundManager SoundInstance { get { return _instance; } }
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
            } else {
                _instance = this;
            }
        }
        public IEnumerator Play(AudioClip clip)
        {
            if(!clip) Debug.Log("Clip not found");
            else{
                _audioSource.Play();
                yield return new WaitForSecondsRealtime(_audioSource.clip.length);
            }
        }

        private void OnDestroy() { if (this == _instance) { _instance = null; } }
    }
}
