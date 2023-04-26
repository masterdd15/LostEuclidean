using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallpaperColorObject : ColorObject
{
    Vector3 dir;

    protected void Start()
    {
        dir = Vector3.RotateTowards(Vector3.forward, Vector3.zero, Mathf.PI, 0);
        dir *= 0.01f;
    }

    protected override void UpdateColorLayer()
    {
        base.UpdateColorLayer();
        /*if (baseColor == room.roomColor)
        {
            transform.Translate(dir);
        }
        else
        {
            transform.Translate(-dir);
        }*/
    }
}
