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
    [SerializeField] private TextMesh catName;
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
        // �ν��Ͻ��� null�̸� ���� ����
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // ���� �ٲ� �ı����� �ʵ��� ����
        }
        // �ν��Ͻ��� �̹� �����ϰ�, ���� �ν��Ͻ��� �� �ν��Ͻ��� �ٸ��� �ڽ��� �ı�
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

    public void UpdateCatName(string _catName)
    {
        catName.text = _catName;
    }


    public void CreateDamageText(int damage, Vector3 hitPos)
    {
        GameObject damageTextInstance = Instantiate(damageTextPrefab, hitPos, Quaternion.identity);
        DamageText damageText = damageTextInstance.GetComponent<DamageText>();
        damageText.SetText(damage.ToString());
    }



    



}

