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


    public void PlayerDamageText(int damage, Vector3 hitPos)
    {
        damageTextPrefab.transform.position = hitPos;
        damageTextPrefab.GetComponent<DamageText>().SetText(damage.ToString());
        damageTextPrefab.SetActive(true);
        StartCoroutine(SetFalse(damageTextPrefab));
    }

    public IEnumerator SetFalse(GameObject gameObject)
    {
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}

