using UnityStandardAssets.CrossPlatformInput;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PongController : MonoBehaviour {
    public int speed = 10;
    public GameObject ball;
    public GameObject player;
    public GameObject enemy;
    private Rigidbody rbEnemy;
    private Rigidbody rbPlayer;
    private Rigidbody rbBall;
    private int score;

    // Use this for initialization
    void Start() {
        rbBall = ball.GetComponent<Rigidbody>();
        rbBall.velocity = new Vector3(speed, speed, 0);
        rbPlayer = player.GetComponent<Rigidbody>();
        rbEnemy = enemy.GetComponent<Rigidbody>();
        score = 0;
    }

    // Update is called once per frame
    void Update() {
        if (Mathf.Abs(ball.transform.position.x) > 10) {
            rbBall.velocity = new Vector3(speed * Mathf.Sign(ball.transform.position.x), speed, 0);
            score += (int) Mathf.Sign(ball.transform.position.x);
            ball.transform.position = Vector3.zero;
        }
        rbPlayer.velocity = new Vector3(0, CrossPlatformInputManager.GetAxisRaw("Vertical") * speed, 0);
        rbEnemy.velocity = new Vector3(0, CrossPlatformInputManager.GetAxisRaw("Vertical") * speed, 0);
        if (Mathf.Abs(score) >= 5) {
            Application.LoadLevel("main");
        }
    }
}
