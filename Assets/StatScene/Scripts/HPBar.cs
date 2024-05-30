using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    private int playerNormalHP = 100;
    public int playerCurrentHP;
    private Slider hpBar;

    void Start()
    {
        hpBar = GetComponent<Slider>();
        hpBar.wholeNumbers = true; // ������ ���
        playerCurrentHP = playerNormalHP;
    }

    void Update()
    {
        HPBarConnect();

        if (playerCurrentHP < 0)
        {
            Debug.Log("���� ���߽��ϴ�.");
        }
    }

    public void HPBarConnect()
    {
        hpBar.value = playerCurrentHP;
    }
}
