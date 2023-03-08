using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlitchParticleSystem : MonoBehaviour
{
    ParticleSystem ps;
    ParticleSystem.MainModule main;

    void Awake()
    {
        ps = GetComponent<ParticleSystem>();
        main = ps.main;
        ChangeStartSize();
        ChangeStartColor();
    }

    void OnEnable()
    {
        ps.Play();
    }

    void OnDisable()
    {
        ps.Stop();
    }

    void ChangeStartSize()
    {
        main.startSizeX = new ParticleSystem.MinMaxCurve(0.05f, 0.2f);
        main.startSizeY = new ParticleSystem.MinMaxCurve(0.05f, 0.2f);
        main.startSizeZ = new ParticleSystem.MinMaxCurve(0.05f, 0.2f);
    }

    void ChangeStartColor()
    {
        Gradient gradient = new Gradient();

        GradientColorKey[] colorKeys = new GradientColorKey[4];

        colorKeys[0].color = Color.blue;
        colorKeys[0].time = 0.0f;
        colorKeys[1].color = Color.red;
        colorKeys[1].time = 0.33f; 
        colorKeys[2].color = Color.green;
        colorKeys[2].time = 0.67f; 
        colorKeys[3].color = Color.blue;
        colorKeys[3].time = 1.0f;

        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        alphaKeys[0].alpha = 1.0f;
        alphaKeys[0].time = 0.0f;
        alphaKeys[1].alpha = 1.0f;
        alphaKeys[1].time = 1.0f;
        gradient.SetKeys(colorKeys, alphaKeys);

        main.startColor = new ParticleSystem.MinMaxGradient(gradient);
    }
}
