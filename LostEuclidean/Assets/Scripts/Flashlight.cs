using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum LightColor { Blue = 0, Green, Red}

public class Flashlight : MonoBehaviour
{
    private LightColor currentColor = LightColor.Blue;
    private bool isHolding = true;

    [Space(10), Header("Dependencies"), SerializeField]
    Transform[] lightMasks = new Transform[3];
    [SerializeField]
    Light[] lights = new Light[3];

    private const float PICKUP_DISTANCE = 2.0f;
    private Vector3 HOLD_OFFSET = new Vector3(0.5f, 0.5f, 0.5f);

    private void TryPickUp(Transform holder)
    {
        if (isHolding || (holder.position - transform.position).magnitude >= PICKUP_DISTANCE)
            return;
        isHolding = true;
        transform.parent = holder;
        transform.localRotation = Quaternion.identity;
        transform.localPosition = HOLD_OFFSET;
    }

    private void TryDrop ()
    {
        if (!isHolding)
            return;
        isHolding = false;
        transform.parent = null;
    }

    public void OnChangeColor(InputValue value)
    {
        if (!isHolding)
            return;
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
