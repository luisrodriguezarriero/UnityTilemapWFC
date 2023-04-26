using System;
using UnityEngine;

namespace Character
{
    public class CharacterController:MonoBehaviour
    {
        private Animator a;
        public CharacterModel model;
        private void Start()
        {
            if (this.model == null) throw new NullReferenceException("Character doesnt exist");
        }

        private void Update()
        {
            throw new NotImplementedException();
        }

        public void move(Direction d)
        {
            //a.parameters[0]. = (int)d;
        }
        public enum Direction{NONE, UP, DOWN, LEFT, RIGHT}
    }
}