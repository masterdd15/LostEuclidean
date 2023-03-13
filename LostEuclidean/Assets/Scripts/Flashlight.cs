using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum LightColor { Blue = 0, Red, Green, Off}

public class Flashlight : MonoBehaviour
{
    [SerializeField]
    LightColor[] lightModes = { LightColor.Off };

    [Space(10), Header("Dependencies"), SerializeField]
    Transform[] lightMasks = new Transform[3];
    [SerializeField]
    Light[] lights = new Light[3];
    [SerializeField]
    Transform[] volumetricMeshes = new Transform[3];
    [SerializeField]
    Transform player;
    [SerializeField] Canvas contextualPromptCanvas;
    [SerializeField] GameObject buttonText;

    private Light[] playerLights = new Light[3];
    private Transform[] playerMasks = new Transform[3];

    private LightColor currentColor = LightColor.Off;
    private int currentColorIndex = 0;
    private bool isHolding = false;
    private List<ColorObject> colorObjList = new List<ColorObject>();

    private const float PICKUP_DISTANCE = 2.0f;
    private Vector3 HOLD_OFFSET = new Vector3(0.5f, 1.0f, 0.0f);
    private Quaternion HOLD_ROTATION;

    void Awake()
    {
        if (player == null)
        {
            player = GameObject.Find("Player").transform;
        }
        Transform playerLightMask = player.Find("Light Masks");
        playerLights[0] = playerLightMask.Find("Player_Light_Blue").GetComponent<Light>();
        playerLights[1] = playerLightMask.Find("Player_Light_Red").GetComponent<Light>();
        playerLights[2] = playerLightMask.Find("Player_Light_Green").GetComponent<Light>();

        playerMasks[0] = playerLightMask.Find("Player_LightMask_Blue").transform;
        playerMasks[1] = playerLightMask.Find("Player_LightMask_Red").transform;
        playerMasks[2] = playerLightMask.Find("Player_LightMask_Green").transform;

        HOLD_ROTATION = Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
    }

    private void Update()
    {
        if (!isHolding)
        {
            if ((player.position - transform.position).magnitude <= PICKUP_DISTANCE && !contextualPromptCanvas.enabled)
            {
                contextualPromptCanvas.enabled = true;
            }
            else if ((player.position - transform.position).magnitude > PICKUP_DISTANCE && contextualPromptCanvas.enabled)
            {
                contextualPromptCanvas.enabled = false;
            }
        }
        else if (isHolding && contextualPromptCanvas.enabled)
        {
            contextualPromptCanvas.enabled = false;
        }
    }

    private void TryPickUp(Transform holder)
    {
        if (isHolding || (holder.position - transform.position).magnitude >= PICKUP_DISTANCE)
            return;
        isHolding = true;
        transform.parent = holder;
        transform.localRotation = HOLD_ROTATION;
        transform.localPosition = HOLD_OFFSET;
        UpdateLightColor();

        buttonText.SetActive(true);
    }

    private void TryDrop ()
    {
        if (!isHolding)
            return;
        isHolding = false;
        transform.parent = null;
        UpdateLightColor();

        buttonText.SetActive(false);
    }

    public void OnChangeColor(InputValue value)
    {
        if (!isHolding)
            return;
        for (int i = 0; i < colorObjList.Count; i++)
        {
            colorObjList[i].OnLightExit(currentColor);
        }
        currentColorIndex++;
        if (currentColorIndex >= lightModes.Length)
        {
            currentColorIndex = 0;
        }
        currentColor = lightModes[currentColorIndex];
        for (int i = 0; i < colorObjList.Count; i++)
        {
            colorObjList[i].OnLightEnter(currentColor);
        }
        UpdateLightColor();
    }

    public void OnPickUpFlashlight (InputValue value)
    {
        if (isHolding)
            TryDrop();
        else
            TryPickUp(player);
    }

    /*public void ChangeColorInput()
    {
        currentColor++;
        if ((int)currentColor >= 3)
            currentColor = LightColor.Blue;
        ChangeLightColor();
    }*/

    private void UpdateLightColor()
    {
        for (int i = 0; i < lightMasks.Length; i++)
        {
            if (i == (int)currentColor)
            {
                lightMasks[i].gameObject.SetActive(true);
                lights[i].gameObject.SetActive(true);
                volumetricMeshes[i].gameObject.SetActive(true);
                if (isHolding)
                {
                    playerLights[i].gameObject.SetActive(true);
                    playerMasks[i].gameObject.SetActive(true);
                }
                else
                {
                    playerLights[i].gameObject.SetActive(false);
                    playerMasks[i].gameObject.SetActive(false);
                }
            }
            else
            {
                lightMasks[i].gameObject.SetActive(false);
                lights[i].gameObject.SetActive(false);
                volumetricMeshes[i].gameObject.SetActive(false);
                playerLights[i].gameObject.SetActive(false);
                playerMasks[i].gameObject.SetActive(false);
            }
        }
    }

    private void AddColorObject (ColorObject obj)
    {
        if (!colorObjList.Contains(obj))
        {
            colorObjList.Add(obj);
            obj.OnLightEnter(currentColor);
        }
    }

    private void RemoveColorObject (ColorObject obj)
    {
        if (colorObjList.Contains(obj))
        {
            colorObjList.Remove(obj);
            obj.OnLightExit(currentColor);
        }
    }

    void OnTriggerStay(Collider other)
    {
        ColorObject obj = other.GetComponent<ColorObject>();
        if (obj != null)
        {
            AddColorObject(obj);
        }
    }

    void OnTriggerExit(Collider other)
    {
        //Debug.Log(other.gameObject.name + " flashlight exit");
        ColorObject obj = other.GetComponent<ColorObject>();
        if (obj != null)
        {
            RemoveColorObject(obj);
        }
    }

}
