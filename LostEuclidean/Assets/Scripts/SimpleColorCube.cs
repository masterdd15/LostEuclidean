using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleColorCube : ColorObject
{
    public override void OnLightEnter(LightColor lightColor)
    {
        if (lightColor == baseColor)
        {
            EnableCollider();
        }
        else
        {
            DisableCollider();
        }
    }

    public override void OnLightExit(LightColor lightColor)
    {
        DisableCollider();
    }
}
