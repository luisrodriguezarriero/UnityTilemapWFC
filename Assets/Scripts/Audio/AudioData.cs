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
        [SerializeField] private AudioClip[] sounds;

        public int Count => sounds.Length;

        public bool IsReadOnly => true;

        public void Add(AudioClip item)
        {
            throw new System.NotSupportedException();
        }

        public void Clear()
        {
            throw new System.NotSupportedException();
        }

        public bool Contains(AudioClip item)
        {
            return sounds.Contains(item);
        }

        public void CopyTo(AudioClip[] array, int arrayIndex)
        {
            throw new System.NotSupportedException();
        }

        public AudioClip getClip(string name){
            AudioClip clip = null;
            for (int i = 0; clip == null && i < sounds.Length; i++)
            {
                if(sounds[i].name == name)clip = sounds[i];  
            }        
            return clip;
        }

        public IEnumerator<AudioClip> GetEnumerator()
        {
            return (IEnumerator<AudioClip>)sounds.GetEnumerator();        
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