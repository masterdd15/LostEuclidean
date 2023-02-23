using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : Interactable
{
    [Header("Door Variables")]
    [SerializeField] public Transform front;
    [SerializeField] GameObject contextualPrompt;

    [Header("Destination Info")]
    [SerializeField] string sceneDestination;
    [SerializeField] string doorDestination;
    [SerializeField] string colorDestination;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void Update()
    {
        Player player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        float distToPlayer = (player.transform.position - transform.position).magnitude;

        if (distToPlayer <= player.InteractionRange && !contextualPrompt.activeInHierarchy)
        {
            contextualPrompt.SetActive(true);
        }
        else if (distToPlayer >= player.InteractionRange && contextualPrompt.activeInHierarchy)
        {
            contextualPrompt.SetActive(false);
        }
    }

    public override void Interact()
    {
        GameManager.Instance.ChangeScene(sceneDestination, doorDestination, colorDestination);
    }
}
