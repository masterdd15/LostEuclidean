using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRoom : MonoBehaviour
{
    public LightColor roomColor = LightColor.Blue;

    Light directionalLight;
    [SerializeField] Color greenDirectionalLight;
    [SerializeField] Color redDirectionalLight;
    [SerializeField] Color blueDirectionalLight;

    private List<ColorObject> objectsInRoom = new List<ColorObject>();

    void Start()
    {
        //adds all its children color objects to the list
        //TODO: support nested children
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    ColorObject child = transform.GetChild(i).GetComponent<ColorObject>();
        //    if (child != null && !objectsInRoom.Contains(child))
        //    {
        //        objectsInRoom.Add(child);
        //    }
        //}

        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();

        if (roomColor == LightColor.Green)
        {
            directionalLight.color = greenDirectionalLight;
        }
        else if (roomColor == LightColor.Red)
        {
            directionalLight.color = redDirectionalLight;
        }
        else if (roomColor == LightColor.Blue)
        {
            directionalLight.color = blueDirectionalLight;
        }

        foreach (ColorObject colorChild in transform.GetComponentsInChildren<ColorObject>())
        {
            if (!objectsInRoom.Contains(colorChild))
            {
                objectsInRoom.Add(colorChild);
            }
        }
    }

    public void ChangeRoomColor(LightColor color)
    {
        roomColor = color;

        //Checks if we need to switch the music or not
        AudioManager.Instance.HandleCurrentDimension(roomColor);

        if (roomColor == LightColor.Green)
        {
            directionalLight.color = greenDirectionalLight;
        }
        else if (roomColor == LightColor.Red)
        {
            directionalLight.color = redDirectionalLight;
        }
        else if (roomColor == LightColor.Blue)
        {
            directionalLight.color = blueDirectionalLight;
        }

        foreach (ColorObject colorObject in objectsInRoom)
        {
            colorObject.OnRoomColorChange();
        }
    }
}
