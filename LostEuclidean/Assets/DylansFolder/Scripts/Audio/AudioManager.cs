using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //We are going to make sure there is only one audio manager between scenes
    public static AudioManager Instance;

    //We need to organize our sound clip variables, as to not confuse them for other references
    [Header("Background Audio")]
    [SerializeField] AudioClip greenDimensionMusic;
    [SerializeField] AudioClip redDimensionMusic;
    [SerializeField] AudioClip blueDimensionMusic;

    [Header("Button FX")]
    [SerializeField] AudioClip buttonActivate;
    [SerializeField] AudioClip buttonDeactivate;

    //We should include a reference to our children Audio Sources
    [Header("Audio Sources")]
    [SerializeField] private AudioSource _BackgroundSource;
    [SerializeField] private AudioSource _FXSource;

    private void Awake()
    {
        //This statment creates an instance if one is not already existing
        //If it already does, we delete it
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    //When we start a scene, this function will make sure that all of our sounds are set up properly
    private void Start()
    {
        if(_BackgroundSource.clip == null)
        {
            SetBackground(greenDimensionMusic);
        }
    }

    private void SetBackground(AudioClip music)
    {
        _BackgroundSource.clip = music;
        _BackgroundSource.Play();
    }

    public void PlaySound(AudioClip clip)
    {

    }

}
