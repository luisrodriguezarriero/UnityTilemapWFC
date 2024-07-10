using UnityEngine;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Mono.Collections.Generic;
using System.Collections;
using System.Linq;

namespace AudioUtilities{
    [CreateAssetMenu]
    public class AudioData : ScriptableObject, ICollection<AudioClip>
    {
        [SerializeField] private AudioReference[] sounds;
        public AudioClip defaultAudioClip = null;
        [System.Serializable]
        public class AudioReference
        {
            public string name;
            public AudioClip audioClip;
        }
        public int Count => sounds.Length;

        public bool IsReadOnly => true;

        public void Add(AudioClip item)
        {
            /*
            var oldSize = sounds.Length;
            System.Array.Resize(ref sounds, oldSize+1);
            sounds[oldSize] = item;
            */
        }

        public void Clear()
        {
            throw new System.NotSupportedException();
        }

        public bool Contains(AudioClip item)
        {
            foreach (var soundData in sounds)
            {
                if (soundData.audioClip.Equals(item)) return true;
            }
            return false;   
        }

        public bool Contains(string key)
        {
            foreach (var item in sounds)
            {
                if (key == item.name) return true;
            }
            getDefault();    
            return false;          
        }

        public void CopyTo(AudioClip[] array, int arrayIndex)
        {
            throw new System.NotSupportedException();
        }

        public AudioClip getClip(string name){
            foreach (var item in sounds)
            {
                if (name == item.name) return item.audioClip;
            }
            return getDefault();              
        }

        private AudioClip getDefault(){
            Debug.LogWarning(message: $"Audio clip not found for {name}");
            return defaultAudioClip;    
        }

        public IEnumerator<AudioClip> GetEnumerator()
        {
            List<AudioClip> list = new();

            foreach (var item in sounds)
            {
                list.Add(item.audioClip);
            }
            return list.GetEnumerator();
        }

        public IEnumerator<string> GetKeysEnumerator()
        {
            List<string> list = new();

            foreach (var item in sounds)
            {
                list.Add(item.name);
            }
            return list.GetEnumerator();
        }

        public bool Remove(AudioClip item)
        {
            throw new System.NotSupportedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return sounds.GetEnumerator();
        }

        private AudioClip soundError(string name){
            Debug.LogError($"Sound {name} does not exist"); 
            return null;
        }    
    }
}