using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoreInfo : MonoBehaviour
{
    private SnakeBehaviour _SnakeBehaviour;
    [SerializeField] private GameObject MoreInfoPanel;

    private void Start()
    {
        _SnakeBehaviour = GameObject.FindObjectOfType<SnakeBehaviour>();
        MoreInfoPanel.SetActive(false);
    }

    public void MoreInfoOnClick()
    {
        MoreInfoPanel.SetActive(true);
    }

    public void Back()
    {
        MoreInfoPanel.SetActive(false);
    }
}
