using System.Collections;
using System.Collections.Generic;
using UnityEditor.Purchasing;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager instance;

    [SerializeField] private Slider hpBar;
    [SerializeField] private Slider spBar;
    [SerializeField] GameObject damageTextPrefab;

    public static UIManager Instance
    {
        get
        {
            if (instance == null)
            {
                return FindObjectOfType<UIManager>();
            }
            else
            {
                return instance;
            }
        }
    }

    private void Awake()
    {
        // 인스턴스가 null이면 새로 생성
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // 씬이 바뀌어도 파괴되지 않도록 설정
        }
        // 인스턴스가 이미 존재하고, 현재 인스턴스가 그 인스턴스와 다르면 자신을 파괴
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void UpdateHPBar(float currentHP , float maxHP)
    {
        hpBar.maxValue = maxHP;
        hpBar.value = currentHP;
    }
    public void UpdateSPBar(float currentSP, float maxSP)
    {
        spBar.maxValue = maxSP;
        spBar.value = currentSP;
    }



    public void CreateDamageText(int damage, Vector3 hitPos)
    {
        GameObject damageTextInstance = Instantiate(damageTextPrefab, hitPos, Quaternion.identity);
        DamageText damageText = damageTextInstance.GetComponent<DamageText>();
        damageText.SetText(damage.ToString());
    }



    



}

