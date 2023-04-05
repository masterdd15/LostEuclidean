using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentColorObject : ColorObject
{
    [SerializeField]
    protected bool enableCollider = false;

    protected override void OnEnable()
    {
        base.OnEnable();
        if (!enableCollider)
            DisableCollider();
    }
}
