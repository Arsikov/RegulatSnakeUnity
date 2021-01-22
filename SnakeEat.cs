using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// just handles eating
public class SnakeEat : MonoBehaviour
{
    [SerializeField] private GameObject FoodPrefab;

    private void Start()
    {
        GameEvents._GameEvents.OnGameStart += OnGameStart;
        GameEvents._GameEvents.OnGameOver += OnGameOver;
        GameEvents._GameEvents.OnEatFood += OnEatFood;
    }

    private void OnEatFood()
    {
        Instantiate(FoodPrefab);
    }

    private void OnGameStart()
    {
        // i just can set amount of food
        for(int i = 0; i < Settings.current.AmountOfFood; i++)
        {
            Instantiate(FoodPrefab);
        }
    }
    private void OnGameOver()
    {
        // on gameOver all food must be destroyed
        GameObject[] FoodPrefabs = GameObject.FindGameObjectsWithTag("Food");
        foreach(GameObject food in FoodPrefabs)
        {
            Destroy(food);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Food>() != null)
        {
            GameEvents._GameEvents.PlayOnEatFoodEvent();
        }
    }
}
