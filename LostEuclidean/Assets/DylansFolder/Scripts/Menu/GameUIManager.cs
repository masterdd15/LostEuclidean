using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameUIManager : MonoBehaviour
{
    //GameManager
    [SerializeField] GameManager gm;

    //0 = DarkenBase (Used to put focus on UI in front of game)
    //1 = PauseMenuUI
    [SerializeField] List<GameObject> gameUIScreens;

    // Start is called before the first frame update
    void Start()
    {
        gm = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    /*
     * Handles setting up the Pause menu (turns on if off, and off if on)
     * We trigger this in the GameManager, which in turn is triggered through a player controller event
     * We should make sure no other priority is active (transition between doors, cutscene, etc.) before pausing
     */
    public void TriggerPause()
    {
        Debug.Log("MADE IT TO THE PAUSE OBJECT");

        //We need to decide if our pause menu is on or off, and set it active accordingly

        //This sets the dark background to it's opposite state (kind of like an on/off switch)
        gameUIScreens[0].SetActive(!gameUIScreens[0].activeSelf);

        //This sets the pause menu UI to it's opposite state (kind of like an on/off switch)
        gameUIScreens[1].SetActive(!gameUIScreens[1].activeSelf);
    }
}
