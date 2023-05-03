using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelTitleController : MonoBehaviour
{
    [SerializeField] Image BG;
    [SerializeField] public TMP_Text levelTitle;
    [SerializeField] float waitTime;
    [SerializeField] float fadeRate;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RevealTitle());
    }

    IEnumerator RevealTitle()
    {
        float startA = 0f;
        float imageA = 0f;
        float t = 0f;
        while (t < 1)
        {
            Color newTitleColor = levelTitle.color;
            newTitleColor.a = Mathf.Lerp(startA, 1f, t);
            levelTitle.color = newTitleColor;

            Color newImageColor = BG.color;
            newImageColor.a = Mathf.Lerp(imageA, 1f, t);
            BG.color = newImageColor;

            yield return new WaitForSeconds(0.01f);

            t += Time.deltaTime;
        }

        yield return new WaitForSeconds(waitTime);

        startA = levelTitle.color.a;
        imageA = BG.color.a;
        t = 0f;
        while (t < 1)
        {
            Color newTitleColor = levelTitle.color;
            newTitleColor.a = Mathf.Lerp(startA, 0f, t);
            levelTitle.color = newTitleColor;

            Color newImageColor = BG.color;
            newImageColor.a = Mathf.Lerp(imageA, 0f, t);
            BG.color = newImageColor;

            yield return new WaitForSeconds(0.01f);

            t += Time.deltaTime;
        }

        Color finalTitleColor = levelTitle.color;
        finalTitleColor.a = 0f;
        levelTitle.color = finalTitleColor;

        Color finalImageColor = BG.color;
        finalImageColor.a = 0f;
        BG.color = finalImageColor;
    }
}
