using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioHandling
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : MonoBehaviour
    {
        private AudioSource _audioSource;
        private static AudioManager _instance;
        public static AudioManager AudioInstance { get { return _instance; } }
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
            _audioSource.clip = clip;
            _audioSource.Play();
            yield return new WaitForSecondsRealtime(_audioSource.clip.length);
        }

        private void OnDestroy() { if (this == _instance) { _instance = null; } }
    }
}
