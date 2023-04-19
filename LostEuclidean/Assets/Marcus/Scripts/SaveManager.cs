using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using System.Collections;
using TMPro;

public class SaveManager : MonoBehaviour
{
    private int currentSceneIndex;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        // Get the current scene index
        currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
    }

    private void Update()
    {
        // Check if the current scene index has changed
        if (SceneManager.GetActiveScene().buildIndex != currentSceneIndex)
        {
            // Update the current scene index
            currentSceneIndex = SceneManager.GetActiveScene().buildIndex;;
        }
    }

    public void SaveGame()
    {
        // Save the current scene index
        PlayerPrefs.SetInt("CurrentSceneIndex", currentSceneIndex);

        // Save the game
        PlayerPrefs.Save();

        Debug.Log("game saved");
    }

    public void LoadGame()
    {
        // Load the current scene index
        currentSceneIndex = PlayerPrefs.GetInt("CurrentSceneIndex", 0);

        // Load the scene
        SceneManager.LoadScene(currentSceneIndex);
    }
}

