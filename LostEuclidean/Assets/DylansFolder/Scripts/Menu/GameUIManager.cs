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
    
}
