using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "2D/Tiles/PeriodicTile")]
public class PeriodicTile : Tile
{
    [Tooltip("If you enter any number of sources that is not a multiple of the xPeriod, you may lose some of the last ones")]
    [SerializeField] List<Sprite> _sources;

    [Tooltip("If zero, will equal the number of sources")]
    [Min(0)]
    [SerializeField] int xPeriod = 1;
    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData){

        base.GetTileData(position, tilemap, ref tileData);

        int n = _sources.Count;

        if(n <= 0) return;

        xPeriod = xPeriod >= 1 ? xPeriod : n;
        xPeriod = xPeriod <= n ? xPeriod : n;

        int yPeriod = n / xPeriod;

        int x = Math.Abs(position.x) % xPeriod;
        int y = Math.Abs(position.y) % yPeriod;
#if UNITY_EDITOR
        //Debug.Log($"Trying to show sprite number {x * xPeriod + y} / {n}"  );
#endif
        tileData.sprite = _sources[ x * xPeriod + y ];
    }
}
