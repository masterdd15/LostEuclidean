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
    protected bool canInteract = false;

    public virtual void OnLightEnter(LightColor lightColor) { }
    public virtual void OnLightExit(LightColor lightColor) { }
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

    protected void Awake()
    {
        col = GetComponent<Collider>();
    }
}
