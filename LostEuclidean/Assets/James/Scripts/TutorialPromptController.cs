using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TutorialPromptController : MonoBehaviour
{
    [Header("Tutorial Prompts")]
    [SerializeField] string movePrompt;
    [SerializeField] string cameraPrompt;
    [SerializeField] string cameraMovePrompt;

    [Header("UI Elements")]
    [SerializeField] TMP_Text tutorialText;
    [SerializeField] CameraController cam;
    [SerializeField] Player player;

    bool swappingPrompts;

    // Start is called before the first frame update
    void Start()
    {
        tutorialText.text = movePrompt;

        swappingPrompts = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!swappingPrompts)
        {
            if (tutorialText.text == movePrompt && player.PlayerMoving())
            {
                StartCoroutine(AdvanceTutorial());
            }
            else if (tutorialText.text == cameraPrompt && cam.CameraRotating())
            {
                StartCoroutine(AdvanceTutorial());
            }
            else if (tutorialText.text == cameraMovePrompt && cam.CameraMoving())
            {
                StartCoroutine(AdvanceTutorial());
            }
        }
    }

    IEnumerator AdvanceTutorial()
    {
        swappingPrompts = true;

        yield return new WaitForSeconds(1.5f);

        if (tutorialText.text == movePrompt)
        {
            tutorialText.text = cameraPrompt;
        }
        else if (tutorialText.text == cameraPrompt)
        {
            tutorialText.text = cameraMovePrompt;
        }
        else if (tutorialText.text == cameraMovePrompt)
        {
            Destroy(gameObject);
        }

        swappingPrompts = false;
    }
}
