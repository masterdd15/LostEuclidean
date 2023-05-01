using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] GameManager gm;

    public float InteractionRange;
    
    [SerializeField] Camera m_Camera;
    [SerializeField] LayerMask playerLookMask;
    [SerializeField] PlayerInput playerInput;
    [SerializeField] Animator playerAnimator;

    [Header("Player Controller Variables")]
    [SerializeField] Rigidbody rb;
    public float speed;
    public float drag;

    Flashlight flashlight;
    // Movement variables
    bool moving;
    Vector3 moveVec;
    Vector2 mouseVec;
    float ramp;
    bool controlCamera;
    public bool IsHolding;
    bool againstWall;

    // Start is called before the first frame update
    void Start()
    {
        controlCamera = false;
        moving = false;
        playerAnimator.SetBool("IsMoving", moving);
        ramp = 0f;
        moveVec = Vector3.zero;
        IsHolding = false;
        againstWall = false;

        gm = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateAnimation();
        if (playerInput.currentControlScheme == "Keyboard&Mouse" && playerInput.currentActionMap == playerInput.actions.FindActionMap("Player") && !IsHolding)
        {
            // Look at the mouse
            LookAtMouse();
        }

        // Move the character
        if (ramp != 0)
        {
            float wallSlowdown = 1f;
            if (againstWall)
                wallSlowdown = 0.5f;

            transform.Translate(moveVec * speed * ramp * Time.deltaTime * wallSlowdown, Space.World);
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

    private void OnCollisionEnter(Collision collision)
    {
        WallpaperColorObject wallpaper = collision.gameObject.GetComponent<WallpaperColorObject>();
        if (wallpaper != null)
        {
            againstWall = true;
        }

        //if (collision.gameObject.layer == 10 || collision.gameObject.layer == 12)
        //{
        //    againstWall = true;
        //}
    }

    private void OnCollisionExit(Collision collision)
    {
        WallpaperColorObject wallpaper = collision.gameObject.GetComponent<WallpaperColorObject>();
        if (wallpaper != null)
        {
            againstWall = false;
        }

        //if (collision.gameObject.layer == 10 || collision.gameObject.layer == 12)
        //{
        //    againstWall = false;
        //}
    }

    void UpdateAnimation()
    {
        if (moveVec.magnitude < 0.01f)
            moving = false;
        else
            moving = true;
        playerAnimator.SetBool("IsMoving", moving);
        flashlight = GetComponentInChildren<Flashlight>();
        if (flashlight != null)
            playerAnimator.SetBool("IsHolding", true);
        else
            playerAnimator.SetBool("IsHolding", false);
        float dir = Vector3.Dot(moveVec, transform.right);
        if (dir > 0.5f)
            playerAnimator.SetInteger("MoveDirVert", 1);
        else if (dir < -0.5f)
        {
            playerAnimator.SetInteger("MoveDirVert", -1);
        }
        else
            playerAnimator.SetInteger("MoveDirVert", 0);

        dir = Vector3.Dot(moveVec, -transform.forward);
        if (dir > 0.5f)
            playerAnimator.SetInteger("MoveDirHor", 1);
        else if (dir < -0.5f)
            playerAnimator.SetInteger("MoveDirHor", -1);
        else
            playerAnimator.SetInteger("MoveDirHor", 0);
    }

    void LookAtMouse()
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        //Vector3 worldMousePos = m_Camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 25f));

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
            mouseVec = Vector3.Normalize(targetDir);

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
                //Debug.Log("Set value to true");

                // Rotate the moveVec to correspond to the camera
                Vector3 cameraRotation = m_Camera.transform.rotation.eulerAngles;

                // Get the direction of the player's forward
                cameraRotation = new Vector3(0f, cameraRotation.y, 0f);

                moveVec = Quaternion.Euler(0f, cameraRotation.y, 0f) * moveVec;
            }
            else
            {
                moving = false;
                moveVec = Vector3.zero;
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
        if (context.performed && !m_Camera.gameObject.GetComponent<CameraController>().CameraRotating())
        {
            if (context.ReadValue<float>() < 0)
            {
                m_Camera.gameObject.GetComponent<CameraController>().RotateCamera(1f, true);
            }
            else
            {
                m_Camera.gameObject.GetComponent<CameraController>().RotateCamera(-1f, true);
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
                if (context.ReadValue<float>() < 0)
                {
                    fl.SelectColor(LightColor.Green);

                    //fl.OnChangeColor(-1);
                }
                else
                {
                    fl.SelectColor(LightColor.Red);

                    //fl.OnChangeColor(1);
                }
            }
        }
    }

    public void FlashlightOnOff (InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Flashlight fl = GetComponentInChildren<Flashlight>();

            if (fl != null)
            {
                fl.FlashlightOnOff();
            }
        }
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            // Find the nearest interactable
            GameObject[] interactables = GameObject.FindGameObjectsWithTag("Interactable");
            GameObject closest = null;
            float minDist = float.MaxValue;
            if (interactables.Length > 0)
            {
                //closest = interactables[0];
                //minDist = (closest.transform.position - transform.position).magnitude;
                for (int i = 0; i < interactables.Length; i++)
                {
                    ColorObject colorObj = interactables[i].GetComponent<ColorObject>();
                    if (colorObj == null)
                        colorObj = interactables[i].GetComponentInChildren<ColorObject>();

                    Interactable interactable = interactables[i].GetComponent<Interactable>();
                    float newDist = (interactable.InteractionObject.transform.position - transform.position).magnitude;
                    if (newDist < minDist && colorObj != null && colorObj.CanInteract() && interactable.InteractionEnabled)
                    {
                        //Debug.Log(interactables[i].name);

                        minDist = newDist;
                        closest = interactables[i];
                    }
                }
            }

            // If a door is the closest and we're not holding a cube then just go through the door
            DoorController door = null;
            if (closest != null)
            {
                door = closest.GetComponent<DoorController>();
            }

            if (door != null && minDist <= door.InteractionRange && !IsHolding)
            {
                door.Interact();
            }
            else
            {
                // Get the flashlight
                GameObject[] flashlights = GameObject.FindGameObjectsWithTag("Flashlight");
                Flashlight fl = null;
                float dist = float.MaxValue;
                if (flashlights.Length > 0)
                {
                    fl = flashlights[0].GetComponent<Flashlight>();
                    dist = (fl.transform.position - transform.position).magnitude;

                    for (int i = 1; i < flashlights.Length; i++)
                    {
                        if ((flashlights[i].transform.position - transform.position).magnitude < dist)
                        {
                            fl = flashlights[i].GetComponent<Flashlight>();
                            dist = (flashlights[i].transform.position - transform.position).magnitude;
                        }
                    }
                }

                if (closest != null && fl != null)
                {
                    if (minDist < dist)
                    {
                        // If the closest interactable is close enough to be interacted with
                        Interactable interactable = closest.GetComponent<Interactable>();
                        if (door == null && flashlight == null && minDist <= interactable.InteractionRange)
                        {
                            interactable.Interact();
                        }
                    }
                    else
                    {
                        if (!IsHolding)
                            fl.OnPickUpFlashlight(null);
                    }
                }
                else if (closest != null && fl == null)
                {
                    // If the closest interactable is close enough to be interacted with
                    Interactable interactable = closest.GetComponent<Interactable>();
                    if (door == null && flashlight == null && minDist <= interactable.InteractionRange)
                    {
                        interactable.Interact();
                    }
                }
                else if (closest == null && fl != null)
                {
                    if (!IsHolding)
                        fl.OnPickUpFlashlight(null);
                }
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

    public void GamepadLook(InputAction.CallbackContext context)
    {
        if (context.performed && context.ReadValue<Vector2>() != Vector2.zero && !IsHolding)
        {
            Vector2 inputLook = context.ReadValue<Vector2>();

            Vector3 lookVec = new Vector3(inputLook.x, 0f, inputLook.y);

            // Rotate the moveVec to correspond to the camera
            Vector3 cameraRotation = m_Camera.transform.rotation.eulerAngles;

            // Get the direction of the player's forward
            cameraRotation = new Vector3(0f, cameraRotation.y, 0f);

            lookVec = Quaternion.Euler(0f, cameraRotation.y - 90f, 0f) * lookVec;

            transform.rotation = Quaternion.LookRotation(lookVec);
        }
    }

    /*This script sends a single that the Pause Menu should be opened or closed
     * Currently, it links directly to the GameManager, where it is checked if the Pause menu can be opened
     */
    public void TogglePause(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            if (playerInput.currentActionMap == playerInput.actions.FindActionMap("Player"))
            {
                playerInput.SwitchCurrentActionMap("Menu");

                //Debug.Log("We did it Joe");
                gm.HandlePause();
            }
            else if (playerInput.currentActionMap == playerInput.actions.FindActionMap("Menu"))
            {
                playerInput.SwitchCurrentActionMap("Player");

                //Debug.Log("We did it Joe");
                gm.HandlePause();
            }
        }

    }

    public string GetInputScheme()
    {
        return playerInput.currentControlScheme;
    }
}
