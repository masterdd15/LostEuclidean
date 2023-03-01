using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonColorObject : ColorObject
{
    protected void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Default"))
        {
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
            EnablePhysics();
            EnableInteract();
        }
        else
        {
            DisableCollider();
            DisablePhysics();
            DisableInteract();
        }
    }

    public override void OnLightExit(LightColor lightColor)
    {
        base.OnLightExit(lightColor);
        if (!isLightActive)
            return;
        DisableCollider();
        DisablePhysics();
        DisableInteract();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            EnablePhysics();
            EnableInteract();
        }
    }
}
