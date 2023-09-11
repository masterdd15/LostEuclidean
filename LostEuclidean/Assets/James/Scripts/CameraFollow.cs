using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : CameraController
{
    Player player;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
    }

    private void Update()
    {
        Vector3 newPos = player.transform.position;
        newPos.y = transform.position.y;

        transform.position = newPos;
    }

    public new void RotateCamera(float direction, bool fast)
    {
        // Do nothing
    }

    public new void Rotate180Degrees()
    {
        // Do nothing
    }
}
