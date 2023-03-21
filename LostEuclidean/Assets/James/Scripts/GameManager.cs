using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //Temp boolean variable to show when the player has won the game
    [SerializeField] bool hasWon = false;

    //We are going to have a bool that keeps track of if the game is paused or not
    [SerializeField] bool isPaused = false;

    //Find the current pauseMenu screen

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ChangeScene(string sceneName, string doorName, LightColor color)
    {
        StartCoroutine(LoadNewScene(sceneName, doorName, color));
    }

    IEnumerator LoadNewScene(string sceneName, string doorName, LightColor color)
    {
        if (sceneName != SceneManager.GetActiveScene().name)
        {
            // Fade out
            Image fadingImage = GameObject.Find("FadeImage").GetComponent<FullscreenFadeController>().FadeOut();

            while (fadingImage.color.a < 1)
            {
                yield return null;
            }

            // Load the new scene if necessary
            AsyncOperation newScene = SceneManager.LoadSceneAsync(sceneName);

            while (!newScene.isDone)
            {
                yield return null;
            }
        }

        // Place the player at the right door
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        DoorController door = GameObject.Find(doorName).GetComponent<DoorController>();

        player.transform.position = door.front.position;

        // Set the color
        GameObject[] colorRooms = GameObject.FindGameObjectsWithTag("ColorRoom");
        foreach (GameObject colorRoom in colorRooms)
        {
            colorRoom.GetComponent<ColorRoom>().ChangeRoomColor(color);
        }
    }

    //Getter and Setter Methods to change the hasWon boolean
    
    public bool GethasWon()
    {
        return hasWon;
    }
    
    public void HasWonOn()
    {
        hasWon = true;
    }

    public void HasWonOff()
    {
        hasWon = true;
    }

    /*This starts the sequence of events to pause the game
     * Later on, we may have cutscenes or transitions were we DON'T want the player to pause
     * This gateway will help us implement checks later on
     */
    public void HandlePause()
    {
        GameObject UICheck = GameObject.Find("GameUI_Manager");
        if (UICheck != null)
        {
            isPaused = !isPaused; //Flip isPaused on or off depending on what it is currently
            UICheck.GetComponent<GameUIManager>().TriggerPause();

            //If true, we need to stop all time in the game to fully be paused
            if (isPaused)
            {
                Time.timeScale = 0.0f;
            }
            else //If false, we need to resume the timescale to it's normal values
            {
                Time.timeScale = 1.0f;
            }
        }
        else
        {
            Debug.Log("GAMEMANAGER ERROR: INGAME UI NOT FOUND");
        }
    }
}
