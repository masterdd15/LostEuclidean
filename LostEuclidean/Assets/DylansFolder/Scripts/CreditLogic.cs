using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditLogic : MonoBehaviour
{
    private Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void finishCredits()
    {
        //Debug.Log("We have finished the credits");
        GameManager.Instance.ChangeScene("ProtoMenu", " ", LightColor.Off);
    }
}
