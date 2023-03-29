using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera and Layer Variables")]
    [SerializeField] LayerMask cameraRotateMask;
    [SerializeField] int wallLayer;
    [SerializeField] int wallHiddenLayer;
    [SerializeField] Camera m_Camera;
    [SerializeField] int startingLeftWall;
    [SerializeField] GameObject camTarget;

    [Header("Other Variables")]
    [SerializeField] GameObject wallParent;
    [SerializeField] float cameraSpeed;

    bool camRotating;
    int leftWall;
    Vector3 moveVec;

    // Start is called before the first frame update
    void Start()
    {
        leftWall = startingLeftWall;

        camRotating = false;

        moveVec = Vector3.zero;

        //gameObject.transform.LookAt(camParent.transform);
    }

    private void Update()
    {
        // Move the camera if necessary
        if (moveVec != Vector3.zero && !camRotating)
        {
            transform.Translate(moveVec * Time.deltaTime * cameraSpeed, Space.World);
        }
    }

    public void SetCameraMovement(Vector2 inputVec)
    {
        if (inputVec == Vector2.zero)
        {
            moveVec = Vector3.zero;
        }
        else
        {
            // Set the initial movement vector
            moveVec = new Vector3(inputVec.x, inputVec.y, 0f);

            // Now we need to rotate the moveVec to the camera's rotation because we want to be moving relative to the camera's rotation in world space
            Vector3 cameraRotation = m_Camera.transform.rotation.eulerAngles;

            // Get the direction of the camera's forward
            cameraRotation = new Vector3(0f, cameraRotation.y, 0f);

            moveVec = Quaternion.Euler(0f, cameraRotation.y, 0f) * moveVec;
        }
    }

    public void RotateCamera(float direction)
    {
        StartCoroutine(CameraRotatingCoroutine(direction));
    }

    public void Rotate180Degrees()
    {
        StartCoroutine(Rotate180Coroutine());
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

        if (camTarget != null)
        {
            float deg = 0f;
            bool revealed = false;
            while (deg < 90f)
            {
                m_Camera.transform.RotateAround(camTarget.transform.position, Vector3.up, 2 * dir);

                // Reveal and hide walls as necessary
                if (deg > 45f && !revealed)
                {
                    if (dir > 0)
                    {
                        leftWall--;

                        if (leftWall < 0)
                            leftWall = wallParent.transform.childCount - 1;
                    }
                    else
                    {
                        leftWall++;

                        if (leftWall > wallParent.transform.childCount - 1)
                            leftWall = 0;
                    }

                    int rightWall = leftWall + 1;
                    if (rightWall > wallParent.transform.childCount - 1)
                        rightWall = 0;

                    int i = 0;

                    foreach (Transform wall in wallParent.transform)
                    {
                        if (i == leftWall || i == rightWall)
                        {
                            wall.gameObject.layer = wallHiddenLayer;

                            Transform[] allChildren = wall.GetComponentsInChildren<Transform>();
                            foreach (Transform child in allChildren)
                            {
                                child.gameObject.layer = wallHiddenLayer;
                            }
                        }
                        else
                        {
                            wall.gameObject.layer = wallLayer;

                            Transform[] allChildren = wall.GetComponentsInChildren<Transform>();
                            foreach (Transform child in allChildren)
                            {
                                child.gameObject.layer = wallLayer;
                            }
                        }

                        i++;
                    }

                    revealed = true;
                }

                deg += 2;

                yield return new WaitForSeconds(0.01f);
            }
        }

        camRotating = false;
    }

    IEnumerator Rotate180Coroutine()
    {
        camRotating = true;

        if (camTarget != null)
        {
            // Rotate the camera the first 90 degrees
            float deg = 0f;
            bool revealedOne = false;
            bool revealedTwo = false;
            while (deg < 180f)
            {
                m_Camera.transform.RotateAround(camTarget.transform.position, Vector3.up, -4);

                // Reveal and hide walls as necessary
                if (deg > 45f && !revealedOne)
                {
                    leftWall++;

                    if (leftWall > wallParent.transform.childCount - 1)
                        leftWall = 0;

                    int rightWall = leftWall + 1;
                    if (rightWall > wallParent.transform.childCount - 1)
                        rightWall = 0;

                    int i = 0;
                    foreach (Transform wall in wallParent.transform)
                    {
                        if (i == leftWall || i == rightWall)
                        {
                            wall.gameObject.layer = wallHiddenLayer;

                            Transform[] allChildren = wall.GetComponentsInChildren<Transform>();
                            foreach (Transform child in allChildren)
                            {
                                child.gameObject.layer = wallHiddenLayer;
                            }
                        }
                        else
                        {
                            wall.gameObject.layer = wallLayer;

                            Transform[] allChildren = wall.GetComponentsInChildren<Transform>();
                            foreach (Transform child in allChildren)
                            {
                                child.gameObject.layer = wallLayer;
                            }
                        }

                        i++;
                    }
                    revealedOne = true;
                }
                else if (deg > 135f && !revealedTwo)
                {
                    leftWall++;

                    if (leftWall > wallParent.transform.childCount - 1)
                        leftWall = 0;

                    int rightWall = leftWall + 1;
                    if (rightWall > wallParent.transform.childCount - 1)
                        rightWall = 0;

                    int i = 0;
                    foreach (Transform wall in wallParent.transform)
                    {
                        if (i == leftWall || i == rightWall)
                        {
                            wall.gameObject.layer = wallHiddenLayer;

                            Transform[] allChildren = wall.GetComponentsInChildren<Transform>();
                            foreach (Transform child in allChildren)
                            {
                                child.gameObject.layer = wallHiddenLayer;
                            }
                        }
                        else
                        {
                            wall.gameObject.layer = wallLayer;

                            Transform[] allChildren = wall.GetComponentsInChildren<Transform>();
                            foreach (Transform child in allChildren)
                            {
                                child.gameObject.layer = wallLayer;
                            }
                        }

                        i++;
                    }
                    revealedTwo = true;
                }

                deg += 4;

                yield return new WaitForSeconds(0.01f);
            }
        }

        camRotating = false;
    }

    public bool CameraMoving()
    {
        return moveVec != Vector3.zero;
    }

    public bool CameraRotating()
    {
        return camRotating;
    }
}
