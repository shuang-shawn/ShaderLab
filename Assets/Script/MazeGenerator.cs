using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class MazeGenerator : MonoBehaviour
{
    public static MazeGenerator mazeGenerator;
    public int width = 5; // Maze width
    public int height = 5; // Maze height
    // public GameObject wallPrefab; // Prefab for the wall
    public GameObject horizontalWallPrefab; // Prefab for the wall
    public GameObject verticalWallPrefab; // Prefab for the wall
    public GameObject floorPrefab; // Prefab for the floor
    public GameObject player; // Reference to the player object
    public GameObject exit; // Reference to the exit object
    public GameObject monsterPrefab;
    public GameObject doorPrefab;

    private Cell[,] maze; // 2D array representing the maze
    private List<Vector2Int[]> directionSequence; // Stores pre-generated direction sequences
    private List<GameObject> walls = new List<GameObject>();
    private bool isPassable = false;
    public GameObject currentMonster;
    private void Awake() {
        if (mazeGenerator == null)
        {
            mazeGenerator = this;
        } else if (mazeGenerator != this)
        {
            Destroy(gameObject); // Ensure only one instance exists
        }

        DontDestroyOnLoad(gameObject); // Optional: persists across scenes
        SceneManager.sceneLoaded += OnSceneLoaded; // Register event
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unregister event
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Check if the maze scene is loaded
        if (scene.name == "Maze") // Replace "MazeScene" with your actual maze scene name
        {
            SetUpPlayerEnemy();
        }
    }

    private void Start()
    {
        GenerateDirectionSequence();
        InitializeMaze();
        GenerateMazeLayout();
        PositionPlayerAndExit();
    }

    // Cell struct to store wall information
    private struct Cell
    {
        public bool visited;
        public bool northWall, southWall, eastWall, westWall;

        public Cell(bool visited)
        {
            this.visited = visited;
            northWall = southWall = eastWall = westWall = true; // Start with walls on all sides
        }
    }

    void InitializeMaze()
    {
        maze = new Cell[width, height];
        
        // Initialize all cells as unvisited with all walls intact
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                maze[x, y] = new Cell(false);
                Instantiate(floorPrefab, new Vector3(x, 0, y), Quaternion.identity, transform);
            }
        }
    }

    // Pre-generate the sequence of directions for each cell
    void GenerateDirectionSequence()
    {
        directionSequence = new List<Vector2Int[]>();

        // Define possible directions: up, down, left, right
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        // Create a sequence for each cell in the maze
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector2Int[] shuffledDirections = (Vector2Int[])directions.Clone();
                ShuffleArray(shuffledDirections); // Shuffle directions for randomness
                directionSequence.Add(shuffledDirections); // Add to sequence list
            }
        }
    }

    void GenerateMazeLayout()
    {
        // Generate maze by starting from (0,0)
        DFSGenerateMaze(0, 0);
        
        // Create walls around the maze based on the maze structure
        PlaceWalls();
    }

    void DFSGenerateMaze(int x, int y)
    {
        maze[x, y].visited = true;

        // Retrieve the pre-generated shuffled directions for this cell
        Vector2Int[] directions = directionSequence[x * width + y];

        // Explore each direction
        foreach (var direction in directions)
        {
            int newX = x + direction.x;
            int newY = y + direction.y;

            // Check if the next cell is within bounds and unvisited
            if (IsWithinBounds(newX, newY) && !maze[newX, newY].visited)
            {
                // Remove walls between the current cell and the new cell
                RemoveWallBetweenCells(x, y, newX, newY);

                // Recursively visit the next cell
                DFSGenerateMaze(newX, newY);
            }
        }
    }

    void RemoveWallBetweenCells(int x1, int y1, int x2, int y2)
    {
        if (x1 == x2) // Moving vertically
        {
            if (y1 < y2)
            {
                maze[x1, y1].northWall = false;
                maze[x2, y2].southWall = false;
            }
            else
            {
                maze[x1, y1].southWall = false;
                maze[x2, y2].northWall = false;
            }
        }
        else if (y1 == y2) // Moving horizontally
        {
            if (x1 < x2)
            {
                maze[x1, y1].eastWall = false;
                maze[x2, y2].westWall = false;
            }
            else
            {
                maze[x1, y1].westWall = false;
                maze[x2, y2].eastWall = false;
            }
        }
    }

    void PlaceWalls()
    {
        // Place walls based on each cell's wall information
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 cellPosition = new Vector3(x, 0, y);

                if (x == width - 1 && y == height - 1)
                {
                    // Replace the north or east wall with the door
                    if (maze[x, y].northWall)
                        walls.Add(Instantiate(horizontalWallPrefab, cellPosition + new Vector3(0, 0, 0.5f), Quaternion.identity, transform));
                    if (maze[x, y].southWall)
                        walls.Add(Instantiate(horizontalWallPrefab, cellPosition + new Vector3(0, 0, -0.5f), Quaternion.identity, transform));
                    if (maze[x, y].eastWall)
                        walls.Add(Instantiate(doorPrefab, cellPosition + new Vector3(0.5f, 0, 0), Quaternion.identity, transform));
                    if (maze[x, y].westWall)
                        walls.Add(Instantiate(verticalWallPrefab, cellPosition + new Vector3(-0.5f, 0, 0), Quaternion.identity, transform));
                    continue; // Skip placing walls for this cell
                }

                if (maze[x, y].northWall)
                    walls.Add(Instantiate(horizontalWallPrefab, cellPosition + new Vector3(0, 0, 0.5f), Quaternion.identity, transform));
                if (maze[x, y].southWall)
                    walls.Add(Instantiate(horizontalWallPrefab, cellPosition + new Vector3(0, 0, -0.5f), Quaternion.identity, transform));
                if (maze[x, y].eastWall)
                    walls.Add(Instantiate(verticalWallPrefab, cellPosition + new Vector3(0.5f, 0, 0), Quaternion.identity, transform));
                if (maze[x, y].westWall)
                    walls.Add(Instantiate(verticalWallPrefab, cellPosition + new Vector3(-0.5f, 0, 0), Quaternion.identity, transform));
            }
        }
    }
    public bool ToggleWallLayer() {
        isPassable = !isPassable;
        if (isPassable) {
            ChangeWallLayer("PassThroughWall");
        } else {
            ChangeWallLayer("Default");
        }
        return isPassable;
    }

    private void ChangeWallLayer(string layerName) {
        int layer = LayerMask.NameToLayer(layerName);
        if (layer == -1)
        {
            Debug.LogError($"Layer '{layerName}' does not exist. Please create it in the Inspector.");
            return;
        }

        foreach (GameObject wall in walls)
        {
            wall.layer = layer;

            // Optionally, change the layer for all child objects
            foreach (Transform child in wall.transform)
            {
                child.gameObject.layer = layer;
            }
        }
    }
    private void SetUpPlayerEnemy() {
        Debug.Log("setting up");
        player = GameObject.Find("Player");
        exit = GameObject.Find("End");
        PositionPlayerAndExit();
    }

    void PositionPlayerAndExit()
    {
        // Position the player at the start of the maze
        player.transform.position = new Vector3(0, 0, 0);

        // Position the exit at the opposite corner of the maze
        exit.transform.position = new Vector3(width - 1, 0, height - 1);
        currentMonster = Instantiate(monsterPrefab,new Vector3(0, 0, height - 1), Quaternion.identity);
    }
    
    public void RestartGame() {
        PlayerPrefs.SetInt("Score", 0);
        int randomX = Random.Range(0, width);
        int randomZ = Random.Range(0, height);
        Vector3 respawnPosition = new Vector3(randomX, 0, randomZ);
        currentMonster.transform.position = respawnPosition;
        player.transform.position = new Vector3(0, 0, 0);
    }

    public void RespawnEnemy() {
        StartCoroutine(DelayedRespawn());
        
    }

    IEnumerator DelayedRespawn()
    {
        // Wait for 5 seconds
        yield return new WaitForSeconds(5f);

        // Execute the function after the delay
        int randomX = Random.Range(0, width);
        int randomZ = Random.Range(0, height);
        Vector3 respawnPosition = new Vector3(randomX, 0, randomZ);
        currentMonster = Instantiate(monsterPrefab, respawnPosition, Quaternion.identity);
        if (AudioController.aCtrl != null) {
            AudioController.aCtrl.PlayEnemyRespawn();
        }
    }

    bool IsWithinBounds(int x, int y)
    {
        // Check if the cell is within the maze grid
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    // Utility method to shuffle an array
    void ShuffleArray(Vector2Int[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            Vector2Int temp = array[i];
            int randomIndex = Random.Range(i, array.Length);
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
    }

    public void ResetGame() {
        Debug.Log("maze resetting");
        CharacterController playerController = player.GetComponent<CharacterController>();
        playerController.enabled = false;
        playerController.transform.position = new Vector3(0, 0, 0);
        playerController.enabled = true;


        monsterPrefab.SetActive(false);
        monsterPrefab.transform.position = new Vector3(0, 0, height - 1);
        monsterPrefab.SetActive(true);

    }
}



