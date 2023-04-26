using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using Mono.Collections.Generic;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Character
{
    public class CombatManager:MonoBehaviour
    {
        public PartyController[] Combatants;
        public Queue<Turn> Turns;
        private Turn currentTurn = null;
        private CircularList<PartyController> turnOrder;
        
        void nextTurn()
        {
            Debug.Log("Siguiente Turno");
        }

        private void OnEnable()
        {
            if (Combatants.Length <= 0) this.enabled = false;
        }
        
        private void OnDisable()
        {
            throw new NotImplementedException();
        }

        private void Start()
        {
            OnEnable();
        }

        private void Init()
        {
            

            turnOrder = new CircularList<PartyController>(Combatants);
        }

        private void newTurn()
        {
            if(this.currentTurn != null)Turns.Enqueue(this.currentTurn);
            currentTurn = new Turn(turnOrder.next.getIds(), newTurn, pointToCharacter);
        }

        private void pointToCharacter(int id)
        {
            Debug.Log("Point to " + id);
        }
    }

    public class CircularList<Object> : List<Object>
    {
        private int index = 0;

        public Object next => this[index <= this.Count ? index++ : resetIndex()];
        public Object previous => this[index > 0 ? --index : resetIndex()];

        int resetIndex()
        {
            if (index > this.Count) index = 0;
            if (index < 0) index = this.Count - 1;
            return index;
        }

        public CircularList() : base()
        {
            index = 0;
        }
        
        public CircularList(Object[] objects) : this()
        {
            this.AddRange(objects);
        }
    }
}