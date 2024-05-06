using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Fade : MonoBehaviour
{
    [SerializeField]
    [Range(0.01f, 10f)]
    private float fadeTime;
    private Image image;

    public void Awake()
    {
        image = GetComponent<Image>();

        StartCoroutine("FadeIn");
    }
    private IEnumerator FadeIn()
    {
        float currTime = 0.0f;
        float t = 0.0f;

        while (t < 1)
        {
            currTime += Time.deltaTime;
            t = currTime / fadeTime;

            Color color = image.color;
            color.a = Mathf.Lerp(0, 1, t);
            image.color = color;

            yield return null;
        }
    }


}
