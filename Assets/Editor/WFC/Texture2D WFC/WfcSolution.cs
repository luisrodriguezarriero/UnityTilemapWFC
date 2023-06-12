namespace WFC
{
    public abstract class WfcSolution{
        public WfcSolution(int mx, int my, int t, int n, int[] observed, bool[][] wave, int[][] patterns, bool periodic) 
        {
            MX = mx;
            MY = my;
            T = t;
            N = n;
            this.observed = observed;
            this.wave = wave;
            this.patterns = patterns;
            this.periodic = periodic;
        }

        protected int MX, MY, T, N;
        protected int[] observed;
        protected bool[][] wave;
        protected int[][] patterns;
        protected bool periodic;
        
        abstract public void Save();
    }
}
