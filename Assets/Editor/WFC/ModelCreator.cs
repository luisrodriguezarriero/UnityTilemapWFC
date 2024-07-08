namespace WFC
{
    public abstract class ModelCreator
    {
        protected ModelCreator(int N, bool periodic, bool ground)
        {
            this.N = N;
            this.periodic = periodic;
            this.ground = ground;
        }
        
        protected int[][][] propagator;
        
        protected int T, N;
        protected bool periodic, ground;
        
        public double[] weights;

        public abstract void Save(UnityEngine.Tilemaps.Tilemap outout, int[] observed, int MX, int MY);

    }
}