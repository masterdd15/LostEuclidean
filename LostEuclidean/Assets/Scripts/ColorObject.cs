using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class ColorObject : MonoBehaviour
{
    [SerializeField]
    protected LightColor baseColor;
    [SerializeField]
    protected bool canBeRevealed = true;
    [SerializeField]
    protected bool interactable = false;
    [SerializeField]
    protected bool canGlitch = false;
    [SerializeField]
    protected bool canCauseGlitch = false;

    [Space(10), Header("Dependencies"), SerializeField]
    protected Material[] glitchMaterials = new Material[3];

    protected Collider col;
    protected ColorRoom room;
    protected Rigidbody rigidBody;
    protected MeshRenderer meshRenderer;
    protected ParticleSystem glitchParticles; //component of direct child object

    [HideInInspector]
    public bool isRevealed = false; //true if visible
    protected bool isLightActive = true; //true if on a LightColor layer
    protected bool canInteract = false;
    protected bool isGlitching = false;

    protected List<ColorObject> glitchersTouching = new List<ColorObject>();

    protected virtual void Awake()
    {
        col = GetComponent<Collider>();
        room = GetComponentInParent<ColorRoom>(); //TODO: support nested parents
        rigidBody = GetComponent<Rigidbody>();
        meshRenderer = GetComponent<MeshRenderer>();
        glitchParticles = GetComponentInChildren<ParticleSystem>();

        //fallback glitch materials if they are not manually assigned
        //if (glitchMaterials[0] == null)
        //    glitchMaterials[0] = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/Mat_Glitch/Mat_GlitchRed.mat", typeof(Material));
        //if (glitchMaterials[1] == null)
        //    glitchMaterials[1] = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/Mat_Glitch/Mat_GlitchGreen.mat", typeof(Material));
        //if (glitchMaterials[2] == null)
        //    glitchMaterials[2] = (Material)AssetDatabase.LoadAssetAtPath("Assets/Materials/Mat_Glitch/Mat_GlitchBlue.mat", typeof(Material));
    }

    protected virtual void OnEnable()
    {
        UpdateColorLayer();
    }

    public virtual void OnLightEnter(LightColor lightColor)
    {
        if (!isLightActive)
            return;
        if (lightColor != baseColor)
            return;
        isRevealed = true;
        meshRenderer.enabled = true;
    }

    public virtual void OnLightExit(LightColor lightColor)
    {
        if (!isLightActive)
            return;
        if (!isRevealed)
            return;
        isRevealed = false;
        meshRenderer.enabled = false;
    }

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
        if (col != null)
            col.isTrigger = false;
    }

    protected void DisableCollider()
    {
        if (col != null)
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
        Debug.Log(gameObject.name + " Start Glitch");
        isGlitching = true;
        DisableInteract();
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
        Debug.Log("Stop glitching");
        isGlitching = false;
        EnableInteract();
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
    virtual protected void UpdateColorLayer()
    {
        if (baseColor == room.roomColor)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");

            EnableInteract();
            EnableCollider();
            //EnablePhysics();

            isLightActive = false && canBeRevealed;
            isRevealed = true;
            meshRenderer.enabled = true;
            return;
        }

        DisableCollider();
        DisableInteract();
        //DisablePhysics();
        isRevealed = false;
        meshRenderer.enabled = false;

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
        isLightActive = true && canBeRevealed;
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

    protected void OnTriggerEnter(Collider other)
    {
        ColorObject co = other.GetComponent<ColorObject>();
        if (co != null)
        {
            if (canGlitch && co.canCauseGlitch)
            {
                if (!glitchersTouching.Contains(co))
                    glitchersTouching.Add(co);
                //update itself as other color objects' glitcher
                foreach (ColorObject glitcher in glitchersTouching)
                {
                    if (canCauseGlitch && glitcher.canGlitch && !glitcher.glitchersTouching.Contains(this))
                        glitcher.glitchersTouching.Add(this);
                }
            }
        }
    }

    protected void OnTriggerExit(Collider other)
    {
        //Debug.Log(gameObject.name + " Trigger Exit");
        ColorObject co = other.GetComponent<ColorObject>();
        if (co != null)
        {
            if ((col.isTrigger || co.GetComponent<Collider>().isTrigger) && glitchersTouching.Contains(co))
            {
                glitchersTouching.Remove(co);
            }
            UpdateGlitchState();
        }
    }

    protected void UpdateGlitchState ()
    {
        if (isGlitching)
        {
            DisableInteract();
        }
        if ((isRevealed || baseColor == room.roomColor) && glitchersTouching.Count > 0)
        {
            foreach (ColorObject glitcher in glitchersTouching)
            {
                if (glitcher.isRevealed == true)
                {
                    StartGlitching();
                    return;
                }
            }
        }
        StopGlitching();
    }

    protected void LateUpdate()
    {
        UpdateGlitchState();
    }
}
