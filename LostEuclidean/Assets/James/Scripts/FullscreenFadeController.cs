using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullscreenFadeController : MonoBehaviour
{
    [SerializeField] Image BlackScreen;
    [SerializeField] float fadeRate;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeInRoutine());
    }

    public Image FadeOut()
    {
        StartCoroutine(FadeOutRoutine());

        return BlackScreen;
    }

    IEnumerator FadeInRoutine()
    {
        float t = 0;
        while (t <= 1)
        {
            Color screenColor = BlackScreen.color;
            screenColor.a = Mathf.Lerp(1, 0, t);
            BlackScreen.color = screenColor;

            yield return new WaitForSeconds(0.025f);

            t += Time.deltaTime * fadeRate;
        }

        // Ensure that the alpha is 0
        Color finalColor = BlackScreen.color;
        finalColor.a = 0f;
        BlackScreen.color = finalColor;
    }
    
    IEnumerator FadeOutRoutine()
    {
        float t = 0;
        while (t < 1)
        {
            Color screenColor = BlackScreen.color;
            screenColor.a = Mathf.Lerp(0, 1, t);
            BlackScreen.color = screenColor;

            yield return new WaitForSeconds(0.025f);

            t += Time.deltaTime * fadeRate;
        }

        // Ensure that the alpha is 1
        Color finalColor = BlackScreen.color;
        finalColor.a = 1f;
        BlackScreen.color = finalColor;
    }
}
