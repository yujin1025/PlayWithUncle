using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : UIComponent
{
    [SerializeField] GameObject uncleImage;
    [SerializeField] GameObject nephewImage;

    // [SerializeField] 페이드 시간을 조정할 필요가 있다면 사용할 것
    protected float fadeTime = 0.4f;
    protected CanvasGroup panel;

    protected bool isFadeOutProgressing = false;
    protected bool isFadeInProgressing = false;

    void Awake()
    {
        panel = GetComponent<CanvasGroup>();
        panel.alpha = 0.0f;

        if(uncleImage != null)
        {
            if (NetworkMgr.Instance.IsMasterClient())
                nephewImage.SetActive(false);
            else
                uncleImage.SetActive(false);
        }
    }

    public override void SetUI(UIParam param = null) // 페이드 인 되며 켜집니다.
    {
        if (isFadeInProgressing) return; // 이미 페이드 인 중이라면 실행하지 않습니다.

        gameObject.SetActive(true);
        StartCoroutine("FadeIn");
    }

    public override void UnsetUI() // 페이드 아웃되며 꺼집니다.
    {
        if (isFadeOutProgressing) return; // 이미 페이드 아웃 중이라면 실행하지 않습니다.

        StartCoroutine("FadeOut");
    }

    private IEnumerator FadeIn()
    {
        isFadeInProgressing = true;
        panel.alpha = 0.0f;

        float currTime = 0.0f;
        float t = 0.0f;

        while (t < 1)
        {
            currTime += Time.deltaTime;
            t = currTime / fadeTime;

            panel.alpha = Mathf.Lerp(0, 1, t);
            yield return null;
        }

        panel.alpha = 1.0f;
        isFadeInProgressing = true; // 버그 발생 여지가 있음
        base.SetUI();
    }

    private IEnumerator FadeOut()
    {
        isFadeOutProgressing = true;
        float currTime = 0.0f;
        float t = 0.0f;
        panel.alpha = 1.0f;
        while (t < 1)
        {
            currTime += Time.deltaTime;
            t = currTime / fadeTime;

            panel.alpha = Mathf.Lerp(1, 0, t);
            yield return null;
        }

        panel.alpha = 0.0f;
        isFadeOutProgressing = false;
        base.UnsetUI();
    }
}
