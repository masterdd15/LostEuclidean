using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContextualPromptRotator : MonoBehaviour
{
    GameObject m_Camera = null;

    // Start is called before the first frame update
    void Start()
    {
        m_Camera = GameObject.Find("Ortho Camera");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Camera != null)
        {
            transform.LookAt(m_Camera.transform);

            Vector3 transRotation = transform.rotation.eulerAngles;

            transform.rotation = Quaternion.Euler(0f, transRotation.y + 180, 0f);
        }
    }
}
