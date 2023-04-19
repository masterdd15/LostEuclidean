using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableColorObject : ColorObject
{
    protected override void UpdateColorLayer()
    {
        base.UpdateColorLayer();
        if (baseColor != room.roomColor)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);
    }
}
