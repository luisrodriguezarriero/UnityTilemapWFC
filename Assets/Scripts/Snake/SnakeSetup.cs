using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = System.Random;
using UnityEditor;

namespace Snake
{
    public class SnakeSetup {

        private static SnakeSetup _instance;

        public static SnakeSetup Instance 
        { 
            get 
            { 
                if (_instance == null)
                {
                    _instance = new SnakeSetup();
                }
                return _instance; 
            } 
        }

        readonly Vector2 baseLocationForPrefabs = new Vector2(-8.5f, -6.5f);
        private Random seed;
        [SerializeField] Transform foodParentTransform;
        [SerializeField] Transform snakeParentTransform;
        private static readonly string FOOD_PARENT_NAME = "== FOOD PARENT ==";
        private static readonly string SNAKE_PARENT_NAME = "== SNAKE PARENT ==";

        public Transform FoodParent 
        { 
            get => foodParentTransform? foodParentTransform: getParent(FOOD_PARENT_NAME);
            set => foodParentTransform = value; 
        }
        public Transform SnakeParent
        { 
            get => snakeParentTransform? snakeParentTransform: getParent(SNAKE_PARENT_NAME);
            set => snakeParentTransform = value; 
        }
        public Random Seed { get => seed; set => seed = value; }
        private Transform getParent(string name){
            Transform parentTransform = GameObject.Find(name).transform;
            if(!parentTransform){
                var newParentGameObject = Object.Instantiate(new GameObject());
                newParentGameObject.name = name;
                parentTransform = newParentGameObject.transform;
            }
            return parentTransform;
        }
        internal GameObject PlacePrefab(Vector2 location, GameObject prefab, Transform Parent)
        {
            var newObject = Object.Instantiate(prefab);
            newObject.transform.localPosition = new Vector3(location.x+baseLocationForPrefabs.x, location.y+baseLocationForPrefabs.y, 0);
            newObject.transform.SetParent(Parent);
            return newObject;
        }
        
        internal List<GameObject> MassPlacePrefab(Vector2[] locations, GameObject prefab, Transform parent)
        {
            return locations.Select(location => PlacePrefab(location, prefab, parent)).ToList();
        }

        internal Vector2 GetPlaceableLocation(List<Vector2> walkableLocations)
        {
            var index = Seed.Next(walkableLocations.Count-1);
            var location = walkableLocations[index];
            walkableLocations.RemoveAt(index);
            return location;
        }
        
        internal List<Vector2> GetNPlaceableLocations(List<Vector2> walkableLocations, int n)
        {
            var locations = new List<Vector2>();
            for (var i = 0; i < n; i++)
            {
                locations.Add(GetPlaceableLocation(walkableLocations));
            }

            return locations;
        }

        internal bool[][] GetWalkableLocations(Tilemap generatedOutput)
        {
            var xMin = generatedOutput.cellBounds.xMin; var yMin = generatedOutput.cellBounds.yMin;
            var xSize = generatedOutput.cellBounds.size.x; var ySize = generatedOutput.cellBounds.size.y;


            var grid = new bool [xSize][];
            for (var i =  0; i <xSize; i++)
            {
                grid[i] = new bool[ySize];
                
                for (var j = 0; j < ySize; j++)
                {
                    var tile = generatedOutput.GetTile(new Vector3Int(i + xMin, j + yMin, 0));
                    if (tile == null) 
                            Debug.Log($"FailedTile: X = {i}, Y = {j}");
                    else 
                    if (tile.isWalkable())
                    {
                        grid[i][j] = true;
                    }
                }
            }

            return grid;
        }

    }    
}