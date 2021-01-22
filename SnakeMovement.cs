using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class handle movement
public class SnakeMovement : MonoBehaviour
{
    public bool GameStopped = true;

    public bool SwipeControl = true; // <- i can change mobile control

    public GameObject SnakeHead;
    public GameObject SnakePartPrefab;

    // lenght to move per one time
    public float StepLenght = 0.5f;

    public float StartTimeBtwMoveSteps; // <- kind of snake speed
    private float TimeBtwMoveSteps = 0;
    private float TimeBtwChangeMoveDir = 0;

    private Vector2 Pos; // <- position
    private Vector2 MoveDir; // <- dirextion to move

    // move directions
    private Vector2 LeftDir;
    private Vector2 RightDir;
    private Vector2 UpDir;
    private Vector2 DownDir;

    // i need these lists to move snake parts
    private List<GameObject> Parts = new List<GameObject>();
    private List<Vector2> PartsPos = new List<Vector2>();

    // swipe controll
    private Vector2 startSwipePos;
    private Vector2 endSwipePos;

    private void Start()
    {
        GameEvents._GameEvents.OnEatFood += AddNewPart;
        GameEvents._GameEvents.OnGameStart += OnGameStart;
        GameEvents._GameEvents.OnGameOver += OnGameOver;

        LeftDir = new Vector2(-StepLenght, 0);
        RightDir = new Vector2(StepLenght, 0);
        UpDir = new Vector2(0, StepLenght);
        DownDir = new Vector2(0, -StepLenght);
    }

    private void Update()
    {
        if (!GameStopped)
        {
            HandleInput();

            TimeBtwMoveSteps += Time.deltaTime;
            TimeBtwChangeMoveDir += Time.deltaTime;

            if (TimeBtwMoveSteps >= StartTimeBtwMoveSteps)
            {
                Pos += MoveDir;
                transform.position = Pos;

                MoveParts();
                CheckCollisionWithSnakeParts();
                WrapByArea();

                TimeBtwMoveSteps = 0;
            }
        }
    }

    private void AddNewPart()
    {
        GameObject NewSnakePart = Instantiate(SnakePartPrefab, new Vector2(-1000, -1000), Quaternion.identity);
        NewSnakePart.transform.parent = transform;
        NewSnakePart.transform.localScale = Vector2.one;

        Parts.Add(NewSnakePart);
        PartsPos.Add(NewSnakePart.transform.position);
    }

    private void MoveParts()
    {
        PartsPos.Insert(0, SnakeHead.transform.position);
        PartsPos.RemoveAt(PartsPos.Count - 1);

        for(int i = 0; i < PartsPos.Count; i++)
        {
            Parts[i].transform.position = PartsPos[i];
        }
    }

    private void HandleInput()
    {
        // i need timeBtwChangeMoveSteps because i can rapidly click different keys 
        // and as a result go backwards, 
        // and then calls gameOver event
        if (TimeBtwChangeMoveDir >= 0.08f)
        {
            // up
            if (Input.GetKeyDown(KeyCode.W) && MoveDir != DownDir && MoveDir != UpDir)
            {
                MoveDir = UpDir;
                TimeBtwChangeMoveDir = 0;
                GameEvents._GameEvents.PlayOnSnakeChangeMoveDir(); // i am playing clips when changing move direction
            }
            // left
            if (Input.GetKeyDown(KeyCode.A) && MoveDir != RightDir && MoveDir != LeftDir)
            {
                MoveDir = LeftDir;
                TimeBtwChangeMoveDir = 0;
                GameEvents._GameEvents.PlayOnSnakeChangeMoveDir(); // i am playing clips when changing move direction
            }
            // down
            if (Input.GetKeyDown(KeyCode.S) && MoveDir != UpDir && MoveDir != DownDir)
            {
                MoveDir = DownDir;
                TimeBtwChangeMoveDir = 0;
                GameEvents._GameEvents.PlayOnSnakeChangeMoveDir(); // i am playing clips when changing move direction
            }
            // right
            if (Input.GetKeyDown(KeyCode.D) && MoveDir != RightDir && MoveDir != LeftDir)
            {
                MoveDir = RightDir;
                TimeBtwChangeMoveDir = 0;
                GameEvents._GameEvents.PlayOnSnakeChangeMoveDir(); // i am playing clips when changing move direction
            }
        }

        if (SwipeControl && Input.touchCount > 0)
        {
            if(Input.GetTouch(0).phase == TouchPhase.Began)
            {
                startSwipePos = Input.GetTouch(0).position;
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                endSwipePos = Input.GetTouch(0).position;
                SwipeHandle();
            }
        }
    }

    private void WrapByArea()
    {
        // if snake goes out of screen edges calls OnGameOver event
        Vector2 snakePosInPixels = Camera.main.WorldToScreenPoint(transform.position);

        if (snakePosInPixels.x > Screen.width || snakePosInPixels.x < 0)
        {
            GameEvents._GameEvents.PlayOnGameOverEvent();
        }
        if (snakePosInPixels.y > Screen.height || snakePosInPixels.y < 0)
        {
            GameEvents._GameEvents.PlayOnGameOverEvent();
        }
    }

    private void SwipeHandle()
    {
        Vector2 swipeDir = endSwipePos - startSwipePos;

        float xDir = Mathf.Abs(swipeDir.x);
        float yDir = Mathf.Abs(swipeDir.y);

        // horizontal
        if (xDir > yDir)
        {
            // right
            if(swipeDir.x > 0)
            {
                if (MoveDir != LeftDir && MoveDir != RightDir)
                { 
                    MoveDir = RightDir;
                    GameEvents._GameEvents.PlayOnSnakeChangeMoveDir();
                }
            }
            // left
            if (swipeDir.x < 0)
            {
                if (MoveDir != LeftDir && MoveDir != RightDir)
                {
                    MoveDir = LeftDir;
                    GameEvents._GameEvents.PlayOnSnakeChangeMoveDir();
                }
            }
        }
        // vertical
        if (xDir < yDir)
        {
            // up
            if (swipeDir.y > 0)
            {
                if (MoveDir != DownDir && MoveDir != UpDir)
                {
                    MoveDir = UpDir;
                    GameEvents._GameEvents.PlayOnSnakeChangeMoveDir();
                }
            }
            // down
            if (swipeDir.y < 0)
            {
                if (MoveDir != DownDir && MoveDir != UpDir)
                {
                    MoveDir = DownDir;
                    GameEvents._GameEvents.PlayOnSnakeChangeMoveDir();
                }
            }
        }
    }
    private void CheckCollisionWithSnakeParts()
    {
        if (Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("SnakePart")) && MoveDir != Vector2.zero)
        {
            GameEvents._GameEvents.PlayOnGameOverEvent();
        }
    }

    private void OnGameStart()
    {
        GameStopped = false;

        // destroying current parts 
        GameObject[] SnakeParts = GameObject.FindGameObjectsWithTag("SnakePart");
        foreach(GameObject part in SnakeParts)
        {
            Destroy(part);
        }

        // clearing lists 
        Parts.Clear();
        PartsPos.Clear();

        // snake head must be always the first elem of these lists
        Parts.Add(SnakeHead);
        PartsPos.Add(SnakeHead.transform.position);

        // making new snake body
        AddNewPart();
        AddNewPart();
        AddNewPart();

        // default transform settings
        Pos = new Vector2(-1, 0);
        transform.position = Pos;
        MoveDir = Vector2.zero;

        // setting new speed
        StartTimeBtwMoveSteps = Settings.current.TimeBtwMovesteps;

        // sets mobile control
        SwipeControl = Settings.current.SwipeControl;
    }

    private void OnGameOver()
    {
        GameStopped = true;
    }



    // these methods calls only when player choose touch control in gameOverScreen

    public void TurnLeft()
    {
        if(MoveDir != RightDir && MoveDir != LeftDir)
        {
            MoveDir = LeftDir;
            TimeBtwChangeMoveDir = 0;
            GameEvents._GameEvents.PlayOnSnakeChangeMoveDir();
        }
    }
    public void TurnRight()
    {
        if (MoveDir != LeftDir && MoveDir != RightDir)
        {
            MoveDir = RightDir;
            TimeBtwChangeMoveDir = 0;
            GameEvents._GameEvents.PlayOnSnakeChangeMoveDir();
        }
    }
    public void TurnUp()
    {
        if (MoveDir != DownDir && MoveDir != UpDir)
        {
            MoveDir = UpDir;
            TimeBtwChangeMoveDir = 0;
            GameEvents._GameEvents.PlayOnSnakeChangeMoveDir();
        }
    }
    public void TurnDown()
    {
        if (MoveDir != UpDir && MoveDir != DownDir)
        {
            MoveDir = DownDir;
            TimeBtwChangeMoveDir = 0;
            GameEvents._GameEvents.PlayOnSnakeChangeMoveDir();
        }
    }

}
