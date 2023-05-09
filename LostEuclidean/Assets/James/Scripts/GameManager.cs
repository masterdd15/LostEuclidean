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
    [SerializeField] float bloomMultiplier;

    bool changingScene;

    private int currentSceneIndex;

    public float thresholdValueForBloom = 0.03f;


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

    private void Start()
    {
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        changingScene = false;
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex != currentSceneIndex)
        {
            // Update the current scene index
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex; ;
        }
    }

    public void ChangeScene(string sceneName, string doorName, LightColor color)
    {
        StartCoroutine(LoadNewScene(sceneName, doorName, color));
    }

    public void LoadGame()
    {
        // Load the current scene index
        currentSceneIndex = PlayerPrefs.GetInt("CurrentSceneIndex", 0);

        // Load the scene
        SceneManager.LoadScene(currentSceneIndex);
    }

    IEnumerator LoadNewScene(string sceneName, string doorName, LightColor color)
    {
        Debug.Log(sceneName + " " + changingScene);
        if (!changingScene)
        {
            changingScene = true;
            if (sceneName != SceneManager.GetActiveScene().name)
            {
                // Fade out
                if (GameObject.Find("FadeImage") != null)
                {
                    Image fadingImage = GameObject.Find("FadeImage").GetComponent<FullscreenFadeController>().FadeOut();

                    while (fadingImage.color.a < 1)
                    {
                        yield return null;
                    }
                }


                //if(AudioManager.Instance.currentDimension == LightColor.Off)
                //{
                //    AudioManager.Instance.HandleCurrentDimension(LightColor.Off);
                //}
                //else
                //{
                //    AudioManager.Instance.HandleCurrentDimension(color);
                //} 
                // Load the new scene if necessary
                AsyncOperation newScene = SceneManager.LoadSceneAsync(sceneName);

                while (!newScene.isDone)
                {
                    yield return null;
                }

                AudioManager.Instance.HandleCurrentDimension(color);

                if (sceneName != "ProtoMenu")
                {
                    // Save the current scene index
                    PlayerPrefs.SetInt("CurrentSceneIndex", currentSceneIndex);

                    // Save the game
                    PlayerPrefs.Save();

                    //notify that the game has indeed saved
                    Debug.Log("game saved");

                    // Place the player at the right door
                    GameObject player = GameObject.FindGameObjectWithTag("Player");
                    GameObject doorObj = GameObject.Find(doorName);
                    if (doorObj != null)
                    {
                        DoorController door = doorObj.GetComponent<DoorController>();
                        player.transform.position = door.front.position;
                    }
                }

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

                //bloom
                Bloom bloomLayer = null;
                volume.profile.TryGet(out bloomLayer);

                volume.profile.TryGet<ChromaticAberration>(out chrome);

                Color startTint = new Color(255f, 68f, 68f);

                if (color == LightColor.Green)
                {
                    startTint = new Color(62f, 255f, 77f);
                }

                Color endTint = new Color(255f, 255f, 255f);
                float startThreshold = thresholdValueForBloom;
                float endThreshold = 0f;

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
                            mainCamera.RotateCamera(1f, false);
                        }
                        else
                        {
                            mainCamera.RotateCamera(-1f, false);
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

                    ColorRoom mainColorRoom = FindObjectOfType<ColorRoom>();
                    if (color != mainColorRoom.roomColor)
                    {
                        float intensity = 0f;

                        while (intensity < 1)
                        {

                            // Interpolate the tint value from startTint to endTint
                            Color currentTint = Color.Lerp(endTint, startTint, intensity);
                            chrome.intensity.value = intensity;
                            bloomLayer.intensity.value = intensity * bloomMultiplier;
                            bloomLayer.tint.value = currentTint;

                            //// Interpolate the threshold value from startThreshold to endThreshold
                            //float currentThreshold = Mathf.Lerp(startThreshold, endThreshold, intensity);
                            //if (bloomLayer != null)
                            //{
                            //    bloomLayer.threshold.value = currentThreshold;
                            //}

                            intensity += 3 * Time.deltaTime;
                            yield return new WaitForSeconds(0.01f);
                        }

                        bloomLayer.tint.value = startTint;
                        bloomLayer.intensity.value = 1f;
                        chrome.intensity.value = 1f;

                        // Set the color
                        GameObject[] colorRooms = GameObject.FindGameObjectsWithTag("ColorRoom");
                        foreach (GameObject colorRoom in colorRooms)
                        {
                            colorRoom.GetComponent<ColorRoom>().ChangeRoomColor(color);
                        }

                        while (intensity > 0)
                        {
                            // Interpolate the tint value from startTint to endTint
                            Color currentTint = Color.Lerp(startTint, endTint, intensity);
                            chrome.intensity.value = intensity;
                            bloomLayer.intensity.value = intensity * bloomMultiplier;
                            bloomLayer.tint.value = currentTint;

                            //chrome.intensity.value = intensity;
                            //bloomLayer.threshold.value = 0.7f;
                            //bloomLayer.tint.value = new Color(255f / 255f, 255f / 255f, 255f / 255f);

                            intensity -= 6 * Time.deltaTime;
                            yield return new WaitForSeconds(0.01f);
                        }

                        bloomLayer.tint.value = endTint;
                        bloomLayer.intensity.value = 0f;
                        chrome.intensity.value = 0f;
                    }
                }
            }
            changingScene = false;
        }
    }

    public void ResetRoom()
    {
        StartCoroutine(ResetRoomRoutine());
    }

    IEnumerator ResetRoomRoutine()
    {
        // Fade out
        if (GameObject.Find("FadeImage") != null)
        {
            Image fadingImage = GameObject.Find("FadeImage").GetComponent<FullscreenFadeController>().FadeOut();

            while (fadingImage.color.a < 1)
            {
                yield return null;
            }
        }

        // Reload the scene
        AsyncOperation newScene = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

        while (!newScene.isDone)
        {
            yield return null;
        }

        LightColor color = FindObjectOfType<ColorRoom>().roomColor;

        AudioManager.Instance.HandleCurrentDimension(color);

        // Save the current scene index
        PlayerPrefs.SetInt("CurrentSceneIndex", currentSceneIndex);

        // Save the game
        PlayerPrefs.Save();

        //notify that the game has indeed saved
        Debug.Log("game saved");

        // Place the player at the right door
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        DoorController door = GameObject.Find("Entrance").GetComponent<DoorController>();

        if (door != null)
            player.transform.position = door.front.position;

        // Set the color
        GameObject[] colorRooms = GameObject.FindGameObjectsWithTag("ColorRoom");
        foreach (GameObject colorRoom in colorRooms)
        {
            colorRoom.GetComponent<ColorRoom>().ChangeRoomColor(color);
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
                AudioManager.Instance.HandlePauseMusicIn();
                Debug.Log("Setting time scale to 0");
                Time.timeScale = 0.0f;
            }
            else //If false, we need to resume the timescale to it's normal values
            {
                AudioManager.Instance.HandlePauseMusicOut();
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
