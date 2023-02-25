using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonLogic : MonoBehaviour
{
    [SerializeField]private ButtonState localState; 
    private void Awake()
    {
        //This button's collider is attached to the parent object. Note this in case any changes are made
        
        localState = ButtonState.OFF; //Button will start off, and will wait until object are detected inside of it

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
        localState = ButtonState.ON;
    }

    private void OnTriggerExit(Collider other)
    {
        localState = ButtonState.OFF;
    }

    //I'm creating a state machine for the button
    //Right now it functions as a bool, but we may want to add different weights or pressures later on
    private enum ButtonState
    {
        ON,
        OFF
    }
}
