using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public class DadInteract : Interactable
{
    public GameObject playerObj;
    public GameObject dadObj;
    [SerializeField] private float speed;
    [SerializeField]
    private AnimatorController controller;

    private Animator animator;
    

    public override void Start()
    {
        playerObj = GameObject.FindGameObjectWithTag("Player");
        animator = playerObj.GetComponentInChildren<Animator>();
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
        playerObj.transform.LookAt(gameObject.transform);
        playerObj.transform.Rotate(0,-90,0);

        //Now play the animation
        animator.runtimeAnimatorController = controller;

        //Playing the music
        AudioManager.Instance.HandleFinaleMusic();

        yield return null;
    }
}
