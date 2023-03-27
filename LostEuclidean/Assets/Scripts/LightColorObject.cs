using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColorObject : ColorObject
{
    Light lampLight;

    protected override void Awake()
    {
        base.Awake();

        lampLight = transform.GetComponentInChildren<Light>();
    }

    protected void Start()
    {
        if (gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            EnableCollider();
            EnableLamp();
        }
        else
        {
            DisableCollider();
            DisableLamp();
        }
    }

    public override void OnLightEnter(LightColor lightColor)
    {
        Debug.Log(gameObject.name + " OnLightEnter");
        base.OnLightEnter(lightColor);
        if (!isLightActive)
            return;
        if (lightColor == baseColor)
        {
            EnableCollider();
            EnableLamp();
        }
        else
        {
            DisableCollider();
            DisableLamp();
        }
    }

    public override void OnLightExit(LightColor lightColor)
    {
        //Debug.Log(gameObject.name + " OnLightExit");
        base.OnLightExit(lightColor);
        if (!isLightActive)
            return;
        DisableCollider();
        DisableLamp();
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        if (gameObject.layer == LayerMask.NameToLayer("Default"))
        {
            EnableCollider();
            EnableLamp();
        }
    }

    void EnableLamp()
    {
        lampLight.enabled = true;
    }

    void DisableLamp()
    {
        lampLight.enabled = false;
    }

}
