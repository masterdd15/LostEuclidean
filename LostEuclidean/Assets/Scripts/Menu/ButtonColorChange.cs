using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonColorChange : MonoBehaviour, IPointerEnterHandler
{
    private ColorRoom room;
    [SerializeField]
    LightColor color;

    void Awake()
    {
        room = FindObjectOfType<ColorRoom>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        room.ChangeRoomColor(color);
    }
}
