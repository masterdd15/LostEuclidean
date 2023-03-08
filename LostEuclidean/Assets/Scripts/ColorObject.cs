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
    protected bool canGlitch = false;

    [Space(10), Header("Dependencies"), SerializeField]
    protected Material[] glitchMaterials = new Material[3];

    protected Collider col;
    protected ColorRoom room;
    protected Rigidbody rigidBody;
    protected MeshRenderer meshRenderer;
    [SerializeField]
    protected ParticleSystem glitchParticles; //component of direct child object

    protected bool isLightActive = true;
    protected bool canInteract = false;
    protected bool isGlitching = false;

    protected virtual void Awake()
    {
        col = GetComponent<Collider>();
        room = GetComponentInParent<ColorRoom>(); //TODO: support nested parents
        rigidBody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        glitchParticles = GetComponentInChildren<ParticleSystem>();
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

    protected void StartGlitching()
    {
        if (isGlitching || !canGlitch)
            return;
        isGlitching = true;
        Material[] materials = meshRenderer.materials;
        Material[] newMaterials = new Material[materials.Length + 3];
        for (int i = 0; i < materials.Length; i++)
        {
            newMaterials[i] = materials[i];
        }
        for (int i = 0; i < 3; i++)
        {
            newMaterials[materials.Length + i] = glitchMaterials[i];
        }
        meshRenderer.materials = newMaterials;
        if (glitchParticles != null)
            glitchParticles.Play();
    }

    protected void StopGlitching()
    {
        if (!isGlitching)
            return;
        isGlitching = false;
        Material[] materials = meshRenderer.materials;
        Material[] newMaterials = new Material[materials.Length - 3];
        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i] = materials[i];
        }
        meshRenderer.materials = newMaterials;
        if (glitchParticles != null)
            glitchParticles.Stop();
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
