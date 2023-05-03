using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadInteract : Interactable
{
    public GameObject playerObj;
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
        Quaternion lookRotation = Quaternion.LookRotation(transform.position - playerObj.transform.position, playerObj.transform.up);

        // Make the player look at Dad
        playerObj.transform.LookAt(transform);
        playerObj.transform.Rotate(0,-90,0);

        //Now play the animation
        playerObj.GetComponentInChildren<Animator>().Play("DancingTest");

        yield return null;
    }
}
