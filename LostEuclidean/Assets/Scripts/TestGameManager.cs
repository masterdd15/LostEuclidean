using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestGameManager : MonoBehaviour
{
    [SerializeField]
    ColorRoom room;
    [SerializeField]
    float cycle = 3f;

    float time = 0f;

    void Update()
    {
        time += Time.deltaTime;
        if (time > cycle)
        {
            time -= cycle;
            room.ChangeRoomColor((LightColor)Random.Range(0, 3));
        }
    }
}
