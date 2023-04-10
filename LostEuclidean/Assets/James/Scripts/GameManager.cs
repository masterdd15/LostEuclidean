using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering;
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
        else
        {
            Volume volume = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Volume>();

            ChromaticAberration chrome;
            volume.profile.TryGet<ChromaticAberration>(out chrome);

            //Checks if we need to switch the music or not
            AudioManager.Instance.HandleCurrentDimension(color);

            if (chrome != null)
            {
                // Place the player at the right door
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                DoorController door = GameObject.Find(doorName).GetComponent<DoorController>();

                player.transform.position = door.front.position;

                // Determine if we need to rotate the room at all
                CameraController mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraController>();
                //bool camIn180 = 0 <= mainCamera.transform.rotation.eulerAngles.y && mainCamera.transform.rotation.eulerAngles.y <= 180f;
                //bool doorIn180 = 0 <= door.transform.rotation.eulerAngles.y && door.transform.rotation.eulerAngles.y <= 180f;

                Vector3 doorToCam = (mainCamera.transform.position - door.transform.position).normalized;
                float direction = Vector3.Dot(doorToCam, door.transform.forward);

                bool xOpposite = (mainCamera.transform.position.x < 0 && door.transform.position.x > 0) || (mainCamera.transform.position.x > 0 && door.transform.position.x < 0);
                bool zOpposite = (mainCamera.transform.position.z < 0 && door.transform.position.z > 0) || (mainCamera.transform.position.z > 0 && door.transform.position.z < 0);

                if (direction < 0)
                {
                    //Debug.Log(mainCamera.transform.rotation.eulerAngles.y + " - " + door.transform.rotation.eulerAngles.y);
                    if (mainCamera.transform.rotation.eulerAngles.y > door.transform.rotation.eulerAngles.y)
                    {
                        mainCamera.RotateCamera(1f);
                    }
                    else
                    {
                        mainCamera.RotateCamera(-1f);
                    }
                    //mainCamera.Rotate180Degrees();
                }

                //if (xOpposite && zOpposite)
                //{
                //    mainCamera.Rotate180Degrees();
                //}
                //else if (!xOpposite && !zOpposite)
                //{
                //    // Do nothing
                //}
                //else
                //{
                //    float angle = Vector3.Angle(mainCamera.transform.forward, doorToCam);
                //    Vector3 cross = Vector3.Cross(mainCamera.transform.forward, doorToCam);

                //    if (cross.y < 0)
                //        angle = -angle;

                //    if (direction < 0)
                //    {
                //        if (angle < 0)
                //        {
                //            mainCamera.RotateCamera(-1f);
                //        }
                //        else
                //        {
                //            mainCamera.RotateCamera(1f);
                //        }

                //        //if (mainCamera.transform.rotation.eulerAngles.y > door.transform.rotation.eulerAngles.y)
                //        //{
                //        //    mainCamera.RotateCamera(1f);
                //        //}
                //        //else
                //        //{
                //        //    mainCamera.RotateCamera(-1f);
                //        //}
                //    }
                //    else
                //    {
                //        if (angle < 0)
                //        {
                //            mainCamera.RotateCamera(1f);
                //        }
                //        else
                //        {
                //            mainCamera.RotateCamera(-1f);
                //        }

                //        //if (mainCamera.transform.rotation.eulerAngles.y > door.transform.rotation.eulerAngles.y)
                //        //{
                //        //    mainCamera.RotateCamera(-1f);
                //        //}
                //        //else
                //        //{
                //        //    mainCamera.RotateCamera(1f);
                //        //}
                //    }
                //}

                float intensity = 0f;
                while (intensity < 1)
                {
                    chrome.intensity.value = intensity;

                    intensity += 7 * Time.deltaTime;
                    yield return new WaitForSeconds(0.01f);
                }

                chrome.intensity.value = 1f;

                // Set the color
                GameObject[] colorRooms = GameObject.FindGameObjectsWithTag("ColorRoom");
                foreach (GameObject colorRoom in colorRooms)
                {
                    colorRoom.GetComponent<ColorRoom>().ChangeRoomColor(color);
                }

                while (intensity > 0)
                {
                    chrome.intensity.value = intensity;

                    intensity -= 10 * Time.deltaTime;
                    yield return new WaitForSeconds(0.01f);
                }

                chrome.intensity.value = 0f;
            }
        }        
    }

    public void ChangeRoomColor(LightColor color)
    {
        StartCoroutine(ChangeRoomColorCoroutine(color));
    }

    IEnumerator ChangeRoomColorCoroutine(LightColor color)
    {
        Volume volume = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Volume>();

        ChromaticAberration chrome;
        volume.profile.TryGet<ChromaticAberration>(out chrome);

        

        if (chrome != null)
        {
            float intensity = 0f;
            while (intensity < 1)
            {
                chrome.intensity.value = intensity;

                intensity += 7 * Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }

            chrome.intensity.value = 1f;

            // Set the color
            GameObject[] colorRooms = GameObject.FindGameObjectsWithTag("ColorRoom");
            foreach (GameObject colorRoom in colorRooms)
            {
                colorRoom.GetComponent<ColorRoom>().ChangeRoomColor(color);
            }

            while (intensity > 0)
            {
                chrome.intensity.value = intensity;

                intensity -= 10 * Time.deltaTime;
                yield return new WaitForSeconds(0.01f);
            }

            chrome.intensity.value = 0f;
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
            
            //If true, we need to stop all time in the game to fully be paused
            if (isPaused)
            {
                Debug.Log("Setting time scale to 0");
                Time.timeScale = 0.0f;
            }
            else //If false, we need to resume the timescale to it's normal values
            {
                Debug.Log("Setting time scale to 1");
                Time.timeScale = 1.0f;
            }

            UICheck.GetComponent<GameUIManager>().TriggerPause();
        }
        else
        {
            Debug.Log("GAMEMANAGER ERROR: INGAME UI NOT FOUND");
        }
    }
}
