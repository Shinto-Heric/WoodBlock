using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Scores : MonoBehaviour
{
    public Text score;

    private int currentScore;

     void Start()
    {
        currentScore = 0;
    }
    private void OnEnable()
    {
        GameEvents.AddScores += AddScores;
    }
    private void OnDisable()
    {
        GameEvents.AddScores -= AddScores;
    }

    private void AddScores(int score)
    {
        currentScore += score;
        UpdateScoreText(); 
    }

    private void UpdateScoreText()
    {
        score.text = currentScore.ToString();
    }
}
