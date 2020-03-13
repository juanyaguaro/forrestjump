using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    private AudioSource musicPlayer;
    private AudioSource effectPlayer;
    public GameObject player;
    public Slider musicSlider;
    public Slider effectSlider;

    //Start: obtiene las ultimas configuraciones.
    private void Start() {
        GetOptions();
    }
    
    //GetOptions: obtiene el ultimo valor de volumen guardado y lo asigna al valor del slider de volumen.
    private void GetOptions() {
        effectPlayer = player.GetComponent<AudioSource>();
        musicPlayer = GetComponent<AudioSource>();
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        effectSlider.value = PlayerPrefs.GetFloat("EffectVolume", 1f);
    }

    //SetMusicVolume: modifica el valor del AudioSource del Game Canvas al valor obtenido del slider de music volumen,
    // lo guarda como el ultimo registro de valor del musicvolumen.
    public void SetMusicVolume(float vol) {
        musicPlayer.volume = vol;
        PlayerPrefs.SetFloat("MusicVolume", vol);
    }

    //SetEffectVolume: modifica el valor del AudioSource del Player al valor obtenido del slider de effect volumen,
    // lo guarda como el ultimo registro de valor del effectvolumen.
    public void SetEffectVolume(float vol) {
        effectPlayer.volume = vol;
        PlayerPrefs.SetFloat("EffectVolume", vol);
    }

    //SetFullscreenMode: adapta el display a pantalla completa o ventana dependiendo si el toggle esta marcado o no.
    public void SetFullscreenMode(bool option) {
        Screen.fullScreen = option;
    }
}
