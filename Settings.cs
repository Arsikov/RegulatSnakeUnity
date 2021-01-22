using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    public static Settings current;
    
    // food amount
    public int AmountOfFood = 1;

    // snake speed
    public float TimeBtwMovesteps = 0.1f;

    // area size (camera orho size)
    public float CameraOrthoSize;
    [SerializeField] private Camera MainCamera;

    [Header("Mobile Control")]
    public bool SwipeControl; // <- just a classic swipes
    public bool TouchControl; // <- i have 4 transparent buttons in bottom of display(up, down, left, right)

    [SerializeField] private GameObject TouchControlPanel; // <- there is gameObject contains that 4 buttons
    [SerializeField] private GameObject ControlSwitch; // <- this is just a button to change control(has 2 states: swipe, touch)

    /// <summary>
    ///  i can set some setting in GameOverScreen(speed, amountOfFood, areaSize)
    /// </summary>

    // setting snake speed
    public void SetNewSpeedUI(float newSpeed)
    {
        TimeBtwMovesteps = newSpeed;
    }

    // setting amountOfFood
    public void SetNewAmountOfFood(int newAmountOfFood)
    {
        AmountOfFood = newAmountOfFood;
    }

    // setting AreaSize
    public void SetNewAreaSize(float newCameraOrthoSize)
    {
        CameraOrthoSize = newCameraOrthoSize;
        MainCamera.orthographicSize = CameraOrthoSize;
    }

    // setting mobile control(touch or swipe)
    public void SetNewControl()
    {
        if (SwipeControl)
        {
            TouchControl = true;

            SwipeControl = false;
            ControlSwitch.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Touch";
        }
        else if (TouchControl)
        {
            SwipeControl = true;

            TouchControl = false;
            ControlSwitch.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Swipe";
        }
    }

    private void Start()
    {
        GameEvents._GameEvents.OnGameStart += OnGameStart;
        GameEvents._GameEvents.OnGameOver += OnGameOver;

        current = this; // in some classes i need to change some elems of them when i change settings(speed, amountOfFood, areaSize)
        // it is why this variable static 

        SwipeControl = true; // <- by default player uses swipes to control snake
    }

    private void OnGameStart()
    {
        if(TouchControl) TouchControlPanel.SetActive(true);
    }
    private void OnGameOver()
    {
        TouchControlPanel.SetActive(false);
    }

}
