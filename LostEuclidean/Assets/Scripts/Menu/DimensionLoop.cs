using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionLoop : MonoBehaviour
{
    [SerializeField]
    Material mat;

    void Start()
    {
        mat.SetFloat("_Lerp", 0f);
        StartChangeCountdown(1);
    }

    IEnumerator ChangeDimension(int dest)
    {
        yield return new WaitForSeconds(6f);
        float lerp = mat.GetFloat("_Lerp");
        while (Mathf.Abs(lerp - dest) > 0.05)
        {
            lerp += Mathf.Sign(dest - lerp) * Time.deltaTime * 8;
            lerp = Mathf.Clamp(lerp, 0, 3);
            mat.SetFloat("_Lerp", lerp);
            yield return null;
        }
        mat.SetFloat("_Lerp", dest);
        if (dest >= 3)
            StartCoroutine(ChangeDimension(0));
        else
            StartCoroutine(ChangeDimension(dest + 1));
    }

    public void StartChangeCountdown(int dest)
    {
        StartCoroutine(ChangeDimension(dest));
    }
}
