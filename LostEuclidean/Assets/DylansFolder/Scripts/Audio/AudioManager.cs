using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //We are going to make sure there is only one audio manager between scenes
    public static AudioManager Instance;

    //The Music Source is a bit special, so we need seperate sources for each dimensions music
    [Header("Music Sources")]
    [SerializeField] private AudioSource _MusicGreenSource;
    [SerializeField] private AudioSource _MusicRedSource;
    [SerializeField] private AudioSource _MusicBlueSource;
    [SerializeField] private LightColor currentDimension;

    //These will be composed of ur sound effect sources
    [Header("FX Sources")]
    [SerializeField] private AudioSource _FXSource;

    //We need to organize our sound clip variables, as to not confuse them for other references
    [Header("Background Audio")]
    [SerializeField] AudioClip greenDimensionMusic;
    [SerializeField] AudioClip redDimensionMusic;
    [SerializeField] AudioClip blueDimensionMusic;

    [Header("Button FX")]
    [SerializeField] AudioClip buttonActivate;
    [SerializeField] AudioClip buttonDeactivate;

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
        //We need to intialize all of our clips into our audio sources
        SetupColorMusic();

        //We need to know what color music we are using. We can find this from our Color Room object
        LightColor currentColor = GameObject.Find("Color Room").GetComponent<ColorRoom>().roomColor;

        //Now, we can handle the volume for what dimension should be playing
        HandleColorMusic(currentColor);


    }

    //This method is just putting a clip into an audio source.
    private void SetupColorMusic()
    {
        //We set all music to their respective sources, set their volumes to 0, and play them
        //(Is there an easier/cleaner way to do this? Should look into it.)
        //Need a way to be able to tell what color dimension each source is linked to.
        _MusicGreenSource.clip = greenDimensionMusic;
        _MusicGreenSource.volume = 0;
        _MusicGreenSource.Play();

        _MusicRedSource.clip = redDimensionMusic;
        _MusicRedSource.volume = 0;
        _MusicRedSource.Play();

        _MusicBlueSource.clip = blueDimensionMusic;
        _MusicBlueSource.volume = 0;
        _MusicBlueSource.Play();
    }

    //In this function, we choose what which audio source will be heard depending on the rooms color
    private void HandleColorMusic(LightColor roomColor)
    {
        switch(roomColor)
        {
            case LightColor.Green:
                _MusicGreenSource.volume = 1;
                _MusicRedSource.volume = 0;
                _MusicBlueSource.volume = 0;
                break;
            case LightColor.Red:
                _MusicRedSource.volume = 1;
                _MusicGreenSource.volume = 0;
                _MusicBlueSource.volume = 0;
                break;
            case LightColor.Blue:
                _MusicBlueSource.volume = 1;
                _MusicGreenSource.volume = 0;
                _MusicRedSource.volume = 0;
                break;
            default:
                Debug.Log("ERROR: MUSIC COLOR NOT FOUND");
                break;
        }
    }

    public void PlaySound(AudioClip clip)
    {

    }

}
