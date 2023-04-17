using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlashlightUIManager : MonoBehaviour
{
    [Header("Wheel Objects")]
    [SerializeField] GameObject SingleColor;
    [SerializeField] GameObject TwoColors;
    [SerializeField] GameObject ThreeColors;
    //[SerializeField] GameObject ButtonPrompts;
    [SerializeField] GameObject LT;
    [SerializeField] GameObject RT;
    [SerializeField] GameObject MBL;
    [SerializeField] GameObject MBR;

    [Header("Colors")]
    [SerializeField] Color[] Greens;
    [SerializeField] Color[] Reds;
    [SerializeField] Color[] Blues;

    List<LightColor> availableColors;
    GameObject activeWheel;
    Image[] wheelImages;

    public void SetupLightWheel(LightColor[] lightModes, int currentIndex)
    {
        availableColors = new List<LightColor>();

        foreach (LightColor color in lightModes)
        {
            if (color != LightColor.Off)
            {
                availableColors.Add(color);
            }
        }

        Player player = GameObject.FindObjectOfType<Player>();
        if (availableColors.Count == 1)
        {
            if (availableColors[0] == LightColor.Green)
            {
                if (player != null && player.GetInputScheme() == "Gamepad")
                {
                    LT.SetActive(true);
                }
                else
                {
                    MBL.SetActive(true);
                }
            }
            else
            {
                if (player != null && player.GetInputScheme() == "Gamepad")
                {
                    RT.SetActive(true);
                }
                else
                {
                    MBR.SetActive(true);
                }
            }

            activeWheel = SingleColor;
            SingleColor.SetActive(true);
        }
        else if (availableColors.Count == 2)
        {
            if (player != null && player.GetInputScheme() == "Gamepad")
            {
                LT.SetActive(true);
                RT.SetActive(true);
            }
            else
            {
                MBL.SetActive(true);
                MBR.SetActive(true);
            }

            activeWheel = TwoColors;
            TwoColors.SetActive(true);
        }
        else if (availableColors.Count == 3)
        {
            activeWheel = ThreeColors;
            ThreeColors.SetActive(true);
        }

        wheelImages = activeWheel.transform.GetComponentsInChildren<Image>();
        for (int i = 0; i < wheelImages.Length; i++)
        {
            if (availableColors[i] == LightColor.Green)
            {
                wheelImages[i].color = Greens[0];
            }
            else if (availableColors[i] == LightColor.Red)
            {
                wheelImages[i].color = Reds[0];
            }
            else if (availableColors[i] == LightColor.Blue)
            {
                wheelImages[i].color = Blues[0];
            }
        }

        if (lightModes[currentIndex] != LightColor.Off)
        {
            for (int i = 0; i < wheelImages.Length; i++)
            {
                if (availableColors[i] == lightModes[currentIndex])
                {
                    if (lightModes[currentIndex] == LightColor.Green)
                    {
                        wheelImages[i].color = Greens[1];
                    }
                    else if (lightModes[currentIndex] == LightColor.Red)
                    {
                        wheelImages[i].color = Reds[1];
                    }
                    else if (lightModes[currentIndex] == LightColor.Blue)
                    {
                        wheelImages[i].color = Blues[1];
                    }
                }
            }
        }
    }

    public void UpdateColorWheel(LightColor newColor)
    {
        for (int i = 0; i < wheelImages.Length; i++)
        {
            if (availableColors[i] == newColor)
            {
                if (newColor == LightColor.Green)
                {
                    wheelImages[i].color = Greens[1];
                }
                else if (newColor == LightColor.Red)
                {
                    wheelImages[i].color = Reds[1];
                }
                else if (newColor == LightColor.Blue)
                {
                    wheelImages[i].color = Blues[1];
                }
            }
            else
            {
                if (availableColors[i] == LightColor.Green)
                {
                    wheelImages[i].color = Greens[0];
                }
                else if (availableColors[i] == LightColor.Red)
                {
                    wheelImages[i].color = Reds[0];
                }
                else if (availableColors[i] == LightColor.Blue)
                {
                    wheelImages[i].color = Blues[0];
                }
            }
        }
    }

    public void DisableFlashlightUI()
    {
        //ButtonPrompts.SetActive(false);
        LT.SetActive(false);
        RT.SetActive(false);
        MBL.SetActive(false);
        MBR.SetActive(false);
        SingleColor.SetActive(false);
        TwoColors.SetActive(false);
        ThreeColors.SetActive(false);
    }
}
