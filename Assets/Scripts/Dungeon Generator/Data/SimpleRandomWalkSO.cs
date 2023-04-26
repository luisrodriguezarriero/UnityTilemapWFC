using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ProceduralDungeon{
    [CreateAssetMenu(fileName = "SimpleRandomWalkParameters", menuName = "PCG/SimpleRandomWalkData")]
    public class SimpleRandomWalkSO : ScriptableObject
    {
        public int iterations = 10, walkLength = 10;
        public bool startRandomlyEachIteration = true;
    }
}
