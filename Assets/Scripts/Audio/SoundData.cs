 using UnityEngine;
 
namespace AudioUtilities{

    [CreateAssetMenu(fileName = "SoundData", menuName = "SoundData", order = 0)]
    public class SoundData : ScriptableObject {
        
        [SerializeField] AudioClip
    }
}