using UnityEngine.Tilemaps;
using UnityEngine;
using System.Collections.Generic;
using WFC.Tilemap.Overlap;

namespace Snake.Setup
{
    public class TilemapGenerator : MapGenerator
    {
        [Header("Tilemap Objects")]
        public UnityEngine.Tilemaps.Tilemap inputGrid;
        public UnityEngine.Tilemaps.Tilemap outputGrid;

        protected OverlappingModelCreator wfc;

        public override void CreateModel()
        {            
            wfc = new OverlappingModelCreator(inputGrid, N,
                periodicInput, symmetry, false);
            model = wfc.model;
            
        }
        public override void Solve()
        {
            GenerateSeed();
            ClearOutput();
            var solver = initSolver(MX, MY, periodicOutput);
            if (solver.Run(seed, maxIterations > 0 ? maxIterations : int.MaxValue))
                wfc.Save(outputGrid, solver.Result, MX, MY);
            else Debug.Log("Fail");
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
