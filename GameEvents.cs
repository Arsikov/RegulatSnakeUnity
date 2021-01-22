using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

// i using singleton and observer to make connection between classes
// every class does something OnEat, OnGameOver, OnGameStart,
// OnChangeMoveDirection(i use this event just only to play some audio clip)
public class GameEvents : MonoBehaviour
{

    public event Action OnGameStart;
    public event Action OnGameOver;
    public event Action OnEatFood;
    public event Action OnSnakeChangeMoveDir;

    public static GameEvents _GameEvents;

    private void Awake()
    {
        _GameEvents = this;
    }

    public void PlayOnGameStartEvent()
    {
        OnGameStart();
    }

    public void PlayOnGameOverEvent()
    {
        OnGameOver();
    }

    public void PlayOnEatFoodEvent()
    {
        OnEatFood();
    }

    public void PlayOnSnakeChangeMoveDir()
    {
        OnSnakeChangeMoveDir();
    }

}
