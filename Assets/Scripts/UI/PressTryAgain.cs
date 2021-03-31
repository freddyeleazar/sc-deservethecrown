using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PressTryAgain : MonoBehaviour, IPointerClickHandler
{
    public GameController gameController;

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameController.currentStage == 2)
        {
            gameController.LoadStage2();
        }
        else
        {
            gameController.LoadStage1();
        }
    }
}
