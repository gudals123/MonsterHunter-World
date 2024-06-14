using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathAttack : MonoBehaviour
{
    public float countTime = 0;

    private void OnEnable()
    {
        StartCoroutine(CoStartBreath());
    }

    private void OnDisable()
    {
        StopCoroutine(CoStartBreath());
        countTime = 0;
        transform.localScale = new Vector3(0.5f, 0.5f, 0);
    }

    IEnumerator CoStartBreath()
    {
        while (countTime <= 2f)
        {
            transform.localScale += new Vector3(0, 0, Time.deltaTime * 5);
            yield return null;
            countTime += Time.deltaTime;
        }
    }

}
