using System.Collections.Generic;
using UnityEngine;

namespace Character
{
    public enum RangeType
    {
        Melee,
        Ranged
    }
    public enum Type{Martial, Spell, Tech, Social, Move}

    public abstract class Action : ScriptableObject
    {
        private string actionName;
        private int nTargets;
        protected abstract void Execute(SortedSet<int> targets);
        private RangeType range;
        private Type type;

    }
}