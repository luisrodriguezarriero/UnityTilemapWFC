using UnityEngine;
using System.Collections;

namespace WFC
{
    abstract public class WfcCreator : MonoBehaviour{
        [Header("Input image settings")]
        [Tooltip("Pattern size, recommended values between 2 and 4")]
        [Range(1, 8)]
        public int N = 2;        
        [Range(1, 8)]
        public int simmetry = 1;
        [Tooltip("This should be true if you dont know what you're doing, or the image is relatively small")]
        public bool rotate;
        public bool mirrorX;
        public bool mirrorY;
        public bool periodicInput = true;
        [Tooltip("This will not be taken into account in many cases")]
        public bool ground = false;
        [Header("Solver settings")]
        public Heuristic heuristic = Heuristic.Entropy;
        [Tooltip("How many times algorithm will try creating the output before quiting \n If 0 or lower, it will try the current maximum number, MX * MY")]
        public int maxIterations;
        [SerializeField]
        protected int seed;
        public bool keepSeed = false;
        [Header("Output settings")]
        [Tooltip("Output image width")]
        public int MX = 19;
        [Tooltip("Output image height")]
        public int MY = 14;
        [Tooltip("True if you dont know what you're doing")]
        public bool periodicOutput = true;
        
        protected WfcModel model;
        internal void GenerateSeed()
        {
            var s = new System.Random();
            seed = keepSeed? seed: s.Next();
        }
        public bool modelExists => model != null;
        public abstract void CreateMap();
        public abstract void CreateModel();
        public abstract void Solve();
        protected virtual WfcSolver initSolver(int MX, int MY, bool periodicOutput){
            switch (heuristic)
            {
                case Heuristic.Entropy:
                    return new EntropySolver(model, MX, MY, periodicOutput);
                case Heuristic.MRV:
                    return new MRVSolver(model, MX, MY, periodicOutput);
                case Heuristic.Scanline:
                default: 
                    return new ScanlineSolver(model, MX, MY, periodicOutput);
            }
        }
    }
}