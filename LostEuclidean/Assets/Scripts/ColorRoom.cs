using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRoom : MonoBehaviour
{
    public LightColor roomColor = LightColor.Blue;

    private List<ColorObject> objectsInRoom = new List<ColorObject>();

    void Start()
    {
        //adds all its children color objects to the list
        //TODO: support nested children
        for (int i = 0; i < transform.childCount; i++)
        {
            ColorObject child = transform.GetChild(i).GetComponent<ColorObject>();
            if (child != null && !objectsInRoom.Contains(child))
            {
                objectsInRoom.Add(child);
            }
        }
    }

    public void ChangeRoomColor(LightColor color)
    {
        roomColor = color;
        foreach (ColorObject colorObject in objectsInRoom)
        {
            colorObject.OnRoomColorChange();
        }
    }
}
