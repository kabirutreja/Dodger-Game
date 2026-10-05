
using System.Collections;
using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    public TextMeshPro tmpText;
    public float fadeDuration = 1f;
    public string format = "Score: {0}";

    private bool hasFadedIn;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (tmpText == null)
        {
            tmpText = GetComponent<TextMeshPro>();
        }
    }

    public void SetScore(int score)
    {
        if (tmpText == null) return;

        string s = string.Format(format, score);
        tmpText.text = s;
    }

    public void SetVisible(bool visible)
    {
        if (tmpText == null) return;

        if (!visible)
        {
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
                fadeCoroutine = null;
            }

            tmpText.alpha = 0f;
            hasFadedIn = false;
            return;
        }

        if (hasFadedIn) return;

        hasFadedIn = true;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeIn());
    }

    private IEnumerator FadeIn()
    {
        float t = 0f;
        tmpText.alpha = 0f;

        if (fadeDuration <= 0f)
        {
            tmpText.alpha = 1f;
            fadeCoroutine = null;
            yield break;
        }

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            float k = Mathf.SmoothStep(0f, 1f, t / fadeDuration);
            tmpText.alpha = k;

            yield return null;
        }

        tmpText.alpha = 1f;
        fadeCoroutine = null;
    }
}