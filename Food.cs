using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// food just should spawn somewhere and destroy itself onCollision with snake
public class Food : MonoBehaviour
{
    // changing color of food to make game process little bit better
    public Sprite[] FoodSprites;
    private float timeBtwChangeColor = 0;

    // it's for spawning in screen bounds(not outside of the screen)
    private Vector2 WorldPos;

    private void Start()
    {
        SetRandomPos();
    }

    private void Update()
    {
        // food must change its color every half of second
        timeBtwChangeColor += Time.deltaTime;
        if (timeBtwChangeColor >= 0.5f)
        {
            GetComponent<SpriteRenderer>().sprite = FoodSprites[Random.Range(0, FoodSprites.Length)];
            timeBtwChangeColor = 0; 
        }
    }

    private void AreaToSpawn()
    {
        WorldPos = Camera.main.WorldToScreenPoint(transform.position);

        // if object is out of screen bounds -> will set random position until it's will appear inside of screen bounds
        if(WorldPos.x >= Screen.width)
        {
            SetRandomPos();
        }
        else if (WorldPos.x < 0)
        {
            SetRandomPos();
        }

        if (WorldPos.y >= Screen.height)
        {
            SetRandomPos();
        }
        else if (WorldPos.y < 0)
        {
            SetRandomPos();
        }
    }

    private void SetRandomPos()
    {
        transform.position = new Vector2(Random.Range(-9, 9), Random.Range(-10, 10));

        if(Physics2D.OverlapCircle(transform.position, 0.1f, LayerMask.GetMask("Food")))
        {
            SetRandomPos();
        }
        AreaToSpawn();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // onCollision with snake -> destroy gameObject
        if(collision.tag == "SnakeHead")
        {
            Destroy(gameObject);
        }

        // food cant spawn inside of snake body
        if (collision.tag == "SnakePart")
        {
            SetRandomPos();
        }
    }

}
