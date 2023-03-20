using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{
    [SerializeField] private ButtonState localState;

    [SerializeField] Interactable connectedInteractable;

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

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerStay(Collider other)
    {
        ColorObject colorObj = other.GetComponent<ColorObject>();

        if (colorObj != null)
        {
            //if (other.tag != "Flashlight")
            //    Debug.Log(localState.ToString() + " - " + other.gameObject.name + " - " + colorObj.CanInteract() + " - " + buttonColorObject.CanInteract());

            if (colorObj.CanInteract() && buttonColorObject.CanInteract() && localState != ButtonState.ON)
            {
                localState = ButtonState.ON;

                if (connectedInteractable != null)
                    connectedInteractable.Enable();
            }
            else if ((!colorObj.CanInteract() || !buttonColorObject.CanInteract()) && localState == ButtonState.ON)
            {
                localState = ButtonState.OFF;

                if (connectedInteractable != null)
                    connectedInteractable.Disable();
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
        if (other.gameObject.tag != "Flashlight")
        {
            //Debug.Log("EXITING: " + other.name);

            localState = ButtonState.OFF;

            if (connectedInteractable != null)
                connectedInteractable.Disable();
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
