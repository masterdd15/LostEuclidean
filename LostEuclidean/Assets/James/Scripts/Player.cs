using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] Camera m_Camera;
    [SerializeField] Rigidbody rb;
    public float speed;
    public float drag;

    // Movement variables
    bool moving;
    Vector3 moveVec;
    float ramp;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        ramp = 0f;
        moveVec = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (moving)
            Debug.Log("WE'RE MOVING!!!!!!!");

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

        Vector3 worldMousPos = m_Camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, 10f));

        //Debug.Log(worldMousPos.x + " " + worldMousPos.y);

        worldMousPos = new Vector3(worldMousPos.x, transform.position.y, worldMousPos.z);
        Vector3 targetDir = worldMousPos - transform.position;

        float rotY = Mathf.Atan2(targetDir.z, targetDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, -rotY, 0f);
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 inputMove = context.ReadValue<Vector2>();

            moveVec = new Vector3(inputMove.x, 0f, inputMove.y);
            moving = true;
        }
        else
        {
            moving = false;
            //moveVec = Vector3.zero;
        }
    }

    /*public void ChangeColor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Flashlight fl = GetComponentInChildren<Flashlight>();

            if (fl != null)
            {
                fl.ChangeColorInput();
            }
        }
    }*/
}
