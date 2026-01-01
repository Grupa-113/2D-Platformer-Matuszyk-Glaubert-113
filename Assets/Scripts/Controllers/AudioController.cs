using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    [SerializeField] private AudioSource musicSource;
    public AudioClip background;

    private const string MUSICNAME = "MusicVolume";
    private const string SFXNAME = "SFXVolume";

    private const float DEFAULT_VOL = 0.5f;
    private const float MIN_VOL = 0.0001f;


    private void Start()
    {

        musicSource.clip = background;
        musicSource.Play();
    }
}


