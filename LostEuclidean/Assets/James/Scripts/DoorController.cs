using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Interactable
{
    [Header("Door Variables")]
    [SerializeField] public Transform front;
    [SerializeField] GameObject doorMesh;
    [SerializeField] Material disabledMat;
    [SerializeField] Material enabledMat;

    [Header("Destination Info")]
    [SerializeField] string sceneDestination;
    [SerializeField] string doorDestination;
    [SerializeField] LightColor colorDestination;

    Material[] doorMats;
    int materialIndex;

    // Start is called before the first frame update
    public override void Start()
    {
        doorMats = doorMesh.GetComponent<MeshRenderer>().materials;
        for (int i = 0; i < doorMats.Length; i++)
        {
            if (doorMats[i].name.Contains("Indicator"))
            {
                materialIndex = i;
            }
        }

        //Debug.Log(materialIndex);

        if (InteractionEnabled)
        {
            doorMats[materialIndex] = enabledMat;
            doorMesh.GetComponent<MeshRenderer>().materials = doorMats;
        }
        else
        {
            doorMats[materialIndex] = disabledMat;
            doorMesh.GetComponent<MeshRenderer>().materials = doorMats;
        }
    }

    public override void Interact()
    {
        if (gameObject.layer != 12 && InteractionEnabled && !hidden)
        {
            GameManager.Instance.ChangeScene(sceneDestination, doorDestination, colorDestination);
        }
    }

    public override void Enable()
    {
        InteractionEnabled = true;

        doorMats[materialIndex] = enabledMat;
        doorMesh.GetComponent<MeshRenderer>().materials = doorMats;

        //The door is enabled, so we can play a sound effect to show it has unlocked
        AudioManager.Instance.PlaySFX("Door_Unlock");
    }

    public override void Disable()
    {
        InteractionEnabled = false;

        doorMats[materialIndex] = disabledMat;
        doorMesh.GetComponent<MeshRenderer>().materials = doorMats;
    }
}
