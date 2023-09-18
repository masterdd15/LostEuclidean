using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : CameraController
{
    [SerializeField] Vector3 cameraOffset;
    Player player;
    Transform camFollow;

    private void Start()
    {
        player = GameObject.FindObjectOfType<Player>();
        camFollow = player.transform.Find("Camera Follow");
    }

    private void Update()
    {
        Vector3 newPos = camFollow.position + cameraOffset;
        transform.position = newPos;

        transform.LookAt(camFollow);
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
