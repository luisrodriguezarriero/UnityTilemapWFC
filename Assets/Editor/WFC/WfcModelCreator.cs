namespace WFC
{
    public abstract class WfcModelCreator
    {
        protected WfcModelCreator(int N, bool periodic, bool ground)
        {
            this.N = N;
            this.periodic = periodic;
            this.ground = ground;
        }
        
        protected int[][][] propagator;
        
        protected int T, N;
        protected bool periodic, ground;
        
        public double[] weights;
    
        public int[][][] Propagator => propagator;

        public int nTiles => T;

        public int tileSize => N;

        public bool Periodic => periodic;

        public bool Ground => ground;

        public double[] Weights => weights;
    }
}