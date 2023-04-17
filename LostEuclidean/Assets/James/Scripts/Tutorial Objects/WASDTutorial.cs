using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WASDTutorial : MonoBehaviour
{
    [SerializeField] string KeyboardText;
    [SerializeField] string GamepadText;
    [SerializeField] TMP_Text UI_Text;
    [SerializeField] float WaitTime;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowTutorialText());
    }

    IEnumerator ShowTutorialText()
    {
        yield return new WaitForSeconds(1f);

        Player player = GameObject.FindObjectOfType<Player>();

        if (player.GetInputScheme() == "Gamepad")
        {
            UI_Text.text = GamepadText;
        }
        else
        {
            UI_Text.text = KeyboardText;
        }

        UI_Text.enabled = true;

        yield return new WaitForSeconds(WaitTime);

        Destroy(gameObject);
    }
}
