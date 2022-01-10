using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModeToggle : MonoBehaviour
{
    private GameMannager gameManager;

    [SerializeField] private GameObject lowHpToggleButtonOn;
    [SerializeField] private GameObject lowHpToggleButtonOff;



    void Start()
    {
        gameManager = GameMannager.Instance;
    }

    public void LowHpToggleOn() {
        gameManager.gameController.LowHpToggle = true;
        lowHpToggleButtonOn.SetActive(true);
        lowHpToggleButtonOff.SetActive(false);
    }    

    public void LowHpToggleOff() {
        gameManager.gameController.LowHpToggle = false;
        lowHpToggleButtonOn.SetActive(false);
        lowHpToggleButtonOff.SetActive(true);
    }
}
