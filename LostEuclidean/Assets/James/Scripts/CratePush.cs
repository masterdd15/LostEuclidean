using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CratePush : MonoBehaviour
{
    [SerializeField] SimpleColorCube colorCube;
    [SerializeField] float grabDistance = 2f;

    private bool isGrabbed = false;
    private Transform playerTransform;
    private Rigidbody playerRigidbody;
    private FixedJoint fixedJoint;
    private Collider crateCollider;

    private void Start()
    {
        // Get the collider component of the crate
        crateCollider = GetComponent<Collider>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && colorCube.CanInteract())
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
            {
                if (isGrabbed)
                {
                    Debug.Log("RELEASING");

                    // Release the crate from the player
                    isGrabbed = false;
                    playerTransform = null;
                    playerRigidbody = null;

                    // Destroy the fixed joint and reset the parent
                     Destroy(fixedJoint);
                  
                    transform.SetParent(null);

                    // Enable the crate's rigidbody 
                    GetComponent<Rigidbody>().isKinematic = false;
                }

                else
                {
                    // Get the player's transform and rigidbody
                    playerTransform = collision.gameObject.transform;
                    playerRigidbody = playerTransform.gameObject.GetComponent<Rigidbody>();

                    // Calculate the distance between the player and the crate
                    float distance = Vector3.Distance(playerTransform.position, transform.position);


                    // Check if the player is within grabbing distance
                    if (distance <= grabDistance)
                    {
                        // Attach the crate to the player using a fixed joint
                        fixedJoint = gameObject.AddComponent<FixedJoint>();
                        fixedJoint.connectedBody = playerRigidbody;
                        fixedJoint.breakForce = Mathf.Infinity;

                        // Make the crate a child of the player
                        transform.parent = playerTransform;

                        // Disable the crate's rigidbody so it follows the player without physics simulation
                        GetComponent<Rigidbody>().isKinematic = true;

                        isGrabbed = true;
                    }
                }
            }
        }
    }

}
