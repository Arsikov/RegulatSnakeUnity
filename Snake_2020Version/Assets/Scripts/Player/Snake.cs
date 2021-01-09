using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [Header("Settings")]
    public float SnakeTimeBtwSteps;
    public Sprite FoodSprite;
    public int FoodsPerOneTime;
    public Sprite SnakeColor;

    [Header("Other")]
    [SerializeField] private Camera MainCamera;
    [SerializeField] private GameObject GameScore;
    [SerializeField] private GameObject WalkArea;
    [SerializeField] private GameObject GameOverScreen;
    [SerializeField] private GameObject StopGameButton;
    private SnakeBehaviour _SnakeBehaviour;

    private void Start()
    {
        _SnakeBehaviour = GetComponent<SnakeBehaviour>();
        _SnakeBehaviour.OnGameOver += GameOver;
        _SnakeBehaviour.OnReplayGame += PlayGame;
        StopGameButton.SetActive(true);

        PlayGame();
    }

    private void GameOver()
    {
        GameOverScreen.SetActive(true);
        GameScore.SetActive(false);
        Time.timeScale = 0f;
        StopGameButton.SetActive(false);
    }

    private void PlayGame()
    {
        GameScore.SetActive(true);
        GameOverScreen.SetActive(false);
        Time.timeScale = 1f;
        StopGameButton.SetActive(true);
    }

    public void ReplayGameButton()
    {
        _SnakeBehaviour.RunReplayEvent();
    }

    public void StopGame()
    {
        GameScore.SetActive(false);
        _SnakeBehaviour.RunGameOverEvent();
        StopGameButton.SetActive(false);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SetSmallAreaSize()
    {
        WalkArea.transform.localScale = new Vector2(14f,26.4f);
        _SnakeBehaviour.XAreaSize = 4;
        _SnakeBehaviour.YAreaSize = 8;
        MainCamera.orthographicSize = 5.4f;
    }
    public void SetMediumAreaSize()
    {
        WalkArea.transform.localScale = new Vector2(19.8f, 38.8f);
        _SnakeBehaviour.XAreaSize = 6;
        _SnakeBehaviour.YAreaSize = 12;
        MainCamera.orthographicSize = 7.5f;
    }
    public void SetLargeAreaSize()
    {
        WalkArea.transform.localScale = new Vector2(26, 51);
        _SnakeBehaviour.XAreaSize = 8;
        _SnakeBehaviour.YAreaSize = 16;
        MainCamera.orthographicSize = 10;
    }



    /*public void SetAreaSize(float cameraSize, int XSize, int YSize, Vector2 AreaSize)
    {
        WalkArea.transform.localScale = AreaSize;
        _SnakeBehaviour.XAreaSize = XSize;
        _SnakeBehaviour.YAreaSize = YSize;
        MainCamera.orthographicSize = cameraSize;
    }*/

}
