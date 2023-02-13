using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextualPromptRotator : MonoBehaviour
{
    GameObject camera = null;

    // Start is called before the first frame update
    void Start()
    {
        camera = GameObject.Find("Ortho Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (camera != null)
        {
            transform.LookAt(camera.transform);

            Vector3 transRotation = transform.rotation.eulerAngles;

            transform.rotation = Quaternion.Euler(0f, transRotation.y + 180, 0f);
        }
    }
}
