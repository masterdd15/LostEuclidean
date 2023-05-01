using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangingLight : MonoBehaviour
{
    private Light light;
    [SerializeField]
    bool isFlickering = false;
    [SerializeField]
    float dimIntensity = 1;
    [SerializeField]
    float xSwayIntensity = 0, ySwayIntensity = 0, xSwaySpeed = 0, ySwaySpeed = 0;

    void Awake()
    {
        light = GetComponent<Light>();
    }

    void Start()
    {
        if (isFlickering)
            StartCoroutine(Flicker());
    }

    void Update()
    {
        MoveLight();
    }

    void MoveLight()
    {
        float xSwayInt = xSwayIntensity * Random.Range(0.8f, 1.2f);
        float ySwayInt = ySwayIntensity * Random.Range(0.8f, 1.2f);
        Vector3 rotation = new Vector3(xSwayInt * 30f * Time.deltaTime * Mathf.Sin(Time.time * xSwaySpeed),
                                       ySwayInt * 30f * Time.deltaTime * Mathf.Cos(Time.time * ySwaySpeed + 0.5f * Mathf.PI),
                                       0);
        light.transform.Rotate(rotation, Space.Self);
    }

    IEnumerator Flicker()
    {
        for (int i = 0; i < Random.Range(1, 4); i++)
        {
            float intensity = light.intensity;
            light.intensity = dimIntensity;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.2f));
            light.intensity = intensity;
            yield return new WaitForSeconds(Random.Range(0.3f, 0.5f));
        }
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        StartCoroutine(Flicker());
    }
}
