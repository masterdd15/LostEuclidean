using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionLoop : MonoBehaviour
{
    [SerializeField]
    Material mat;

    void Start()
    {
        StartChangeCountdown();
    }

    IEnumerator ChangeDimension()
    {
        yield return new WaitForSeconds(6f);
        float lerp = mat.GetFloat("_Lerp");
        if (lerp > 0.5f)
        {
            while (Mathf.Abs(lerp - 0) > 0.00001)
            {
                lerp += Mathf.Sign(0 - lerp) * Time.deltaTime * 8;
                lerp = Mathf.Clamp(lerp, 0, 1);
                mat.SetFloat("_Lerp", lerp);
                yield return null;
            }
        }
        else
        {
            while (Mathf.Abs(lerp - 1) > 0.00001)
            {
                lerp += Mathf.Sign(1 - lerp) * Time.deltaTime * 8;
                lerp = Mathf.Clamp(lerp, 0, 1);
                mat.SetFloat("_Lerp", lerp);
                yield return null;
            }
        }
        StartCoroutine(ChangeDimension());
    }

    public void StartChangeCountdown()
    {
        StartCoroutine(ChangeDimension());
    }
}
