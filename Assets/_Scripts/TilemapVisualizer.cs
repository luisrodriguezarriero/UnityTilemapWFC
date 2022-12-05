using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class TilemapVisualizer : MonoBehaviour
{
    [SerializeField]
    private Tilemap floorTilemap;

    private TileBase floorTile;
    public void PaintFloorTiles(IEnumerable<Vector2Int> floorPositions)
    {
        PaintTiles(floorPositions, floorTilemap, floorTile);
    }

    private void PaintTiles(IEnumerable<Vector2Int> floorPositions, Tilemap tilemap, TileBase tile)
    {
        foreach (var position in floorPositions)
            PaintSingleTile(tilemap, tile, position);
    }

    private void PaintSingleTile(Tilemap tilemap, object tile, Vector2Int position)
    {
        var tilePosition = tilemap.WorldToCell((Vector3Int)position);

    }
}
