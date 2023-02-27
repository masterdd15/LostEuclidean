using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleColorCube : ColorObject
{
    protected void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            EnableCollider();
            EnablePhysics();
        }
        else
        {
            DisableCollider();
            DisablePhysics();
        }
    }

    public override void OnLightEnter(LightColor lightColor)
    {
        base.OnLightEnter(lightColor);
        if (!isLightActive)
            return;
        if (lightColor == baseColor)
        {
            EnableCollider();
            EnablePhysics();
        }
        else
        {
            DisableCollider();
            DisablePhysics();
        }
    }

    public override void OnLightExit(LightColor lightColor)
    {
        base.OnLightExit(lightColor);
        if (!isLightActive)
            return;
        DisableCollider();
        DisablePhysics();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            EnableCollider();
            EnablePhysics();
        }
    }

}
