using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{
    [SerializeField] private ButtonState localState;
    [SerializeField] private bool onPlayed; //We are using on trigger stay, so we need a bool to keep track of this

    [SerializeField] List<Interactable> connectedInteractables;

    List<GameObject> currentCollisions;

    //[SerializeField] Interactable connectedInteractable;

    ColorObject buttonColorObject;

    private void Awake()
    {
        //This button's collider is attached to the parent object. Note this in case any changes are made
        
        localState = ButtonState.OFF; //Button will start off, and will wait until object are detected inside of it

        buttonColorObject = GetComponent<ColorObject>();
    }


    // Start is called before the first frame update
    void Start()
    {
        currentCollisions = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        ColorObject colorObj = other.GetComponent<ColorObject>();

        if ((colorObj != null && other.tag == "Interactable") || other.tag == "Player")
        {
            if (!currentCollisions.Contains(other.gameObject))
                currentCollisions.Add(other.gameObject);

            //if (other.tag != "Flashlight")
            //    Debug.Log(localState.ToString() + " - " + other.gameObject.name + " - " + colorObj.CanInteract() + " - " + buttonColorObject.CanInteract());
            //If we can interact, and the button hasn't been turned on yet
            if ((other.tag == "Player" || colorObj.CanInteract()) && buttonColorObject.CanInteract() && localState != ButtonState.ON)
            {
                localState = ButtonState.ON;

                if(!onPlayed)
                {
                    Debug.Log("We are triggering the audio");
                    //We trigger the sound effect in the Audio Manager
                    AudioManager.Instance.PlaySFX("But_Act");
                    //We then flip the bool, so we don't trigger again until exit
                    onPlayed = true;
                }

                foreach (Interactable connected in connectedInteractables)
                {
                    connected.Enable();
                }
                    
            }
            else if (other.tag != "Player" && (!colorObj.CanInteract() || !buttonColorObject.CanInteract()) && localState == ButtonState.ON)
            {
                localState = ButtonState.OFF;
                onPlayed = false;

                foreach (Interactable connected in connectedInteractables)
                {
                    connected.Disable();
                }
            }
        }

        //if (localState != ButtonState.ON && other.gameObject.tag != "Flashlight" && other.gameObject.tag != "Player" && !other.isTrigger && buttonColorObject.CanInteract())
        //{
        //    Debug.Log("TURNING ON: " + other.name);

        //    localState = ButtonState.ON;

        //    if (connectedInteractable != null)
        //        connectedInteractable.Enable();
        //}
        //else if (localState == ButtonState.ON && other.tag != "Flashlight" && (other.isTrigger || !buttonColorObject.CanInteract()))
        //{
        //    //Debug.Log("TURNING OFF: " + other.name);

        //    localState = ButtonState.OFF;

        //    if (connectedInteractable != null)
        //        connectedInteractable.Disable();
        //}
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player" || other.tag == "Interactable")
        {
            if (currentCollisions.Contains(other.gameObject))
                currentCollisions.Remove(other.gameObject);

            if (currentCollisions.Count <= 0)
            {
                localState = ButtonState.OFF;
                onPlayed = false;

                foreach (Interactable connected in connectedInteractables)
                {
                    connected.Disable();
                }
            }
        }
    }

    //I'm creating a state machine for the button
    //Right now it functions as a bool, but we may want to add different weights or pressures later on
    private enum ButtonState
    {
        ON,
        OFF
    }
}
