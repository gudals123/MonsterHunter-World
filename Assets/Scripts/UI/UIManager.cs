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
    [SerializeField] private GameObject damageTextPrefab;
    [SerializeField] private Image currentSlot;
    [SerializeField] private Image leftSlot;
    [SerializeField] private Image rightSlot;
    [SerializeField] private Image[] slotsIcon;


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
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
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

    public void UpdateQuickSlotIcon(Slot[] quickSlot, int currentIndex, int quickSlotCount)
    {
        int leftIndex = (currentIndex - 1 + quickSlotCount) % (quickSlotCount);
        int rightIndex = (currentIndex + 1) % (quickSlotCount);
        
        currentSlot.sprite = quickSlot[currentIndex].icon.sprite;
        leftSlot.sprite = quickSlot[leftIndex].icon.sprite;
        rightSlot.sprite = quickSlot[rightIndex].icon.sprite;
    }

    public void SetIcon(Slot slot)
    {
        if(slot.GetType() == typeof(Item_Potion)) 
        {
            slot.icon = slotsIcon[0];
        }
        else if (slot.GetType() == typeof(Skill_CatHeal))
        {
            slot.icon = slotsIcon[1];
        }
        else if (slot.GetType() == typeof(NullSlot))
        {
            slot.icon = slotsIcon[2];
        }
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

