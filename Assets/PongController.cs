using UnityStandardAssets.CrossPlatformInput;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PongController : MonoBehaviour {
    public GameObject ball;
    public GameObject player;
    public GameObject enemy;
    public Text scoreText;
    public int speed = 10;

    private Rigidbody rbEnemy;
    private Rigidbody rbPlayer;
    private Rigidbody rbBall;
    private int score;

    void Start() {
        rbBall = ball.GetComponent<Rigidbody>();
        rbPlayer = player.GetComponent<Rigidbody>();
        rbEnemy = enemy.GetComponent<Rigidbody>();
        rbBall.velocity = new Vector3(speed, speed, 0);
        score = 0;
    }

    void Update() {
        if (Mathf.Abs(ball.transform.position.x) > 10) {
            rbBall.velocity = new Vector3(speed * Mathf.Sign(ball.transform.position.x), Random.Range(-speed, speed), 0);
            score += (int) Mathf.Sign(ball.transform.position.x);
            scoreText.text = score.ToString();
            ball.transform.position = Vector3.zero;
        }
        if (Mathf.Abs(score) >= 5) {
            SceneManager.LoadScene("main");
        }
        rbPlayer.velocity = new Vector3(0, CrossPlatformInputManager.GetAxisRaw("Vertical") * speed, 0);
        rbEnemy.velocity = new Vector3(0, CrossPlatformInputManager.GetAxisRaw("Vertical") * speed, 0);
    }
}
