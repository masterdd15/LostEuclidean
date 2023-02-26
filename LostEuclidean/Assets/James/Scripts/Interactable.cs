using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject contextualPrompt;

    [SerializeField] public bool InteractionEnabled;

    private void Update()
    {
        if (contextualPrompt != null)
        {
            Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

            float distToPlayer = (player.transform.position - transform.position).magnitude;

            if (distToPlayer <= player.InteractionRange && !contextualPrompt.activeInHierarchy && InteractionEnabled)
            {
                contextualPrompt.SetActive(true);
            }
            else if (distToPlayer >= player.InteractionRange && contextualPrompt.activeInHierarchy)
            {
                contextualPrompt.SetActive(false);
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
