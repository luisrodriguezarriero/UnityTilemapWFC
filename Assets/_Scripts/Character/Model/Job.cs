using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Character
{
    [CreateAssetMenu(fileName = "Job", menuName = "Characters/Job", order = 1)]
    public class Job:ScriptableObject
    {
        public int Power => power;

        public int Speed => speed;

        public int Life => life;

        public int Intelligence => intelligence;

        public int Movement => movement;

        public int Mana => mana;

        private new string name = "Apprentice";
        private int power = 40;
        private int speed = 40;
        private int life = 40;
        private int intelligence = 40;
        private int movement = 3;
        private int mana = 40;

        public SortedSet<Action> actions = new SortedSet<Action>();
    
    }
}