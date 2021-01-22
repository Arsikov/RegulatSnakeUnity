using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// this class hadles UI interaction (score, pausing etc...)
public class SnakeUI : MonoBehaviour
{

    [Header("Score")]
    [SerializeField] private TextMeshProUGUI ScoreUIText;
    public int Score;

    [Header("Pause Game")]
    [SerializeField] private GameObject PauseGameButton;
    [SerializeField] private Sprite PauseSprite;
    [SerializeField] private Sprite UnPauseSprite;
    private bool GamePaused;

    private void Start()
    {
        GameEvents._GameEvents.OnEatFood += OnEatFood;
        GameEvents._GameEvents.OnGameStart += OnGameStart;

        GamePaused = false;
    }

    private void OnEatFood()
    {
        Score++;
        ScoreUIText.text = Score.ToString();
    }

    private void OnGameStart()
    {
        Score = 0;
        ScoreUIText.text = Score.ToString();
    }

    // calls onClick the pause button
    public void PauseGame()
    {
        if (GamePaused)
        {
            Time.timeScale = 1f;
            PauseGameButton.GetComponent<Image>().sprite = PauseSprite;
            GamePaused = false;
        }
        else
        {
            Time.timeScale = 0f;
            PauseGameButton.GetComponent<Image>().sprite = UnPauseSprite;
            GamePaused = true;
        }
    }

}
