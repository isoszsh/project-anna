using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    public GameObject popupPanel;

    void Start()
    {
        // Sahne başladığında paneli gizle
        popupPanel.SetActive(false);
    }

    public void ShowPopup()
    {
        popupPanel.SetActive(true);
    }

    public void HidePopup()
    {
        popupPanel.SetActive(false);
    }
}
