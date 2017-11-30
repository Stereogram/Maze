using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    [SerializeField] private Text _scoreText;
    [SerializeField] private int score;

    void Start() {
        _scoreText = GameObject.Find("score").GetComponent<Text>();
    }

    public void SetText() {
        score += 10;
        _scoreText.text = "Score: " + score;
    }

    public int GetScore() {
        return score;
    }

    public void SetScore(int _score) {
        score = _score;
        _scoreText.text = "Score: " + score;
    }
}