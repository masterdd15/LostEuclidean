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

    [Header("Other Variables")]
    [SerializeField] GameObject wallParent;

    bool camRotating;
    int leftWall;

    // Start is called before the first frame update
    void Start()
    {
        leftWall = startingLeftWall;

        camRotating = false;
    }

    public void RotateCamera(float direction)
    {
        StartCoroutine(CameraRotatingCoroutine(direction));
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
        bool revealed = false;
        while (deg < 90f)
        {
            if (rotatePoint != Vector3.zero)
            {
                m_Camera.transform.RotateAround(rotatePoint, Vector3.up, 2 * dir);
            }

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
                        wall.gameObject.layer = wallHiddenLayer;
                    else
                        wall.gameObject.layer = wallLayer;

                    i++;
                }

                revealed = true;
            }

            deg += 2;

            yield return new WaitForSeconds(0.01f);
        }

        camRotating = false;
    }
}
