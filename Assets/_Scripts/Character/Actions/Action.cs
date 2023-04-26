using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Character
{
    enum RangeType
    {
        Melee,
        Ranged
    }
    enum Type{Martial, Spell, Tech}

    public abstract class Action : ScriptableObject
    {
        private string name;
        public abstract void Execute(SortedSet<int> targets);
        private RangeType range;
        private Type type;
    }
    
    
}