using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "2D/Tiles/Auto Override Rule Tile")]
public class CustomRuleTile : Tile
{
    [SerializeField] List<Sprite> _UncommonSources;
    [SerializeField] Sprite _CommonSource;

    [Tooltip("If zero, will equal the number of sources")]

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        base.GetTileData(position, tilemap, ref tileData);
        



    }
}