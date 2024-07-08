using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using static WFC.Utilities;

namespace WFC.Tilemap.SimpleTiled
{
    [CreateAssetMenu(fileName = "SubSet", menuName = "2D/WFC/SimpleTiledModel/SubSet", order = 100)]
    class SubSet : ScriptableObject
    {
        [SerializeField] private List<WfcSprite> sprites = new List<WfcSprite >();

        public List<WfcSprite> Sprites { get => sprites; private set => sprites = value; }

    }

    [CreateAssetMenu(fileName = "WFCTile", menuName = "2D/WFC/SimpleTiledModel/Sprite", order = 1)]
    class WfcSprite : ScriptableObject
    {
        [SerializeField] UnityEngine.Tilemaps.TileBase tile;
        [SerializeField] List<WfcSprite> rightSprites;
        [SerializeField] public SymmetryClass simmetry {get; private set;} = SymmetryClass.X ;
        public List<WfcSprite> RightSprites { get => rightSprites; private set => rightSprites = value; }

        public List<WfcSprite> GetCardinality()
        {
            switch (this.simmetry)
            {
                case SymmetryClass.L:
                    return this.GetCardinalityTypeL();
                case SymmetryClass.X:
                default:
                    var result = new List<WfcSprite>
                    {
                        this
                    };
                    return result;
            }
        }

        internal List<WfcSprite> GetCardinalityTypeL()
        {
            throw new NotImplementedException();
        }
    }

    internal enum SymmetryClass
    {
        X = 1,
        L = 4,
        I = 2,
        T = 4,
        F = 8,
        N = 2 // Equivalent to '//'
    }

    static class Utilities {

    }
}

