using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadInteract : Interactable
{
    public GameObject playerObj;
    public GameObject dadObj;
    [SerializeField] private float speed;
    
    public override void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        base.Start();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Interact()
    {
        Debug.Log("Has interacted!");
        playerObj.GetComponent<Player>().HandleCutscene();
        StartCoroutine(FinalCutScene());

    }

    IEnumerator FinalCutScene()
    {
        playerObj.transform.LookAt(dadObj.transform);
        playerObj.transform.Rotate(0,-90,0);

        //Now play the animation
        //playerObj.GetComponentInChildren<Animator>().SetLayerWeight(0, 1);
        //playerObj.GetComponentInChildren<Animator>().SetLayerWeight(1, 1);

        //Playing the music
        AudioManager.Instance.HandleFinaleMusic();

        yield return null;
    }
}
