using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorRoom : MonoBehaviour
{
    public LightColor roomColor = LightColor.Blue;

    private List<ColorObject> objectsInRoom = new List<ColorObject>();
}
