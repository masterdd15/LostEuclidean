using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void ChangeScene(string sceneName, string doorName, string color)
    {
        StartCoroutine(LoadNewScene(sceneName, doorName, color));
    }

    IEnumerator LoadNewScene(string sceneName, string doorName, string color)
    {
        // Fade out


        // Load the new scene
        AsyncOperation newScene = SceneManager.LoadSceneAsync(sceneName);

        while (!newScene.isDone)
        {
            yield return null;
        }

        //// Place the player at the right door
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        //DoorController door = GameObject.Find(doorName).GetComponent<DoorController>();

        //player.transform.position = door.front.position;

        // Set the color
    }
}
