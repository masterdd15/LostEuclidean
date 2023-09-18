using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPush : Interactable
{
    bool held;
    Player player;

    public override void Start()
    {
        base.Start();

        held = false;
    }

    public override void Interact()
    {
        if (!held)
        {
            held = true;
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
            colliderMesh.isTrigger = true;

            transform.SetParent(player.transform);
        }
        else
        {
            held = false;
            player = null;
            colliderMesh.isTrigger = false;

            transform.SetParent(null);
        }
    }
}
