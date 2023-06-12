using UnityEngine;

namespace WFC
{
    public class WfcTextureSolution :WfcSolution
    {
        public WfcTextureSolution(int mx, int my, int t, int n, int[] observed, bool[][] wave, int[][] patterns, bool periodic, Color32[] tiles): 
            base(mx, my, t, n, observed, wave, patterns, periodic)
        {
            this.tiles = tiles;
        }
        Color32[] tiles;
        
        override public void Save()
        {
            var output = new Texture2D(MX, MY); 
            var bitmap = new int[MX * MY];
            if (observed[0] >= 0)
            {
                for (var y = 0; y < MY; y++)
                {
                    var dy = y < MY - N + 1 ? 0 : N - 1;
                    for (var x = 0; x < MX; x++)
                    {
                        var dx = x < MX - N + 1 ? 0 : N - 1;
                        output.SetPixel(x, y, tiles[patterns[observed[x - dx + (y - dy) * MX]][dx + dy * N]]);
                    }
                }
            }
            else
            {
                for (var i = 0; i < wave.Length; i++)
                {
                    var contributors = 0;
                    int r = 0, g = 0, b = 0, a=0;
                    int x = i % MX, y = i / MX;
                    for (var dy = 0; dy < N; dy++) for (var dx = 0; dx < N; dx++)
                    {
                        var sx = x - dx;
                        if (sx < 0) sx += MX;

                        var sy = y - dy;
                        if (sy < 0) sy += MY;

                        var s = sx + sy * MX;
                        if (!periodic && (sx + N > MX || sy + N > MY || sx < 0 || sy < 0)) continue;
                        for (var t = 0; t < T; t++) if (wave[s][t])
                        {
                            contributors++;
                            var c = tiles[patterns[t][dx + dy * N]];
                            r += c.r;
                            g += c.g;
                            b += c.b;
                            a += c.a;

                        }
                    }

                    var color = MedianColor(r, g, b, a, contributors);
                    output.SetPixel(x, y, color);
                }
            }
            
            
        }

        private Color32 MedianColor(int r, int g, int b, int a, int contributors)
        {
            var c = new int[] { r, g, b, a }.ToBytes(contributors);
            return new Color32(c[0], c[1], c[2], c[3]);
        }
        
        int getContributors(int s)
        {
            var contributors = 0;
            for (var t = 0; t < T; t++)
                if (wave[s][t])
                    contributors++;
            return contributors;
        }
    }
}
