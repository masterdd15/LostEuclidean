using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed;

    Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        moveVec = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        if (moveVec != Vector3.zero)
        {
            transform.Translate(moveVec * speed * Time.deltaTime);
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Vector2 inputMove = context.ReadValue<Vector2>();

            moveVec = new Vector3(inputMove.x, 0f, inputMove.y);
        }
        else
        {
            moveVec = Vector3.zero;
        }
    }
}
