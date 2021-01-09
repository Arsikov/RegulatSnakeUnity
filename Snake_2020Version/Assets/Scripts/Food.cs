using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour
{

    SnakeBehaviour _SnakeBehaviour;
    private bool CollidesWithFoodOnStart;

    private void Start()
    {
        _SnakeBehaviour = GameObject.FindObjectOfType<SnakeBehaviour>();
        SetRandomPos();
        transform.GetChild(0).GetComponent<SpriteRenderer>().sprite = GameObject.FindObjectOfType<Snake>().FoodSprite;
    }

    private void SetRandomPos()
    {
        int XSize = _SnakeBehaviour.XAreaSize / 2;
        int YSize = _SnakeBehaviour.YAreaSize / 2;
        Vector2Int newPos = new Vector2Int(Mathf.RoundToInt(Random.Range(XSize, -XSize)), Mathf.RoundToInt(Random.Range(YSize, -YSize)));
        transform.position = (Vector2)newPos;
        if (Physics2D.OverlapCircle(transform.position, 01f, _SnakeBehaviour.FoodLayer))
        {
            SetRandomPos();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "SnakeHead") Destroy(gameObject);
        else if (collision.tag == "SnakePart") SetRandomPos();
    }
}
