using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class GameController : MonoBehaviour {
    public GameObject mazeContainer;
    public GameObject player;
    public GameObject enemy;
    public GameObject flashlight;
    public Score score;
    [SerializeField] private int mazeScale = 5;
    [SerializeField] private int mazeSize = 10;
    [SerializeField] private Color dayColor = new Color32(200, 180, 160, 255);
    [SerializeField] private Color nightColor = new Color32(32, 32, 64, 255);
    [SerializeField] private AudioClip daySound;
    [SerializeField] private AudioClip nightSound;
    bool[,] mazeData;
    MazeGenerator mazeGenerator;
    Object cellPrefab;
    private AudioSource _source;

    void Start() {
        cellPrefab = Resources.Load("Cell");
        mazeGenerator = new MazeGenerator();
        mazeData = mazeGenerator.GenerateMaze(mazeSize);
        BuildMaze(mazeData);
        flashlight.SetActive(false);
        _source = GameObject.Find("unitychan").GetComponent<AudioSource>();
    }

    void BuildMaze(bool[,] _maze) {
        for (int x = 0; x < _maze.GetLength(0); x++) {
            for (int y = 0; y < _maze.GetLength(1); y++) {
                if (_maze[x, y]) {
                    GameObject temp = (GameObject)Instantiate(cellPrefab);
                    if (x + 1 < _maze.GetLength(0) && _maze[x + 1, y]) {
                        Destroy(temp.transform.Find("East").gameObject);
                    }
                    if (x - 1 >= 0 && _maze[x - 1, y]) {
                        Destroy(temp.transform.Find("West").gameObject);
                    }
                    if (y + 1 < _maze.GetLength(1) && _maze[x, y + 1]) {
                        Destroy(temp.transform.Find("North").gameObject);
                    }
                    if (y - 1 >= 0 && _maze[x, y - 1]) {
                        Destroy(temp.transform.Find("South").gameObject);
                    }
                    temp.transform.localScale *= mazeScale;
                    temp.transform.position = new Vector3(x * mazeScale, 0, y * mazeScale);
                    temp.transform.SetParent(mazeContainer.transform);
                }
            }
        }
    }

    void Update() {
        // Returns player to the start of the maze.
        if (CrossPlatformInputManager.GetButtonDown("Reset_Position")) {
            player.GetComponent<FirstPersonController>().Reset();
        }
        // Reset player position
        if (CrossPlatformInputManager.GetButtonDown("Toggle_Fog")) {
            if (RenderSettings.fog) {
                RenderSettings.fog = false;
                _source.volume = 1.0f;
            } else {
                RenderSettings.fog = true;
                _source.volume = 0.5f;
            }
        }
        // Toggles player layer from '0' to '9'. When player's layer is '9' they will not collide with walls.
        if (CrossPlatformInputManager.GetButtonDown("Toggle_Collisions")) {
            player.layer = (player.layer == 0) ? 9 : 0;
        }
        // Togges ambient lighting between day and night + changes fog color to match
        if (CrossPlatformInputManager.GetButtonDown("Toggle_TOD")) {
            if (RenderSettings.ambientLight == dayColor) {
                RenderSettings.ambientLight = nightColor;
                RenderSettings.fogColor = nightColor;
                _source.clip = nightSound;
            } else {
                RenderSettings.ambientLight = dayColor;
                RenderSettings.fogColor = dayColor;
                _source.clip = daySound;
            }
            if (!_source.isPlaying) {
                _source.Play();
            }
        }
        // Toggles player flashlight
        if (CrossPlatformInputManager.GetButtonDown("Toggle_Flashlight")) {
            flashlight.SetActive(!flashlight.activeSelf);
        }
        // Toggle background music
        if (CrossPlatformInputManager.GetButtonDown("Toggle_Music")) {
            if (_source.isPlaying) {
                _source.Stop();
            } else {
                _source.Play();
            }
        }
        // Saves gamestate
        if (CrossPlatformInputManager.GetButtonDown("Save")) {
            SaveGame();
        }
        // Loads gamestate
        if (CrossPlatformInputManager.GetButtonDown("Load")) {
            LoadGame();
        }
    }

    public void SaveGame() {
        // save maze
        PlayerPrefsSerializer.Save("MazeData", mazeData);
        // save player position
        PlayerPrefs.SetFloat("PlayerX", player.transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", player.transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", player.transform.position.z);
        // save enemy position
        PlayerPrefs.SetFloat("EnemyX", enemy.transform.position.x);
        PlayerPrefs.SetFloat("EnemyY", enemy.transform.position.y);
        PlayerPrefs.SetFloat("EnemyZ", enemy.transform.position.z);
        // save score
        PlayerPrefs.SetInt("Score", score.GetScore());
    }

    public void LoadGame() {
        if (PlayerPrefs.HasKey("MazeData") && PlayerPrefs.HasKey("PlayerX") && PlayerPrefs.HasKey("PlayerY") && PlayerPrefs.HasKey("PlayerZ") && PlayerPrefs.HasKey("EnemyX") && PlayerPrefs.HasKey("EnemyY") && PlayerPrefs.HasKey("EnemyZ") && PlayerPrefs.HasKey("Score")) {
            // rebuild maze
            foreach (Transform child in mazeContainer.transform) {
                Destroy(child.gameObject);
            }
            mazeData = (bool[,])PlayerPrefsSerializer.Load("MazeData");
            BuildMaze(mazeData);
            // load player position
            player.transform.position = new Vector3(PlayerPrefs.GetFloat("PlayerX"), PlayerPrefs.GetFloat("PlayerY"), PlayerPrefs.GetFloat("PlayerZ"));
            // load enemy position
            enemy.transform.position = new Vector3(PlayerPrefs.GetFloat("EnemyX"), PlayerPrefs.GetFloat("EnemyY"), PlayerPrefs.GetFloat("EnemyZ"));
            // load score
            score.SetScore(PlayerPrefs.GetInt("Score"));
        }
    }
}
