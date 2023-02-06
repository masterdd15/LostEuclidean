using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] LayerMask cameraRotateMask;
    [SerializeField] LayerMask playerLookMask;
    [SerializeField] Camera m_Camera;
    [SerializeField] Rigidbody rb;
    public float speed;
    public float drag;

    // Movement variables
    bool moving;
    Vector3 moveVec;
    float ramp;
    bool camRotating;

    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        ramp = 0f;
        moveVec = Vector3.zero;
        camRotating = false;
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

        if (Physics.Raycast(mouseRay, out hit))
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

    public void RotateCamera(InputAction.CallbackContext context)
    {
        if (context.performed && !camRotating)
        {
            if (context.ReadValue<float>() < 0)
            {
                StartCoroutine(CameraRotatingCoroutine(1f));
            }
            else
            {
                StartCoroutine(CameraRotatingCoroutine(-1f));
            }
        }
    }

    IEnumerator CameraRotatingCoroutine(float dir)
    {
        camRotating = true;

        Vector2 center = new Vector2(m_Camera.scaledPixelWidth / 2, m_Camera.scaledPixelHeight / 2);
        RaycastHit hit;
        Vector3 rotatePoint = Vector3.zero;
        Ray mouseRay = m_Camera.ScreenPointToRay(center);

        if (Physics.Raycast(mouseRay, out hit, Mathf.Infinity, cameraRotateMask))
        {
            rotatePoint = hit.point;
        }

        float deg = 0f;
        while (deg < 90f)
        {
            if (rotatePoint != Vector3.zero)
            {
                m_Camera.transform.RotateAround(rotatePoint, Vector3.up, 2 * dir);
            }

            deg += 2;

            yield return new WaitForSeconds(0.01f);
        }

        camRotating = false;
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
