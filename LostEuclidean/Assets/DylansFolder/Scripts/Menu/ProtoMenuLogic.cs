using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProtoMenuLogic : MonoBehaviour
{
    //Game Manager Reference
    [SerializeField] GameManager gm;
    
    //This logic will handle all the different menu screens off our game
    //We store each menu logic within GameObject

    //0 = Menu Base
    //1 = Menu Level Select
    //2 = Menu Win Screen
    [SerializeField] List<GameObject> menuScreens;

    //At the start of the menu, we should set every menu screen off, and turn on base
    public void Awake()
    {
        gm = GameManager.Instance;
        
        //Goes through every item in a menu
        if(gm.GethasWon()) //Player has already completed a level
        {
            menuScreens[0].SetActive(false);
            menuScreens[1].SetActive(true);
            menuScreens[2].SetActive(true);
        }
        else //Player is starting up game for the first time
        {
            for (int i = 0; i < menuScreens.Count; i++)
            {
                //If item is base menu, turn it on
                if (i == 0)
                {
                    menuScreens[i].SetActive(true);
                }
                else
                {
                    menuScreens[i].SetActive(false);
                }
            }
        }
    }


    //This button will show the level select screen, and hide the base menu screen
    public void HandleStartButton()
    {
        menuScreens[0].SetActive(false);
        menuScreens[1].SetActive(true);
    }

    public void HandleExitButton()
    {
        Application.Quit();
    }

    public void HandleMenu_Level_Back()
    {
        menuScreens[1].SetActive(false); //Hide Level Menu
        menuScreens[0].SetActive(true); //Show Base Menu
    }

    public void HandleMenu_Win_Back()
    {
        menuScreens[0].SetActive(false); //Hide Level Menu
        menuScreens[1].SetActive(true); //Show Base Menu
        menuScreens[2].SetActive(false); //Show Base Menu
    }
}
