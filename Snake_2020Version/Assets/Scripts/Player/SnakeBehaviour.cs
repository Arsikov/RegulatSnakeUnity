using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SnakeBehaviour : MonoBehaviour
{
    private Snake _Snake;
    private AudioManager _SnakeAudioManager;

    public event Action OnGameOver;
    public event Action OnReplayGame;

    public int XAreaSize;
    public int YAreaSize;

    public LayerMask[] SnakeMasks;

    [Header("UI")]
    [SerializeField] private TextMeshProUGUI ScoreUI;
    private int Score = 0;

    [Header("Moving")]
    [SerializeField] private float PartDiameter;
    [SerializeField] private float StepLenght;
    [SerializeField] private float StartTimeBtwSteps;
    private float TimeBtwSteps;

    private Vector2 GridPos;
    public Vector2 MoveDir;

    [Header("Food")]
    [SerializeField] private GameObject SnakePartPrefab;
    public int FoodPrefabsPerTime;
    public LayerMask FoodLayer;

    [Header("Objects")]
    [SerializeField] private Sprite DefaultSnakeSprite;
    [SerializeField] private GameObject SnakeHead;
    [SerializeField] private GameObject FoodPrefab;

    [Header("Swipe")]
    private Vector2 StartSwipePos;
    private Vector2 EndSwipePos;



    private List<Vector2> SnakePartsPos = new List<Vector2>();
    private List<GameObject> SnakeParts = new List<GameObject>();

    private void Start()
    {
        _Snake = GetComponent<Snake>();
        _SnakeAudioManager = GameObject.FindObjectOfType<AudioManager>();

        OnGameOver += GameOver;
        OnReplayGame += Play;

        GridPos = new Vector2(-2.5f, 0);
        MoveDir = new Vector2(StepLenght, 0);
        XAreaSize = 6;
        YAreaSize = 12;

        Play();
    }

    private void Update()
    {
        TimeBtwSteps += Time.deltaTime;
        HandleInput();

        if (TimeBtwSteps >= StartTimeBtwSteps)
        {
            transform.eulerAngles = new Vector3(0, 0, RotateSnakeHead(MoveDir));
            GridPos += MoveDir;
            transform.position = GridPos;
            TimeBtwSteps = 0;
            MoveSnakeParts();
            WrapByArea();
        }
    }

    private void HandleInput()
    {
        // key input
        if(Input.GetKeyDown(KeyCode.W) && MoveDir != new Vector2(0, -StepLenght))
        {
            MoveDir = new Vector2(0, StepLenght);
        }
        if (Input.GetKeyDown(KeyCode.S) && MoveDir != new Vector2(0, StepLenght))
        {
            MoveDir = new Vector2(0, -StepLenght);
        }
        if (Input.GetKeyDown(KeyCode.A) && MoveDir != new Vector2(StepLenght, 0))
        {
            MoveDir = new Vector2(-StepLenght, 0);
        }
        if (Input.GetKeyDown(KeyCode.D) && MoveDir != new Vector2(-StepLenght, 0))
        {
            MoveDir = new Vector2(StepLenght, 0);
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                StartSwipePos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                EndSwipePos = touch.position;
                OnSwipe();
            }
        }

    }

    private void OnSwipe()
    {
        Vector2 SwipeDir = EndSwipePos - StartSwipePos;
        float xDist = Mathf.Abs(SwipeDir.x);
        float yDist = Mathf.Abs(SwipeDir.y);
        // vertical swipe
        if (xDist > yDist)
        {
            // right
            if (SwipeDir.x > 0 && MoveDir != new Vector2(-StepLenght, 0))
                MoveDir = new Vector2(StepLenght, 0);
            // left
            if(SwipeDir.x < 0 && MoveDir != new Vector2(StepLenght, 0))
                MoveDir = new Vector2(-StepLenght, 0);
        }
        // horizontal swipe
        if(yDist > xDist)
        {
            // up
            if(SwipeDir.y > 0 && MoveDir != new Vector2(0, -StepLenght))
                MoveDir = new Vector2(0, StepLenght);
            // down
            if(SwipeDir.y < 0 && MoveDir != new Vector2(0, StepLenght))
                MoveDir = new Vector2(0, -StepLenght);
        }
    }

    private void MoveSnakeParts()
    {
        SnakePartsPos.Insert(0, SnakeHead.transform.position);
        SnakePartsPos.RemoveAt(SnakePartsPos.Count - 1);
        
        for(int i = 0; i < SnakePartsPos.Count; i++)
        {
            SnakeParts[i].transform.position = SnakePartsPos[i];
        }
    }

    private void AddPart()
    {
        GameObject newPart = Instantiate(SnakePartPrefab, new Vector2(1000, 1000), Quaternion.identity);
        newPart.transform.parent = transform;
        newPart.transform.localScale = Vector2.one;
        newPart.GetComponent<SpriteRenderer>().sprite = _Snake.SnakeColor;
        if (newPart.GetComponent<SpriteRenderer>().sprite == null)
        {
            newPart.GetComponent<SpriteRenderer>().sprite = DefaultSnakeSprite;
        }

        SnakeParts.Add(newPart);
        SnakePartsPos.Add(newPart.transform.position);
    }

    private float RotateSnakeHead(Vector2 direction)
    {
        float n = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SnakePart")
        {
            OnGameOver();
        }

        if (collision.tag == "Food")
        {
            EatFood();
        }
    }

    private void WrapByArea()
    {
        if(GridPos.x > XAreaSize / 2 || GridPos.x < -(XAreaSize / 2) || GridPos.y > (YAreaSize / 2) || GridPos.y < -(YAreaSize / 2))
            OnGameOver();
    }

    private void EatFood()
    {
        Score++;
        ScoreUI.text = Score.ToString();
        AddPart();
        Instantiate(FoodPrefab);
        _SnakeAudioManager.PlayEatFoodSound();

        if(Score % 10 == 0)
        {
            _SnakeAudioManager.PlayerGetsMoreTenScore();
        }
    }

    private void GameOver()
    {
        SnakeParts.Clear();
        SnakePartsPos.Clear();

        GameObject[] Parts = GameObject.FindGameObjectsWithTag("SnakePart");
        foreach (GameObject part in Parts)
        {
            Destroy(part);
        }

        GameObject[] Foods = GameObject.FindGameObjectsWithTag("Food");
        for(int i = 0; i < Foods.Length; i++)
        {
            Destroy(Foods[i]);
        }
        
        GridPos = new Vector2(-2.5f, 0);
        transform.eulerAngles = new Vector3(0, 0, RotateSnakeHead(MoveDir));

        _SnakeAudioManager.PlayerDead();

        Score = 0;
        ScoreUI.text = Score.ToString();

        Time.timeScale = 0f;
    }

    private void Play()
    {
        SnakePartsPos.Add(SnakeHead.transform.position);
        SnakeParts.Add(SnakeHead);
        MoveDir = new Vector2(StepLenght, 0);

        AddPart();
        AddPart();

        // Down there set different settings in game over screen

        // time between steps
        StartTimeBtwSteps = _Snake.SnakeTimeBtwSteps;

        // amount of food on start 
        FoodPrefabsPerTime = _Snake.FoodsPerOneTime;
        for(int i = 0; i < FoodPrefabsPerTime; i++) 
        {
            Instantiate(FoodPrefab);
        }

        // snake color
        GetComponent<SpriteRenderer>().sprite = _Snake.SnakeColor;

        // area size
        //WalkArea.transform.localScale = _Snake.AreaSize;

        if (GetComponent<SpriteRenderer>().sprite == null)
        {
            GetComponent<SpriteRenderer>().sprite = DefaultSnakeSprite;
        }
    }

    public void RunReplayEvent()
    {
        OnReplayGame();
    }

    public void RunGameOverEvent()
    {
        OnGameOver();
    }

}
