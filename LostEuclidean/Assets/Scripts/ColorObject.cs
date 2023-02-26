using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColorObject : MonoBehaviour
{
    [SerializeField]
    protected LightColor baseColor;
    [SerializeField]
    protected bool interactable = false;

    protected Collider col;
    protected ColorRoom room;
    protected bool isLightActive = true;
    protected bool canInteract = false;

    public virtual void OnLightEnter(LightColor lightColor)
    {
        if (!isLightActive)
            return;
    }
    public virtual void OnLightExit(LightColor lightColor)
    {
        if (!isLightActive)
            return;
    }
    public virtual void Interact() { }


    protected void EnableInteract()
    {
        canInteract = true;
        
    }

    protected void DisableInteract()
    {
        canInteract = false;
    }

    protected void EnableCollider()
    {
        col.isTrigger = false;
    }

    protected void DisableCollider()
    {
        col.isTrigger = true;
    }


    protected void UpdateColorLayer()
    {
        if (baseColor == room.roomColor)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            isLightActive = false;
            return;
        }
        
        if (baseColor == LightColor.Red)
        {
            gameObject.layer = LayerMask.NameToLayer("LightObject_Red");
        }
        else if (baseColor == LightColor.Green)
        {
            gameObject.layer = LayerMask.NameToLayer("LightObject_Green");
        }
        else if (baseColor == LightColor.Blue)
        {
            gameObject.layer = LayerMask.NameToLayer("LightObject_Blue");
        }
        isLightActive = true;
    }

    protected virtual void Awake()
    {
        col = GetComponent<Collider>();
        room = GetComponentInParent<ColorRoom>();
    }

    protected virtual void OnEnable()
    {
        UpdateColorLayer();
    }
}
