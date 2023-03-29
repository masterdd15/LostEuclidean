using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharlesDoorController : Interactable
{
    [Header("Door Variables")]
    [SerializeField] public Transform front;
    [SerializeField] GameObject doorMesh;
    [SerializeField] Material disabledMat;
    [SerializeField] Material enabledMat;

    [Header("Destination Info")]
    [SerializeField] ColorRoom colorRoom;
    [SerializeField] LightColor colorOne;
    [SerializeField] LightColor colorTwo;

    Material[] doorMats;
    int materialIndex;

    // Start is called before the first frame update
    void Start()
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
        if (colorRoom.roomColor == colorOne)
        {
            GameManager.Instance.ChangeRoomColor(colorTwo);
        }
        else if (colorRoom.roomColor == colorTwo)
        {
            GameManager.Instance.ChangeRoomColor(colorOne);
        }
        else
        {
            GameManager.Instance.ChangeRoomColor(colorOne);
        }
    }

    public override void Enable()
    {
        InteractionEnabled = true;

        doorMats[materialIndex] = enabledMat;
        doorMesh.GetComponent<MeshRenderer>().materials = doorMats;
    }

    public override void Disable()
    {
        InteractionEnabled = false;

        doorMats[materialIndex] = disabledMat;
        doorMesh.GetComponent<MeshRenderer>().materials = doorMats;
    }
}
