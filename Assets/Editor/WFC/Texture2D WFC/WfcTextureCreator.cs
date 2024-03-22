
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static WFC.WfcUtilities;


namespace WFC.Texture
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class WfcTextureCreator : WfcCreator
    {
        private const string V = "GeneratedTexture";
        public Texture2D input;
        private Color[] colors;
        public Sprite outputFile;

        public void CreateWfcModel()
        {
            var pixelData = input.GetPixels(0);

            var p = new PatternList(ColorsAsBytes(pixelData), input.width, input.height, N, simmetry,
                periodicInput, colors.Length);

            var propagator = InitPropagator(p.patterns, p.T, N);
            
            model = new WfcTextureModel(p.T, N, periodicOutput, ground, p.weights, colors, p.patterns, propagator);
        }

        private byte[] ColorsAsBytes(Color[] allPixels)
        {
            var distinctTiles = new List<Color>();
            var inputAsInts = new byte[allPixels.Length];
            for (var i = 0; i < allPixels.Length; i++)
            {
                if (!distinctTiles.Contains(allPixels[i]))
                {
                    distinctTiles.Add(allPixels[i]);
                }
                inputAsInts[i] = (byte)distinctTiles.IndexOf(allPixels[i]);
            }
            colors = distinctTiles.ToArray();
            return inputAsInts;
        }
        
        protected int[][][] InitPropagator(List<byte[]> patterns, int T, int N)
        {
            var propagator = new int[4][][];
            for (var d = 0; d < 4; d++)
            {
                propagator[d] = new int[T][]; // Número de Patrones
                for (var t = 0; t < T; t++)
                {
                    List<int> list = new();
                    for (var t2 = 0; t2 < T; t2++)
                        if (arePatternsHardCompatible(patterns[t], patterns[t2], dx[d], dy[d], N)) list.Add(t2);
                    propagator[d][t] = list.ToArray();

                    list.Clear();
                }
            }
            return propagator;
        }

        public override void CreateModel()
        {
            CreateWfcModel();
        }

        public override void Solve(){
            var solver = initSolver(MX, MY, periodicOutput);
            GenerateSeed();
            if (solver.Run(seed, maxIterations > 0 ? maxIterations : int.MaxValue))
            {
                save(solver.observed);
            }
            else Debug.Log("Fail");
        }

        public void SolveOneStep(){
            var solver = initSolver(MX, MY, periodicOutput);
            GenerateSeed();
            if (solver.RunOneStep(seed))
            {
                
            }
            else Debug.Log("Fail");
        }
        
        public override void CreateMap(){
            CreateWfcModel();
            Solve();
        }

        void save(int [] data){
            var texture = new Texture2D(MX, MY);
            texture.name= V;
            for (int y = 0; y < MY; y++)
            {
                int dy = y < MY - N + 1 ? 0 : N - 1;
                for (int x = 0; x < MX; x++)
                {
                    int dx = x < MX - N + 1 ? 0 : N - 1;
                    texture.SetPixel(x, y, colors[model.patterns[data[x - dx + (y - dy) * MX]][dx + dy * N]]);
                }
            }

            Rect rect = new Rect(0, 0, 1, 1);
            setOutput(Sprite.Create(texture, rect, new Vector2(MX/2, MY/2)));
        }

        public void setOutput(Sprite s){
            var renderer = GetComponent<SpriteRenderer>();
            if(renderer != null) renderer.sprite = s;
        }

    }

#if UNITY_EDITOR

    [CustomEditor(typeof(WfcTextureCreator))]

    public class WfcTextureInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            WfcTextureCreator myScript = (WfcTextureCreator)target;
            if (myScript.input)
            {
                if(myScript.modelExists){
                    if (GUILayout.Button("Solve from model"))
                    {
                        myScript.Solve();
                    }
                }
                
                if (GUILayout.Button("CreateModel"))
                {
                    myScript.CreateModel();
                }

                if (GUILayout.Button("New Object & Map"))
                {
                    myScript.CreateMap();
                }

            }


        }
    }
#endif
}


