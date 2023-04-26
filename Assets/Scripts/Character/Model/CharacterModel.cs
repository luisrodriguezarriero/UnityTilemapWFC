using UnityEngine;
using System.Collections.Generic;

namespace Character
{
    public abstract class 
        CharacterModel{       
        private int power;
        private int speed;
        private int life;
        private int intelligence;
        private int mana;
        
        private long id;
        private static long idCounter = 0;
        
        public byte level;
        private int exp = 0;
        
        public string name;
        
        public Job job;
        protected static Job defaultJob = new Job();

        public long ID => id;
        public static long IDCounter => idCounter;
        public int Power => power;
        public int Speed => speed;
        public int Life => life;
        public int Intelligence => intelligence;
        public int Mana => mana;
        protected CharacterModel(byte level, Job job, string name)
        {
            this.exp = 0;
            this.id = idCounter++;
            this.level = level;
            this.name = name;
            if (job != null) this.job = job;
            else this.job = defaultJob;
            calculateStats();
        }
        private int calc(int baseStat)
        {
            return (5 + level * baseStat / 13);
        }
        public void calculateStats()
        {
            power = calc(job.Power);
            speed = calc(job.Speed);
            life = (int)(2.3 * calc(job.Life));
            intelligence = calc(job.Intelligence);
            mana = 2 * calc(job.Mana);
        }


    }
}