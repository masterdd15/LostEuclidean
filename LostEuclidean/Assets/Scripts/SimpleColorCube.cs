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
            //EnablePhysics();
            EnableInteract();
        }
        else
        {
            DisableCollider();
            //DisablePhysics();
            DisableInteract();
        }
    }

    public override void OnLightEnter(LightColor lightColor)
    {
        //Debug.Log(gameObject.name + " OnLightEnter");
        base.OnLightEnter(lightColor);
        if (!isLightActive)
            return;
        if (lightColor == baseColor)
        {
            EnableCollider();
            //EnablePhysics();
            EnableInteract();
        }
        else
        {
            //DisableCollider();
            //DisablePhysics();
            //DisableInteract();
        }
    }

    public override void OnLightExit(LightColor lightColor)
    {
        //Debug.Log(gameObject.name + " OnLightExit");
        base.OnLightExit(lightColor);
        if (!isLightActive)
            return;
        DisableCollider();
        //DisablePhysics();
        DisableInteract();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            EnableCollider();
            //EnablePhysics();
            EnableInteract();
        }
    }

}
