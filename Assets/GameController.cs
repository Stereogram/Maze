using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;
using UnityStandardAssets.CrossPlatformInput;

public class GameController : MonoBehaviour {
    [SerializeField] private int mazeScale = 5;
    [SerializeField] private int mazeSize = 10;
    [SerializeField] private Color dayColor = new Color32(200, 180, 160, 255);
    [SerializeField] private Color nightColor = new Color32(32, 32, 64, 255);
    [SerializeField] private AudioClip daySound;
    [SerializeField] private AudioClip nightSound;
    MazeGenerator mazeGenerator;
    Object cellPrefab;
    GameObject player;

    private AudioSource _source;

    void Start() {
        cellPrefab = Resources.Load("Cell");
        mazeGenerator = new MazeGenerator();
        BuildMaze(mazeGenerator.GenerateMaze(mazeSize));
        player = GameObject.Find("FPSController");
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
                }
            }
        }
    }

    void Update() {
        // Returns player to the start of the maze.
		if (CrossPlatformInputManager.GetButtonDown("Reset_Position")) {
            player.GetComponent<FirstPersonController>().Reset();
        }
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
                _source.clip = nightSound;
                if(!_source.isPlaying) _source.Play();
            } else {
                RenderSettings.ambientLight = dayColor;
                _source.clip = daySound;
                if (!_source.isPlaying) _source.Play();
            }
			RenderSettings.fogColor = RenderSettings.ambientLight;
        }

        if(CrossPlatformInputManager.GetButtonDown("Toggle_Music")) {
            if (_source.isPlaying) {
                _source.Stop();
            }
            else {
                _source.Play();
            }
        }
    }
}
