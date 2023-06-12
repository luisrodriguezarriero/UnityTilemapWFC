
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;

namespace WFC
{
    public abstract class TilemapWfcModelCreator : WfcModelCreator
    {
        protected TilemapWfcModelCreator(int N, bool periodic, bool ground = false) : base(N, periodic, ground) {}
        public abstract void Save(Tilemap outout, int[] observed, int MX, int MY);
    }
}
