using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Camera m_Camera;
    [SerializeField] LayerMask playerLookMask;

    [Header("Player Controller Variables")]
    [SerializeField] Rigidbody rb;
    public float speed;
    public float drag;

    // Movement variables
    bool moving;
    Vector3 moveVec;
    float ramp;
    bool camRotating;
    bool controlCamera;

    // Start is called before the first frame update
    void Start()
    {
        controlCamera = false;
        moving = false;
        ramp = 0f;
        moveVec = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        // Look at the mouse
        LookAtMouse();

        // Move the character
        if (ramp != 0)
        {
            transform.Translate(moveVec * speed * ramp * Time.deltaTime, Space.World);
            //rb.MovePosition(transform.position + (moveVec * speed * ramp * Time.deltaTime));
        }

        // Lower the speed as necessary
        if (ramp < 1f && moving)
        {
            ramp += (1 - (3 * drag));

            if (ramp > 1f)
                ramp = 1f;
        }
        if (ramp > 0 && !moving)
        {
            ramp *= (1 - drag);

            if (ramp < 0.01f)
                ramp = 0f;
        }
    }

    void LookAtMouse()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        //Vector3 worldMousPos = m_Camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        //Debug.Log(worldMousPos.x + " " + worldMousPos.y);

        RaycastHit hit;
        Vector3 worldMousePos = Vector3.zero;
        Ray mouseRay = m_Camera.ScreenPointToRay(mousePos);

        if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, playerLookMask))
        {
            worldMousePos = hit.point;
        }

        if (worldMousePos != Vector3.zero)
        {
            worldMousePos = new Vector3(worldMousePos.x, transform.position.y, worldMousePos.z);
            Vector3 targetDir = worldMousePos - transform.position;

            float rotY = Mathf.Atan2(targetDir.z, targetDir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, -rotY, 0f);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (!controlCamera) // If we're not controlling the camera, move the player
        {
            if (context.performed)
            {
                Vector2 inputMove = context.ReadValue<Vector2>();

                moveVec = new Vector3(inputMove.x, 0f, inputMove.y);
                moving = true;

                // Rotate the moveVec to correspond to the camera
                Vector3 cameraRotation = m_Camera.transform.rotation.eulerAngles;

                // Get the direction of the player's forward
                cameraRotation = new Vector3(0f, cameraRotation.y, 0f);

                moveVec = Quaternion.Euler(0f, cameraRotation.y, 0f) * moveVec;
            }
            else
            {
                moving = false;
                //moveVec = Vector3.zero;
            }
        }
        else // Move the camera
        {
            if (context.performed)
            {
                m_Camera.GetComponent<CameraController>().SetCameraMovement(context.ReadValue<Vector2>());
            }
            else
            {
                m_Camera.GetComponent<CameraController>().SetCameraMovement(Vector2.zero);
            }
        }
    }

    public void RotateCamera(InputAction.CallbackContext context)
    {
        if (context.performed && !camRotating)
        {
            if (context.ReadValue<float>() < 0)
            {
                m_Camera.gameObject.GetComponent<CameraController>().RotateCamera(1f);
            }
            else
            {
                m_Camera.gameObject.GetComponent<CameraController>().RotateCamera(-1f);
            }
        }
    }

    public void CameraControl(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            controlCamera = true;

            // Make sure we cancel all movement on the player
            moveVec = Vector3.zero;
        }
        else
        {
            controlCamera = false;

            // Make sure we cancel all camera movement
            m_Camera.GetComponent<CameraController>().SetCameraMovement(Vector2.zero);
        }
    }

    public bool PlayerMoving()
    {
        return moveVec != Vector3.zero;
    }

    public void ChangeColor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Flashlight fl = GetComponentInChildren<Flashlight>();

            if (fl != null)
            {
                fl.OnChangeColor(null);
            }
        }
    }

    public void PickupFlashlight(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            GameObject[] flashlights = GameObject.FindGameObjectsWithTag("Flashlight");

            if (flashlights.Length > 0)
            {
                Flashlight flashlight = flashlights[0].GetComponent<Flashlight>();
                float dist = (flashlight.transform.position - transform.position).magnitude;

                for (int i = 1; i < flashlights.Length; i++)
                {
                    if ((flashlights[i].transform.position - transform.position).magnitude < dist)
                    {
                        flashlight = flashlights[i].GetComponent<Flashlight>();
                        dist = (flashlights[i].transform.position - transform.position).magnitude;
                    }
                }

                flashlight.OnPickUpFlashlight(null);
            }
        }
    }
}
