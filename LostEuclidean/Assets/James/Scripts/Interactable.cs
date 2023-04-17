using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject controllerPrompt;
    [SerializeField] GameObject keyboardPrompt;

    [SerializeField] public bool InteractionEnabled;

    [SerializeField] public float InteractionRange;

    public bool hidden;

    [SerializeField] Collider colliderMesh;

    public virtual void Update()
    {
        GameObject contextualPrompt = null;
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (player.GetInputScheme() == "Gamepad")
        {
            contextualPrompt = controllerPrompt;
            keyboardPrompt.SetActive(false);
        }
        else
        {
            contextualPrompt = keyboardPrompt;
            controllerPrompt.SetActive(false);
        }

        if (contextualPrompt != null)
        {
            float distToPlayer = (player.transform.position - transform.position).magnitude;

            if (distToPlayer <= InteractionRange && !contextualPrompt.activeInHierarchy && InteractionEnabled && !hidden)
            {
                contextualPrompt.SetActive(true);
            }
            else if (distToPlayer >= InteractionRange && contextualPrompt.activeInHierarchy)
            {
                contextualPrompt.SetActive(false);
            }
        }

        if (colliderMesh != null)
        {
            if (colliderMesh.isTrigger)
            {
                hidden = true;
            }
            else
            {
                hidden = false;
            }
        }
    }

    /// <summary>
    /// The core function of interactable objects. Determines what happens when interacted with
    /// </summary>
    public virtual void Interact()
    {

    }

    public virtual void Enable()
    {

    }

    public virtual void Disable()
    {

    }
}
