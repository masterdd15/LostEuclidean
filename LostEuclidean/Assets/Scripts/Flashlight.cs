using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum LightColor { Blue = 0, Green, Red}

public class Flashlight : MonoBehaviour
{
    private KeyCode cycleKey = KeyCode.R;
    private LightColor currentColor = LightColor.Blue;
    private bool isHolding = true;

    [Space(10), Header("Dependencies"), SerializeField]
    Transform[] lightMasks = new Transform[3];
    [SerializeField]
    Light[] lights = new Light[3];

    void OnChangeColor(InputValue value)
    {
        currentColor++;
        if ((int)currentColor >= 3)
            currentColor = LightColor.Blue;
        ChangeLightColor();
    }

    public void ChangeColorInput()
    {
        currentColor++;
        if ((int)currentColor >= 3)
            currentColor = LightColor.Blue;
        ChangeLightColor();
    }

    private void ChangeLightColor()
    {
        for (int i = 0; i < lightMasks.Length; i++)
        {
            if (i == (int)currentColor)
            {
                lightMasks[i].gameObject.SetActive(true);
                lights[i].gameObject.SetActive(true);
            }
            else
            {
                lightMasks[i].gameObject.SetActive(false);
                lights[i].gameObject.SetActive(false);
            }
        }
    }
}
