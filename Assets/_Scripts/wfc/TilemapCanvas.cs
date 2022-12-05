using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TilemapCanvas : MonoBehaviour
{
    /*paletteSource*/ 
    public Tilemap tilemap;
    public Vector3 paletteSize;
    
    /*paletteData*/
    protected bool[][] compatible;

    /* Model */
    protected int MX, MY;
    public int [][]finalMap;
    protected bool[][][] model;

    void Start()
    {   
    }

    void wfc(){
        Init();
        run();
    }
    void Init(){
        InitPalette();
        InitModel();
    }
    void InitPalette(){
        tilemap.CompressBounds();
        Vector3 paletteSize = tilemap.size;
        //TODO COMPATIBLE
    }

    void InitModel(){
        model = new bool[MX][][];
        //Init to true preti pls
        //TODO
    }

    void run(){
        
    }

    #if UNITY_EDITOR
    #endif
}

/*
While there are still cells that have multiple items in the domain:

    Pick a random cell using the “least entropy” heuristic.
    Pick a random tile from that cell’s domain, and remove all other tiles from the domain.
    Update the domains of other cells based on this new information, i.e. propagate cells. This needs to be done repeatedly as changes to those cells may have further implications.*/



