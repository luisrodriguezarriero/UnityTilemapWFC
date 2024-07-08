using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioUtilities
{
    [RequireComponent(typeof(AudioSource))]
    public class MusicManager : MonoBehaviour
    {
        private AudioSource _musicSource;
        private static MusicManager _instance;
        public static MusicManager Instance { get => _instance; private set => _instance = value; }
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            } else {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }
        public IEnumerator Play(AudioClip clip)
        {
            _musicSource.clip = clip;
            _musicSource.Play();
            yield return null;
        }

        private void OnDestroy() { if (this == _instance) { _instance = null; } }

        public IEnumerator PlayFaded(AudioClip nextTrack, float fadeDuration = 0.5f){
            if(_musicSource != null) Fade(fadeDuration, true);

            _musicSource.clip = nextTrack;
            _musicSource.Play();

            Fade(fadeDuration);
            yield return null;
        }

        IEnumerator Fade(float fadeDuration = 0.5f, bool fadeOut = false){
            float percent = 0;
            var (begin, end) = fadeOut? (1.0f, 0.0f): (0.0f, 1.0f);
            while (percent < 1)
            {
                percent+=Time.deltaTime / fadeDuration;
                _musicSource.volume = Mathf.Lerp(1f, 0, percent);
                yield return null;
            }
        }
    
    }
}
