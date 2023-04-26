using System.Collections.Generic;
using UnityEngine;

namespace ProceduralDungeon{
    public static class WallGenerator
    {
        public static void CreateWalls(HashSet<Vector2Int> floorPositions, TilemapVisualizer tilemapVisualizer)
        {
            var basicWallPos = FindWallsInDrections(floorPositions, Direction2D.cardinalDirectionsList);
            foreach (var position in basicWallPos)
            {
                tilemapVisualizer.PaintSimpleBasicWall(position);
            }
        }

        private static HashSet<Vector2Int> FindWallsInDrections(HashSet<Vector2Int> floorPositions, List<Vector2Int> directionList)
        {
            HashSet<Vector2Int> wallPositions = new HashSet<Vector2Int>();
            foreach (var position in floorPositions)
            {
                foreach (var direction in directionList)
                {
                    var neighbourPosition = position + direction;
                    if (!floorPositions.Contains(neighbourPosition))
                        wallPositions.Add(neighbourPosition);
                }
                
            }
            return wallPositions;
        }
    }
}

