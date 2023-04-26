using System.Collections.Generic;
using UnityEngine.Tilemaps;
using UnityEngine;

namespace Character
{
    public class BattleMap
    {
        private Tilemap tilemap;
        public List<PartyController> NPCs;
        public PartyController Player;

        public Vector3Int start;
        public Vector3Int end;
        
        
    }
}