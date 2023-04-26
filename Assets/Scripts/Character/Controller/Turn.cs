using System.Collections.Generic;
using UnityEngine.Events;

namespace Character
{
    
    public class Turn
    {
        private SortedSet<PartyMember> Party;
        UnityEvent endOfTurnEvent;
        UnityEvent<int> actorHasNotActed;
        
        int getUnusedActorID()
        {
            foreach (PartyMember member in Party)
            {
                if (!member.HasActed) return member.ID;
            }

            return -1;
        }
        
        void tryEndTurn()
        {
            int unUsedActorID = getUnusedActorID();
            if(unUsedActorID>=0)actorHasNotActed.Invoke(unUsedActorID);
                else endOfTurnEvent.Invoke();
        }
        
        public void initTurn()
        {
            foreach (PartyMember member in Party)
            {
                member.HasActed = false;
            }
        }

        public Turn(int[] ids, UnityAction endOfTurnCall, UnityAction<int> actorLeftCall)
        {
            Party = new SortedSet<PartyMember>();
            
            foreach (int id in ids)
            {
                Party.Add(new PartyMember(id));
            }
            
            endOfTurnEvent = new UnityEvent();
            actorHasNotActed = new UnityEvent<int>();
            
            endOfTurnEvent.AddListener(endOfTurnCall);
            actorHasNotActed.AddListener(actorLeftCall);
        }
    }
}