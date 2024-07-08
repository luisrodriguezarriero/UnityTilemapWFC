using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;
using WFC.Tilemap;
using UnityEditor;
using AudioUtilities;

namespace Snake
{
    public class GameManager : MonoBehaviour
    {
        private GameStateManager gameStateManager;
        readonly int xSize = 19, ySize = 14;
        public GameObject playerPrefab;
        public GameObject foodPrefab;
        public WFC.Tilemap.Overlap.TilemapGenerator mapGenerator;
        private GameObject player;
        private List<GameObject> foodList; 
        private readonly Random seed = new Random();
        private void Awake() => Setup();
        public void GameOver()
        {
            if (gameStateManager.isPaused())  return;

            gameStateManager.GameOver();

            Destroy();
        }   
        private void Destroy()
        {            
            if(player) Destroy(player);

            foreach (var food in foodList) Destroy(food);
        }
        public void Restart()
        {
            Destroy();

            player.GetComponent<Snake>().Reset();

            Setup();
            gameStateManager.Resume();    

        }
        public void Setup()
        {
            gameStateManager = GameStateManager.GetInstance();
            gameStateManager.Setup();

            GenerateMap();

            int level = GetPunctuation();
            var nFoods = seed.Next(level +1, level + 4);

            var setup = SnakeSetup.Instance;
            setup.Seed = seed;

            bool[][] grid = setup.GetWalkableLocations(mapGenerator.outputGrid);

            var placeableZones = new ZonesDelimiter(grid).Zone;
            
            List<Vector2> locations = setup.GetNPlaceableLocations(placeableZones, nFoods + 1);

            var playerLocation = locations.Pop();
            
            player = setup.PlacePrefab(playerLocation, playerPrefab, setup.SnakeParent);
            foodList = setup.MassPlacePrefab(locations.ToArray(), foodPrefab, setup.FoodParent);

            player.gameObject.SetActive(true);
            gameStateManager.Resume();
        }

        private int GetPunctuation()
        {
            throw new System.NotImplementedException();
        }

#if UNITY_EDITOR
        public void TestSetup(){
            DestroyFoodAndPlayer();
            Setup();
        }

        void DestroyFoodAndPlayer(){
            DestroyImmediate(player);
            foreach(GameObject food in foodList) DestroyImmediate(food);
        }

#endif

        private void GenerateMap()
        {
            mapGenerator.MX = xSize; 
            mapGenerator.MY = ySize;
            mapGenerator.CreateMap();
            mapGenerator.Solve();
        }

        private void NextLevel()
        {
            Destroy();
            Setup();
        }

        public void RemoveFood(Food food)
        {
            foodList.Remove(food.gameObject);
            Destroy(food.gameObject);
            if (foodList.Count == 0) NextLevel();
        }
        
    }

#if  UNITY_EDITOR

    [CustomEditor(typeof(GameManager))]

    public class GameInspector : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            GameManager myScript = (GameManager)target;
            if (GUILayout.Button("Setup"))
                {
                    myScript.TestSetup();
                }
        }
    }

#endif
}