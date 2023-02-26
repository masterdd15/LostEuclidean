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
    [SerializeField] string colorDestination;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Interact()
    {
        if (gameObject.layer != 12 && InteractionEnabled)
        {
            GameManager.Instance.ChangeScene(sceneDestination, doorDestination, colorDestination);
        }
    }

    public override void Enable()
    {
        InteractionEnabled = true;

        doorMesh.GetComponent<MeshRenderer>().material = enabledMat;
    }

    public override void Disable()
    {
        InteractionEnabled = false;

        doorMesh.GetComponent<MeshRenderer>().material = disabledMat;
    }
}
