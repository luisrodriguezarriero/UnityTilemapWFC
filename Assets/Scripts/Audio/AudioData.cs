using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace AudioHandling{
    [CreateAssetMenu]
    public class AudioData : ScriptableObject
    {
        [SerializeField] private List<AudioClip> sounds;
        public AudioClip getClip(string name){
            AudioClip clip = null;
            for (int i = 0; clip == null && i < sounds.Count; i++)
            {
                if(sounds[i].name == name)clip = sounds[i];  
            }        
            return clip;
        }
        private AudioClip soundError(string name){
            Debug.LogError($"Sound {name} does not exist"); 
            return null;
        }
        
    }
}