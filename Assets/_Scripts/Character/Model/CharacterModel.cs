using UnityEngine;
using System.Collections.Generic;

namespace _Scripts.Character.Model
{
    public abstract class 
        CharacterModel
    {
        private int power;
        private int speed;
        private int life;
        private int intelligence;
        private int mana;
        public Job job;
        private int id;
        public CharacterView view;
    }

    public class CharacterView:ScriptableObject
    {
    }


    public enum Alignment {Player, Ally, Neutral, Wild, BountyHunter}
    public enum ActionType{Common, Spell, Tech}

    public abstract class Action : ScriptableObject
    {
        private string name;
        public abstract void Execute(HashSet<Actor> targets);
        private ActionType actionType;
    }

    public class Actor
    {
    }

    
    
}