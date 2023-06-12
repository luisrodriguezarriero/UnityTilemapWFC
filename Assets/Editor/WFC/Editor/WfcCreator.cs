using UnityEngine;
using System.Collections;

namespace WFC
{
    abstract public class WfcCreator : MonoBehaviour{
        [Tooltip("Recommended values between 2 and 4")]
        public int tileSize;
        [Tooltip("How many times algorithm will try creating the output before quiting")]
        public int maxIterations;
        [Tooltip("Output image width")]
        public int outputWidth = 5;
        [Tooltip("Output image height")]
        public int outputHeight = 5;
        [Tooltip("Between 1 and 8, any other value will be tweaked")]
        public int simmetry = 1;
        [Tooltip("")]
        public bool periodicInput, periodic;
        [Tooltip("")]
        public bool ground;
        public Heuristic heuristic;
        protected int seed;

        internal void GenerateSeed()
        {
            var s = new System.Random();
            seed = s.Next();
        }

        protected void checkSimmetry(){
            if (simmetry < 1) simmetry = 1;
            if (simmetry > 8) simmetry = 8;
        }

        public abstract bool modelExists();
        public abstract bool hasInput();

        public abstract void CreateMap();
        public abstract void CreateModel();
        public abstract void Solve();
    }
}