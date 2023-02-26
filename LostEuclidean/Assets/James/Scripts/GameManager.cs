using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

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
        // Fade out
        Image fadingImage = GameObject.Find("FadeImage").GetComponent<FullscreenFadeController>().FadeOut();

        while (fadingImage.color.a < 1)
        {
            yield return null;
        }

        // Load the new scene
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
            colorRoom.GetComponent<ColorRoom>().roomColor = color;
        }
    }
}
