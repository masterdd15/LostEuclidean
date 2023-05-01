using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonColorChange : MonoBehaviour, IPointerEnterHandler
{
    private ColorRoom room;
    private DimensionLoop dl;
    [SerializeField]
    LightColor color;

    void Awake()
    {
        dl = FindObjectOfType<DimensionLoop>();
        room = FindObjectOfType<ColorRoom>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        room.ChangeRoomColor(color);
        dl.StopAllCoroutines();
        dl.StartChangeCountdown();
    }
}
