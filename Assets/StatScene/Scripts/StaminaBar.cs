using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    private int playerNormalStamina = 100;
    public int playerCurrentStamina;
    private Slider staminaBar;

    void Start()
    {
        staminaBar = GetComponent<Slider>();
        staminaBar.wholeNumbers = true;
        playerCurrentStamina = playerNormalStamina;
    }

    void Update()
    {
        StaminaBarConnect();
        playerCurrentStamina--;
    }

    public void StaminaBarConnect()
    {
        staminaBar.value = playerCurrentStamina;
    }
}
