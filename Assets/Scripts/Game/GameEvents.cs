using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static Action<int> GameOver;
    public static Action<int> AddScores;
    public static Action CheckIfBlockCanBePlaced;
    public static Action MoveShapeToStartPosition;
    public static Action RequestNewBlock;
    public static Action SetShapeInactive;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
