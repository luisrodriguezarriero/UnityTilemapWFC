using System.Collections.Generic;

namespace SnakeGame
{
    public class ZonesDelimiter
    {
        private List<(int, int)> biggestZone;
        private List<(int, int)> banlist;
        
        public ZonesDelimiter(bool[][] grid)
        {
            this.grid = grid;
            biggestZone = new List<(int, int)>();
            banlist = new List<(int, int)>();
            
            for (var i = 0; i < grid.Length; i++)
            {
                for (var j = 0; j < grid[i].Length; j++)
                {
                    if (HasBeenSeen(i, j)) continue;
                    if (grid[i][j])
                    {
                        List<(int, int)> newZone = GenerateZone(i, j);
                        if (newZone.Count > biggestZone.Count)
                        {
                            banlist.AddRange(biggestZone);
                            biggestZone = new List<(int, int)>(newZone);
                        }
                    }
                    else banlist.Add((i, j));
                }
            }
        }

        public List<(int, int)> Zone => biggestZone;

        private bool HasBeenSeen(int i, int j)
        {
            return banlist.Contains((i, j)) || biggestZone.Contains((i, j));
        }

        private List<(int, int)> GenerateZone(int i, int j)
        {
            List<(int, int)> zone = new List<(int, int)>(); zone.Add((i, j));
            List<(int, int)> options = new List<(int, int)>();
            options.Add((i, j));
            while (options.Count > 0)
            {
                var (i1, j1) = options.Pop();
                
                var neighbours = GetNeighbours(i1, j1);
                foreach (var (i2, j2) in neighbours)
                {
                    if (!HasBeenSeen(i2, j2))
                    {
                        if (grid[i2][j2] && !zone.Contains((i2, j2)))
                        {
                            zone.Add((i2, j2));
                            options.Add((i2, j2));
                        }
                        else banlist.Add((i2, j2));
                    }
                }
            }
            return zone;
        }

        private List<(int, int)> GetNeighbours(int i, int j)
        {
            List<(int, int)> neighbours = new List<(int, int)>();
            
            if(i<grid.Length-1) neighbours.Add((i+1, j));
            if(j<grid[i].Length-1) neighbours.Add((i, j+1));
            if(i>1) neighbours.Add((i-1, j));
            if(j>1) neighbours.Add((i, j-1));
            
            return neighbours;
        }
        
        private bool[][] grid;
    }
}