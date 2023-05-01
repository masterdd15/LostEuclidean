using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionLoop : MonoBehaviour
{
    private ColorRoom room;

    void Awake()
    {
        room = FindObjectOfType<ColorRoom>();
    }

    void Start()
    {
        StartCoroutine(ChangeDimension());
    }

    IEnumerator ChangeDimension()
    {
        yield return new WaitForSeconds(10f);
        if (room.roomColor == LightColor.Red)
            room.ChangeRoomColor(LightColor.Green);
        else
            room.ChangeRoomColor(LightColor.Red);
    }
}
