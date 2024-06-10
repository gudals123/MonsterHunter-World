using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneChange : MonoBehaviour
{
    [SerializeField] private Image image;

    public void StartFade()
    {
        StartCoroutine("FadeOut");
    }

    public void Update()
    {
        if(Input.anyKeyDown)
            StartFade();
    }

    IEnumerator FadeOut()
    {
        float startAlpha = 0;
        while (startAlpha < 1.0f)
        {
            startAlpha += 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, startAlpha);
        }
        StopCoroutine(FadeOut());
        SceneManager.LoadScene("ProtoType_6.8");
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float startAlpha = 1;
        while (startAlpha > 0f)
        {
            startAlpha -= 0.01f;
            yield return new WaitForSeconds(0.01f);
            image.color = new Color(0, 0, 0, startAlpha);
        }
    }
}
