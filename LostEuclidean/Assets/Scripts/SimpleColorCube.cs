using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleColorCube : ColorObject
{
    public override void OnLightEnter(LightColor lightColor)
    {
        base.OnLightEnter(lightColor);
        if (!isLightActive)
            return;
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
        base.OnLightExit(lightColor);
        if (!isLightActive)
            return;
        DisableCollider();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            EnableCollider();
        }
    }
}
