using System.Collections.Generic;
using UnityEngine;
using static Snake.Utilities;

namespace Snake.Setup
{
    public class ZonesDelimiter
    {
        private List<Vector2> biggestZone;
        private List<Vector2> banlist;
        public ZonesDelimiter(bool[][] grid)
        {
            this.grid = grid;
            biggestZone = new List<Vector2>();
            banlist = new List<Vector2>();
            
            for (var i = 0; i < grid.Length; i++)
            {
                for (var j = 0; j < grid[i].Length; j++)
                {
                    if (HasBeenSeen(i, j)) continue;
                    if (grid[i][j])
                    {
                        List<Vector2> newZone = GenerateZone(i, j);
                        if (newZone.Count > biggestZone.Count)
                        {
                            banlist.AddRange(biggestZone);
                            biggestZone = new List<Vector2>(newZone);
                        }
                    }
                    else banlist.Add((i, j));
                }
            }
        }

        public List<Vector2> Zone => biggestZone;

        private bool HasBeenSeen(Vector2 v)
        {
            return banlist.Contains(v) || biggestZone.Contains(v);
        }

        private bool HasBeenSeen(int i, int j) => HasBeenSeen(new(i, j));
        private List<Vector2> GenerateZone(int i, int j) => GenerateZone(new(i, j));
        private List<Vector2> GenerateZone(Vector2 vector)
        {
            List<Vector2> zone = new List<Vector2>(); 
            zone.Add(vector);
            List<Vector2> options = new List<Vector2>();
            options.Add(vector);
            while (options.Count > 0)
            {
                var option = options.Pop();
                
                var neighbours = GetNeighbours(option);
                foreach (var neighbour in neighbours)
                {
                    if (!HasBeenSeen(neighbour))
                    {
                        int x= (int)neighbour.x; int y= (int)neighbour.y;
                        if (grid[x][y] && !zone.Contains((x, y)))
                        {
                            zone.Add((x, y));
                            options.Add((x, y));
                        }
                        else banlist.Add((x, y));
                    }
                }
            }
            return zone;
        }

        private List<Vector2> GetNeighbours(Vector2 v)
        {
            int i = (int)v.x; int j = (int)v.y;
            List<Vector2> neighbours = new List<Vector2>();
            
            if(i<grid.Length-1) neighbours.Add(new(i+1, j));
            if(j<grid[i].Length-1) neighbours.Add(new(i, j+1));
            if(i>1) neighbours.Add(new(i-1, j));
            if(j>1) neighbours.Add(new(i, j-1));
            
            return neighbours;
        }
        
        private bool[][] grid;
    }
}