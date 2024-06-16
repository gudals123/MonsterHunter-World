using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageText : MonoBehaviour
{
    [SerializeField] private TextMeshPro damageText;
    private float lifeTime = 1f; 
    private float speed = 1f;

    void Awake()
    {
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);
    }

    public void SetText(string damage)
    {
        damageText.text = damage;
    }
}
