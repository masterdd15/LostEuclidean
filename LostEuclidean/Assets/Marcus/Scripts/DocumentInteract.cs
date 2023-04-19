using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DocumentInteract : Interactable
{
    [TextArea(3, 10)]
    public string textInput;

    GameUIManager gm; 

    private void Start()
    {
        gm = FindObjectOfType<GameUIManager>();
    }

    public override void Interact()
    {
        {
            gm.DocumentInteract();

            GameObject textToShowObject = GameObject.Find("TextToShow");
            if (textToShowObject != null)
            {
                TextMeshProUGUI textMesh = textToShowObject.GetComponent<TextMeshProUGUI>();
                if (textMesh != null)
                {
                    textMesh.text = textInput;
                }
                else
                {
                    Debug.LogError("TextMeshPro component not found on TextToShow game object!");
                }
            }
            else
            {
                Debug.LogError("TextToShow game object not found!");
            }
        }
    }
}
