using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonColorChange : MonoBehaviour, IPointerEnterHandler
{
    private DimensionLoop dl;
    [SerializeField]
    float destination = 0;
    [SerializeField]
    Material mat;
    [SerializeField]
    float speed = 5;

    void Awake()
    {
        dl = FindObjectOfType<DimensionLoop>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        StopAllCoroutines();
        StartCoroutine(StartChange());
        dl.StopAllCoroutines();
        dl.StartChangeCountdown();
    }

    IEnumerator StartChange()
    {
        float lerp = mat.GetFloat("_Lerp");
        while (Mathf.Abs(lerp - destination) > 0.00001)
        {
            lerp += Mathf.Sign(destination - lerp) * Time.deltaTime * speed;
            lerp = Mathf.Clamp(lerp, 0, 1);
            mat.SetFloat("_Lerp", lerp);
            yield return null;
        }
    }
}
