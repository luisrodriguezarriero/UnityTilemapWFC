using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public abstract class PartyController
    {
        public Turn t;
        private bool turnIsOver = false;

        public SortedSet<Character> Characters;

        public int[] getIds()
        {
            //TODO
            throw new System.NotImplementedException();
        }
    }

    public class Character : ScriptableObject
    {
        public CharacterModel model;
    }
    
    
}