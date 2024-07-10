using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;
using AudioUtilities;
using Snake.Setup;
using System;
using Snake.UI;

namespace Snake
{
    public class GameManager : MonoBehaviour
    {
        private GameStateManager gameStateManager;
        readonly int xSize = 19, ySize = 14;
        public GameObject playerPrefab;
        public GameObject foodPrefab;
        public MapGenerator mapGenerator;
        private GameObject player;
        private List<GameObject> foodList; 

        [Header("UI GameObjects Reference")]
        public Controller PauseMenuController;
        public Controller GameOverMenuController;
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
            
            gameStateManager.PauseUI     = PauseMenuController;
            gameStateManager.GameOverUI  = GameOverMenuController;

            gameStateManager.Setup();

            GenerateMap();

            int level = GetLevel();
            var nFoods = seed.Next(level +1, level + 4);

            var levelFactoryInstance = LevelFactory.Instance;
            levelFactoryInstance.Seed = seed;

            bool[][] grid = levelFactoryInstance.GetWalkableLocations((UnityEngine.Tilemaps.Tilemap)mapGenerator.getOutput());

            var placeableZones = new Setup.ZonesDelimiter(grid).Zone;
            
            List<Vector2> locations = levelFactoryInstance.GetNPlaceableLocations(placeableZones, nFoods + 1);

            var playerLocation = locations.Pop();
            
            player = levelFactoryInstance.PlacePrefab(playerLocation, playerPrefab, levelFactoryInstance.SnakeParent);
            foodList = levelFactoryInstance.MassPlacePrefab(locations.ToArray(), foodPrefab, levelFactoryInstance.FoodParent);

            player.gameObject.SetActive(true);
            gameStateManager.Resume();
        }

        private int GetLevel()
        {
            return GameStateManager.GetInstance().level;
        }

#if UNITY_EDITOR
        public void TestSetup(){
            if(mapGenerator.modelExists)
                DestroyFoodAndPlayer();
            else mapGenerator.CreateMap();
            Setup();
        }

        void DestroyFoodAndPlayer(){
            DestroyImmediate(player);
            if(foodList != null)
            {            
                foreach(GameObject food in foodList) DestroyImmediate(food);
                foodList=new List<GameObject>();
            }
        }

#endif

        private void GenerateMap()
        {
            mapGenerator.MX = xSize; 
            mapGenerator.MY = ySize;
            mapGenerator.CreateMap();
            mapGenerator.Solve();
        }

        private void Victory()
        {
            GameStateManager.GetInstance().Victory();
            Destroy();
            Setup();
        }

        public void RemoveFood(Food food)
        {
            foodList.Remove(food.gameObject);
            Destroy(food.gameObject);
            if (foodList.Count == 0) Victory();
        }
        
    }
}