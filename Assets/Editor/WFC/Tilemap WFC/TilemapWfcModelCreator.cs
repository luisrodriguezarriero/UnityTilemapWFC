
using UnityEngine.Tilemaps;

namespace WFC.TiledWFC
{
    public abstract class TilemapWfcModelCreator : WfcModelCreator
    {
        protected TilemapWfcModelCreator(int N, bool periodic, bool ground = false) : base(N, periodic, ground) {}
        public abstract void Save(Tilemap outout, int[] observed, int MX, int MY, Tilemap floorGrid = null);
    }
}
