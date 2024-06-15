using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreathAttack : MonoBehaviour
{
    public float countTime = 0;

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartCoroutine(CoStartBreath());
    }

    private void OnDisable()
    {
        StopCoroutine(CoStartBreath());
        countTime = 0;
        transform.localScale = new Vector3(1f, 1f, 0);
    }

    IEnumerator CoStartBreath()
    {
        yield return new WaitForSeconds(1f);

        while (countTime <= 0.8f)
        {
            transform.localScale += new Vector3(0, 0, Time.deltaTime * 8);
            yield return null;
            countTime += Time.deltaTime;
        }

        yield return new WaitForSeconds(0.7f);

        countTime = 0;

        while (countTime <= 0.5f)
        {
            transform.localScale -= new Vector3(0, 0, Time.deltaTime * 12);
            yield return null;
            countTime += Time.deltaTime;
        }
        gameObject.SetActive(false);
    }
}
