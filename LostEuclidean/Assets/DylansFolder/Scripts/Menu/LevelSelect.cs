using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    GameManager gm;
    [SerializeField] string levelName;
    [SerializeField] LightColor levelColor;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    void Start()
    {
        gm = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenScene()
    {
        //gm.HasWonOn();
        //SceneManager.LoadScene(levelName);
        GameManager.Instance.ChangeScene(levelName, "Entrance", levelColor);
    }
}
