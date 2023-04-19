using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlashlightTutorial : MonoBehaviour
{
    [SerializeField] string[] KeyboardText;
    [SerializeField] string[] GamepadText;
    [SerializeField] TMP_Text UI_Text;
    [SerializeField] float WaitTime;

    Flashlight flashlight;
    bool displayed;

    // Start is called before the first frame update
    void Start()
    {
        flashlight = GameObject.FindObjectOfType<Flashlight>();
        displayed = false;
        //StartCoroutine(ShowTutorialText());
    }

    private void Update()
    {
        if (!displayed)
        {
            if (flashlight != null)
            {
                if (flashlight.IsHeld())
                {
                    StartCoroutine(ShowTutorialText());
                    displayed = true;
                }
            }
        }
    }

    IEnumerator ShowTutorialText()
    {
        int activeText = 0;

        yield return new WaitForSeconds(1f);

        Player player = GameObject.FindObjectOfType<Player>();

        UI_Text.enabled = true;

        while (activeText < KeyboardText.Length && activeText < GamepadText.Length)
        {
            if (player.GetInputScheme() == "Gamepad")
            {
                UI_Text.text = GamepadText[activeText];
            }
            else
            {
                UI_Text.text = KeyboardText[activeText];
            }

            yield return new WaitForSeconds(WaitTime);

            activeText++;
        }

        Destroy(gameObject);
    }
}
