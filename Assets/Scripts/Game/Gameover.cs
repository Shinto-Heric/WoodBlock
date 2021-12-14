using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    public Text scoreText;
    public Text bestScore;
    private void Awake()
    {
        gameObject.SetActive(false);
    }
    private void OnEnable()
    {
        GameEvents.GameOver += GameOver;
    }
    private void OnDisable()
    {
        GameEvents.GameOver -= GameOver;

    }
    void Start()
    {
        
    }

    public void GameOver(int score)
    {
        scoreText.text = "Score : " + score.ToString();
        bestScore.text = "Best : " + score.ToString();
        gameObject.SetActive(true);
    }

    public void OnPlayAgainButtonClick()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}
