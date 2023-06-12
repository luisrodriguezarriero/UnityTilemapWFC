
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using Random = System.Random;
using WFC;

namespace SnakeGame{
    public class GameManager : MonoBehaviour
    {
        public AudioClip pause;
        public AudioClip resume;
        public AudioClip select;
        public AudioClip victory;
        
        public Vector2 baseForPrefabs = new Vector2(-5.5f, -4.5f);
        bool gameIsOver;

        public GameObject pauseMenuUI;

        public GameObject gameOverUI;

        private static bool _gamePaused;
        
        public GameObject playerPrefab;
        public GameObject foodPrefab;
        public SnakeMapEditor mapGenerator;
        private static int _level;

        private GameObject player;

        private List<GameObject> foodList; 
        public string walkableTileName;

        private readonly Random seed = new Random();
        
        public Tilemap tilemap;
        public void GameOver()
        {
            if (gameIsOver) 
                return;
            gameIsOver=true;
            Time.timeScale = 0f;
            gameOverUI.SetActive(true);
            Destroy(player);
            player.transform.position = new Vector3(0.0f, 0f, 100.0f); 
            foreach (var food in foodList)
            {
                Destroy(food);
            }
            
        }
        
        public void Restart()
        {
            gameIsOver = false;
            StartCoroutine(PlaySound(select));
            _level = 0;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            
            Time.timeScale = 1f;
        }

        private void Pause(){    
            StartCoroutine(PlaySound(pause));
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            _gamePaused=true;  
        }

        public void Resume(){       
            StartCoroutine(PlaySound(resume));
            pauseMenuUI.SetActive(false);
            gameOverUI.SetActive(false);
            Time.timeScale = 1f;
            _gamePaused=false;  
        }

        public void Quit(){       
            StartCoroutine(PlaySound(select));

            Time.timeScale = 1f;
            SceneManager.LoadScene(0);
        }

        public void Update()
        {
            if (gameIsOver) 
                return;
            if (!Input.GetKeyDown(KeyCode.Space)) return;
            if(_gamePaused) Resume();
            else Pause();
        }

        private void Start()
        {
            _gamePaused=false;  
            gameIsOver = false;
            Snake.timeToMove = 0.4f;
            Setup();
        }

        private bool Setup()
        {
            GenerateMap();
            gameIsOver = false;
            var nFoods = seed.Next(++_level, _level + 4);
            List<(int x, int y)> walkableLocations;
            bool[][] grid;
            (walkableLocations, grid) = GetWalkableLocations();

            var placeableZones = new ZonesDelimiter(grid).Zone;
            
            List<(int x, int y)> locations = GetNPlaceableLocations(placeableZones, nFoods + 1);

            var playerLocation = locations.Pop();

            //SolutionFinder finder = new SolutionFinder(locations, playerLocation, grid);
            //if (!finder.DoesSolutionExist()) return false; 
            
            player = PlacePrefab(playerLocation, playerPrefab);
            foodList = MassPlacePrefab(locations.ToArray(), foodPrefab);

            player.SetActive(true);
            return true;
        }

        private GameObject PlacePrefab((int x, int y) location, GameObject prefab)
        {
            var newObject = Instantiate(prefab);
            newObject.transform.localPosition = new Vector3(location.x+baseForPrefabs.x, location.y+baseForPrefabs.y, 0);
            return newObject;
        }
        
        private List<GameObject> MassPlacePrefab((int x, int y)[] locations, GameObject prefab)
        {
            return locations.Select(location => PlacePrefab((location.x, location.y), prefab)).ToList();
        }

        private (int x, int y) GetPlaceableLocation(List<(int, int)> walkableLocations)
        {
            var index = seed.Next(walkableLocations.Count-1);
            var (x, y) = walkableLocations[index];
            walkableLocations.RemoveAt(index);
            return (x, y);
        }
        
        private List<(int x, int y)> GetNPlaceableLocations(List<(int, int)> walkableLocations, int n)
        {
            var locations = new List<(int x, int y)>();
            for (var i = 0; i < n; i++)
            {
                locations.Add(GetPlaceableLocation(walkableLocations));
            }

            return locations;
        }

        private (List<(int, int)>, bool[][]) GetWalkableLocations()
        {
            var walkableLocations = new List<(int, int)>();
            var generatorOutput = mapGenerator.output;
            var bounds = new BoundsInt();
            var cellBounds = generatorOutput.cellBounds;
            bounds.SetMinMax(new Vector3Int(cellBounds.xMin, cellBounds.yMin, 0), 
                new Vector3Int(cellBounds.xMax, cellBounds.yMax, 0));
            var xSize = cellBounds.xMax - cellBounds.xMin;
            var ySize = cellBounds.yMax - cellBounds.yMin;
            var grid = new bool [xSize][];
            var xMin = cellBounds.xMin; var yMin = cellBounds.yMin;
            
            for (var i =  0; i <xSize; i++)
            {
                grid[i] = new bool[ySize];
                
                for (var j = 0; j < ySize; j++)
                {
                    var tile = generatorOutput.GetTile(new Vector3Int(i + xMin, j + yMin, 0));
                    if (tile == null) 
                            Debug.Log($"FailedTile: X = {i}, Y = {j}");
                    else 
                    if (tile.name == walkableTileName)
                    {
                        walkableLocations.Add((i, j));
                        grid[i][j] = true;
                    }
                }
            }

            return (walkableLocations, grid);
        }

        private void GenerateMap()
        {
            mapGenerator.CreateMap(0);
        }

        private void NextLevel()
        {
            foreach (var segment in player.GetComponent<Snake>().segments)
            {
                Destroy(segment.gameObject);
            }
            Destroy(player.gameObject);
            StartCoroutine(PlaySound(victory));
            Setup();
        }

        public void RemoveFood(Food food)
        {
            foodList.Remove(food.gameObject);
            Destroy(food.gameObject);
            if (foodList.Count == 0) NextLevel();
        }
        private IEnumerator PlaySound(AudioClip sound)
        {
            AudioSource audio = GetComponent<AudioSource>();
            audio.clip = sound;
            audio.Play();
            yield return new WaitForSecondsRealtime(audio.clip.length);
        }
    }
}
