using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    private bool GameIsOver = true;


    // shows score in the game over screen
    [SerializeField] private TextMeshProUGUI GameScoreUI;

    // using to play game over/start animations
    private Animator animator;

    // these are all for changing food color in game over screen
    [Header("Food Color")]
    [SerializeField] private Image[] FoodImagesUI;
    [SerializeField] private Sprite[] FoodSprites;
    private float timeBtwChangeFoodColor = 0;

    private void Start()
    {
        animator = GetComponent<Animator>();

        GameEvents._GameEvents.OnGameStart += OnGameStart;
        GameEvents._GameEvents.OnGameOver += OnGameOver;
    }

    private void Update()
    {
        if (GameIsOver)
        {
            // i am just changing food color in game over screen(i think with this small feature game becomes little bit better)
            timeBtwChangeFoodColor += Time.deltaTime;
            if (timeBtwChangeFoodColor >= 0.7f)
            {
                foreach (Image foodImage in FoodImagesUI)
                {
                    foodImage.sprite = FoodSprites[Random.Range(0, FoodSprites.Length)];
                }
                timeBtwChangeFoodColor = 0;
            }
        }
    }

    private void OnGameStart()
    {
        GameIsOver = false;

        // playing animation
        animator.Play("OnGameStartAnim");

        // on GameOver i disable chidren(buttons, setting menu etc...)
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
        }
    }
    private void OnGameOver()
    {
        GameIsOver = true;
        // getting score to show it on gameOverScreen
        GameScoreUI.text = GameObject.FindGameObjectWithTag("SnakeHead").GetComponent<SnakeUI>().Score.ToString();

        // playing animation
        animator.Play("OnGameOverAnim");

        // on GameOver i enable chidren(buttons, setting menu etc...)
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }
    }
}
