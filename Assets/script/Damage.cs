using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Damage : MonoBehaviour
{
    [SerializeField] private Text damageText;
    private Vector3 textPosition;
    private Color normalDamage = Color.white; // 일반 데미지 색상
    private Color weaknessDamage = new Color(255, 183, 0, 255); // 약점 데미지 색상
    private float duration = 0;
    private float activeDuration;

    void Start()
    {
        textPosition = transform.position; // 타격 지점
    }

    void Update()
    {
        StartCoroutine(TextMove());
    }

    IEnumerator TextMove()
    {
        duration += Time.deltaTime;
        activeDuration += Time.deltaTime;
        Debug.Log(duration);
        Debug.Log(activeDuration);

        yield return new WaitForSeconds(0.01f);

        if (duration < 0.1f)
        {
            damageText.transform.position += new Vector3(0, 0.7f, 0);
        }

        else if (duration > 0.1f)
        {
            damageText.color = new Color(damageText.color.r, damageText.color.g, damageText.color.b, damageText.color.a - duration);
            if (activeDuration >= 0.5f)
            {
                damageText.gameObject.SetActive(false);
            }
            duration = 0;
        }

    }
}
