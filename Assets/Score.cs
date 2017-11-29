using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    [SerializeField] private Text _scoreText;
    [SerializeField] private int score;

	// Use this for initialization
	void Start () {
        _scoreText = GameObject.Find("score").GetComponent<Text>();
        _scoreText.text = "Score: 0";
	}
	
    public void SetText()
    {
        score += 10;
        _scoreText.text = "Score: " + score;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
