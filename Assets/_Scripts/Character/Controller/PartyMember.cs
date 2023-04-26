﻿namespace _Scripts.Character.Controller
{
    internal class PartyMember
    {
        public int ID
        {
            get => id;
            set => id = value;
        }

        public bool HasActed
        {
            get => hasActed;
            set => hasActed = value;
        }

        private int id;
        private bool hasActed;

        public PartyMember(int id)
        {
            this.id = id;
            this.hasActed = false;
        }
            
            
    }
}