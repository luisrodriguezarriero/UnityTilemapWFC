using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static WFC.WfcUtilities;

namespace WFC.Tiled 
{
    [CreateAssetMenu(fileName = "SimpleTiledData", menuName = "2D/WFC/WfcSimpleTiledData", order = 100)]
    class WfcSimpleTiledData : ScriptableObject
    {
        [SerializeField] List<WfcSprite> Sprites = new List<WfcSprite>();

        internal List<WfcSprite> Sprites1 { get => Sprites; private set => Sprites = value; }
    }

    [CreateAssetMenu(fileName = "WfcSprite", menuName = "2D/WFC/WfcSprite", order = 1)]

    class WfcSprite : ScriptableObject
    {
        [SerializeField] List<Sprite> Sprites = new List<Sprite>();
        [SerializeField] List<WfcSprite> rightSprites = new List<WfcSprite>();
        [SerializeField] List<WfcSprite> downSprites = new List<WfcSprite>();
        [SerializeField] public float weight {get; private set;} = 1.0f;
        [SerializeField] public SymmetryClass simmetryClass {get; private set;} = SymmetryClass.X ;
        public List<Sprite> Sprites1 { get => Sprites; private set => Sprites = value; }
        public List<WfcSprite> RightSprites { get => rightSprites; private set => rightSprites = value; }
        public List<WfcSprite> DownSprites { get => downSprites; private set => downSprites = value; }
    }

    static class WFCTileHelper
    {
        public static Vector3Int[] Get2dNeighbours(this Vector3Int position)
        {
            Vector3Int[] Neighbours = new Vector3Int[4];
            for (int i = 0; i < 4; i++)
            {
                Neighbours[i] = position + new Vector3Int(dx[i], dy[i], 0);
            }
            return Neighbours;
        }

    }

    internal enum SymmetryClass{X, L, I, T, F}
}

