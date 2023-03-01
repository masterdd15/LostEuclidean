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
    [SerializeField]
    protected int colorObjectID = 0;

    protected Collider col;
    protected ColorRoom room;
    protected Rigidbody rigidBody;

    protected bool isLightActive = true;
    protected bool canInteract = false;

    protected virtual void Awake()
    {
        col = GetComponent<Collider>();
        room = GetComponentInParent<ColorRoom>(); //TODO: support nested parents
        rigidBody = GetComponent<Rigidbody>();
    }

    protected virtual void OnEnable()
    {
        UpdateColorLayer();
    }

    public virtual void OnLightEnter(LightColor lightColor) { }
    public virtual void OnLightExit(LightColor lightColor) { }
    public virtual void Interact() { }

    //Bunch of handy functions
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

    protected void EnablePhysics()
    {
        if (rigidBody != null)
        {
            rigidBody.isKinematic = false;
        }
    }

    protected void DisablePhysics()
    {
        if (rigidBody != null)
        {
            rigidBody.isKinematic = true;
        }
    }

    //Put the object onto the right layer based on room color
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

    //Handle for when the room changes color at runtime
    public virtual void OnRoomColorChange()
    {
        UpdateColorLayer();
    }

    public bool CanInteract()
    {
        return canInteract;
    }
}
