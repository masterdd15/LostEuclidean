using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelect : MonoBehaviour
{
    GameManager gm;
    [SerializeField] string levelName;

    // Start is called before the first frame update
    private void Awake()
    {
        gm = GameManager.Instance;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenScene()
    {
        gm.HasWonOn();
        SceneManager.LoadScene(levelName);
    }
}
