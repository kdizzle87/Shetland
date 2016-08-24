﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Highscores : MonoBehaviour {

	public int _score;
	public Text _scoreText;

	void Awake(){
		_score = NewGame._score;
		_scoreText = GameObject.Find("ScoreText").GetComponent<Text>();
		_scoreText.text = "Score: " + _score;
	}

	public void MainMenu(){
		SceneManager.LoadScene(0);
	}
}