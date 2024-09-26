using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using WFC;

namespace WFC.Setup
{
    abstract public class MapGenerator : MonoBehaviour{
        [Header("Input image settings")]
        [Tooltip("Pattern size, recommended values between 2 and 4")]
        [Range(1, 8)]
        public int N = 2;        
        [Range(1, 8)]
        public int symmetry = 1;
        [Tooltip("This should be true if you dont know what you're doing, or the image is relatively small")]
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
        
        protected Model model;
        internal void GenerateSeed()
        {
            var s = new System.Random();
            seed = keepSeed? seed: s.Next();
        }
        public bool modelExists => model != null;
        public abstract void CreateMap();
        public abstract void CreateModel();
        public abstract void Solve();
        protected virtual WFC.Solution.Solver initSolver(int MX, int MY, bool periodicOutput){
            switch (heuristic)
            {
                case Heuristic.Entropy:
                    return new WFC.Solution.Entropy(model, MX, MY, periodicOutput);
                case Heuristic.MRV:
                    return new WFC.Solution.MRV(model, MX, MY, periodicOutput);
                case Heuristic.Scanline:
                default: 
                    return new WFC.Solution.Scanline(model, MX, MY, periodicOutput);
            }
        }

        public abstract object getOutput();
    }

    public static class timeTestUtils
    {
        public static bool Testing = false;

        public static void printTimeStamp(string s = ""){
            if(!Testing) return;
            string timestamp = DateTime.UtcNow.ToString("HH:mm:ss.fff");
            Debug.Log($"{s} Time: {timestamp}");
        }

        public static void printTimeDifference(DateTime start, DateTime end, string TaskName = ""){
            if(!Testing) return;
            var timestamp = end.Subtract(start).TotalMilliseconds;
            Debug.Log($"{TaskName} Time: {timestamp} milliseconds");
        }

        public static DateTime getNow(){
            return DateTime.UtcNow;
        }
    }
}