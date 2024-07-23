using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;
using WFC.Tilemap.Overlap;
using System;
using UnityEditor.VersionControl;

namespace Snake.Setup
{
    public class TilemapGenerator : MapGenerator
    {
        [Header("Tilemap Objects")]
        public UnityEngine.Tilemaps.Tilemap inputGrid;
        public UnityEngine.Tilemaps.Tilemap outputGrid;

        public bool Testing = false;
        protected OverlappingModelCreator wfc;

        public override void CreateModel()
        {    
            var before = timeTestUtils.getNow();
            wfc = new OverlappingModelCreator(inputGrid, N,
                periodicInput, symmetry, false);
            model = wfc.model;
            timeTestUtils.printTimeDifference(before, timeTestUtils.getNow(), "CreateModel");
            
        }
        public override void Solve()
        {
            GenerateSeed();
            ClearOutput();

            var before = timeTestUtils.getNow();
                
            var solver = initSolver(MX, MY, periodicOutput);
            if (solver.Run(seed, maxIterations > 0 ? maxIterations : int.MaxValue))
            {
                timeTestUtils.printTimeDifference(before, timeTestUtils.getNow(), "Model Solved ");
                var beforeSave = timeTestUtils.getNow();
                wfc.Save(outputGrid, solver.Result, MX, MY);
                timeTestUtils.printTimeDifference(beforeSave, timeTestUtils.getNow(), "Model Drawn ");
            }
            else {
                Debug.Log("Fail");
                timeTestUtils.printTimeDifference(before, timeTestUtils.getNow(), "Fail to create model");
            }
        }
        



        private void p(){
            if(!Testing) return;
            string timestamp = DateTime.UtcNow.ToString("HH:mm:ss.fff");
            Debug.Log($"");
        }

        protected virtual void ClearOutput()
        {
            if (outputGrid)
            {
                outputGrid.ClearAllTiles();
            }
        }

        public override void CreateMap()
        {
            if(!inputGrid || !outputGrid) 
            {
                Debug.LogError("INPUT AND OUTPUT MUST NOT BE NULL");
                return;
            }
            
            inputGrid.CompressBounds();
            outputGrid.CompressBounds();

            CreateModel();
        }

        public override object getOutput()
        {
            return outputGrid;
        }
    }
}
