using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using System.Threading.Tasks;
// detecting mouse clicks: https://learn.unity.com/tutorial/onmousedown#

public class UIManager : MonoBehaviour {
    public GameObject startButton;
    TextMeshProUGUI buttonText;
    // public TMP_Dropdown dropdown;
    public GameController gameController;

    // Use this for initialization
    void Start() {
        buttonText = startButton.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update() {

    }

    public void StartButtonClicked() {
        if (gameController) {
            gameController.SetPaused(!gameController.GetPaused());
            buttonText.text = gameController.GetPaused() ? "Play" : "Pause";
        } else {
            Debug.Log("UIManager Error: GameController does not exist");
        }
    }
}
