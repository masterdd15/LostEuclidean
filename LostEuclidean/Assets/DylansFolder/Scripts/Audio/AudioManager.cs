using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    [Header("Transition Modifiers")]
    [SerializeField] private float timeToFade;
    [SerializeField] private float maxVolume;

    //These will be composed of ur sound effect sources
    [Header("FX Sources")]
    [SerializeField] private AudioSource _SFXSource;
    [SerializeField] AudioSource _ButtonSFXSource;

    //We need to organize our sound clip variables, as to not confuse them for other references
    [Header("Background Audio")]
    [SerializeField] AudioClip greenDimensionMusic;
    [SerializeField] AudioClip redDimensionMusic;
    [SerializeField] AudioClip blueDimensionMusic;

    [Header("Sound FX")]
    [SerializeField] SFX[] sfxSounds;

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

        //We link the SFX audio sourcec by linking it manually. We can create a line of code later would be code.


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
                _MusicGreenSource.volume = maxVolume;
                _MusicRedSource.volume = 0;
                _MusicBlueSource.volume = 0;
                break;
            case LightColor.Red:
                _MusicRedSource.volume = maxVolume;
                _MusicGreenSource.volume = 0;
                _MusicBlueSource.volume = 0;
                break;
            case LightColor.Blue:
                _MusicBlueSource.volume = maxVolume;
                _MusicGreenSource.volume = 0;
                _MusicRedSource.volume = 0;
                break;
            default:
                Debug.Log("ERROR: MUSIC COLOR NOT FOUND");
                break;
        }
    }

    //This will trigger when Color Room changes the room color
    //We will need to detect if this is different from the current rooms color, and if so, transition our music
    public void HandleCurrentDimension(LightColor detectedColor)
    {
        Debug.Log("We reached out Handle Dimension");
        
        //We see if they are not the same
        if(currentDimension != detectedColor)
        {
            Debug.Log("We are changing our music");
            //Making sure we don't intrupt something else
            //StopAllCoroutines();

            StartCoroutine(TransitionMusic(detectedColor));
        }

        //We only need to change something if we detect a differnce. If not, we just let our current dimensions music stay
    }

    IEnumerator TransitionMusic(LightColor newColor)
    {
        //Lets find our old audio source, and our new one
        AudioSource oldSource;
        AudioSource newSource;

        //We need to keep track of how much time has passed
        float timeElapsed = 0;

        //Old Dimension we are transitioning out of
        switch(currentDimension)
        {
            case LightColor.Green:
                oldSource = _MusicGreenSource;
                break;
            case LightColor.Red:
                oldSource = _MusicRedSource;
                break;
            case LightColor.Blue:
                oldSource = _MusicBlueSource;
                break;
            default:
                Debug.Log("ERROR WITH TRANSITION MUSIC OLD SOURCE");
                oldSource = _MusicGreenSource;
                break;
        }

        //New Dimension we are transitioning into
        switch (newColor)
        {
            case LightColor.Green:
                newSource = _MusicGreenSource;
                break;
            case LightColor.Red:
                newSource = _MusicRedSource;
                break;
            case LightColor.Blue:
                newSource = _MusicBlueSource;
                break;
            default:
                Debug.Log("ERROR WITH TRANSITION MUSIC NEW SOURCE");
                newSource = _MusicGreenSource;
                break;
        }

        //We will include variables to control the speed at which the transition occurs
        while(timeElapsed < timeToFade)
        {
            oldSource.volume = Mathf.Lerp(maxVolume, 0, timeElapsed / timeToFade);
            newSource.volume = Mathf.Lerp(0, maxVolume, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        oldSource.volume = 0;
        newSource.volume = maxVolume;

        //Finally, we have to update our current color to the new color now that we have transitioned
        currentDimension = newColor;

        yield return null;

    }

    public void PlaySFX(string name)
    {
        //This line is to see if this name exists in our array
        SFX s = Array.Find(sfxSounds, x => x.name == name);
        Debug.Log(s.name);
        Debug.Log(s.clip.name);

        if(s == null)
        {
            Debug.Log("Sound not found");
        }
        else
        {
            if (name != "But_Act")
            {
                //Sound Effect is set
                _SFXSource.PlayOneShot(s.clip);
            }
            else if (!_ButtonSFXSource.isPlaying)
            {
                _ButtonSFXSource.Play();
            }
        }

    }

}
