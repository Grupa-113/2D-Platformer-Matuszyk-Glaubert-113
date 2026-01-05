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
        float savedMusic = PlayerPrefs.GetFloat(MUSICNAME, DEFAULT_VOL);
        float savedSfx = PlayerPrefs.GetFloat(SFXNAME, DEFAULT_VOL);

        musicSlider.value = savedMusic;
        sfxSlider.value = savedSfx;

        SetMusicVolume(savedMusic);
        SetSfxVolume(savedSfx);

        musicSource.clip = background;
        musicSource.Play();
    }

    public void SetMusicVolume(float sliderValue)
    {
        float db = Mathf.Log10(Mathf.Max(sliderValue, MIN_VOL)) * 20f;
        mixer.SetFloat(MUSICNAME, db);
        PlayerPrefs.SetFloat(MUSICNAME, sliderValue);
    }

    public void SetSfxVolume(float sliderValue)
    {
        float db = Mathf.Log10(Mathf.Max(sliderValue, MIN_VOL)) * 20f;
        mixer.SetFloat(SFXNAME, db);
        PlayerPrefs.SetFloat(SFXNAME, sliderValue);
    }
}


