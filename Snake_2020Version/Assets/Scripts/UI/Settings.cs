using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] private Image[] FoodImagesOnStart;
    [SerializeField] private Image FoodImage;
    private Snake _Snake;

    private void Start()
    {
        _Snake = GameObject.FindObjectOfType<Snake>();
    }

    public void SetTimeBtwsteps(float newTimeBtwSteps)
    {
        _Snake.SnakeTimeBtwSteps = newTimeBtwSteps;
    }

    public void SetFoodSprite(Sprite newFoodSprite)
    {
        _Snake.FoodSprite = newFoodSprite;
        FoodImage.sprite = newFoodSprite;
        foreach(Image foodImage in FoodImagesOnStart)
        {
            foodImage.sprite = newFoodSprite;
        }
    }

    public void SetFoodPrefabsPerTime(int newFoodPrefabs)
    {
        _Snake.FoodsPerOneTime = newFoodPrefabs;
    }

    public void SetSnakeColor(Sprite newSnakeColor)
    {
        _Snake.SnakeColor = newSnakeColor;
    }

}
