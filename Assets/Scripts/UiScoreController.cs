using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UiScoreController : MonoBehaviour
{
    private bool gamePaused = false;
    public Text points;
    public Text record;
    public GameObject endMenu;
    public GameObject pauseMenu;
    public GameObject game;
    private float lastScale;
    public GameObject enemyGenerator;
    private AudioSource musicPlayer;
    private AudioSource effectPlayer;
    public GameObject player;
    public Slider musicSlider;
    public Slider effectSlider;

    //UpdateVolume: modifica el valor de los sliders del menu de pausa.
    private void UpdateVolume() {
        effectPlayer = player.GetComponent<AudioSource>();
        musicPlayer = game.GetComponent<AudioSource>();
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1f);
        effectSlider.value = PlayerPrefs.GetFloat("EffectVolume", 1f);
    }

    //Update: se llama una vez por frame, si el juego termina se muestra el End Menu, si se presiona p se
    // muestra el menu de pausa.
    private void Update() {
        if (game.GetComponent<GameController>().gameState == GameState.Ended) {
            points.enabled = false;
            record.enabled = false;
            endMenu.SetActive(true);
            endMenu.SendMessage("AddPlayerScore", game.GetComponent<GameController>().points);
        }
        else if (Input.GetKeyDown(KeyCode.P)) {
            if (gamePaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    //RestartUiScore: resetea el estado del menu para mostrar solo la cantidad de puntos y el recor actual.
    public void RestartUiScore() {
        points.enabled = true;
        record.enabled = true;
        pauseMenu.SetActive(false);
        endMenu.SetActive(false);
    }

    //ResumeGame: resume el juego devolviendo el timescale al ultimo estado antes de pausar el juego. se usa al
    // presionar el boton resume del menu de pausa.
    public void ResumeGame() {
        gamePaused = false;
        Time.timeScale = lastScale;
        points.enabled = true;
        record.enabled = true;
        pauseMenu.SetActive(false);
        game.GetComponent<GameController>().musicPlayer.UnPause();
    }

    //PauseGame: pausa el juego modificando el timescale y abriendo el menu de pausa. se activa al presionar p
    // mientras se encuentra en estado Playing.
    public void PauseGame() {
        UpdateVolume();
        gamePaused = true;
        lastScale = Time.timeScale;
        Time.timeScale = 0f;
        points.enabled = false;
        record.enabled = false;
        pauseMenu.SetActive(true);
        game.GetComponent<GameController>().musicPlayer.Pause();
    }

    //MainMenuGame: devuelve el juego al menu principal. se usa en el menu de pausa al presionar Main Menu.
    public void MainMenuGame() {
        gamePaused = false;
        game.GetComponent<GameController>().ResetTimeScale();
        SceneManager.LoadScene("Principal");
    }

    //RestartGame: resetea el juego, se usa en el menu de pausa al presionar el boton de restart.
    public void RestartGame() {
        gamePaused = false;
        enemyGenerator.SendMessage("StopGenerator", true);
        game.GetComponent<GameController>().musicPlayer.Stop();
        game.GetComponent<GameController>().ResetTimeScale();
        game.GetComponent<GameController>().EndButtonRestartPressed();
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
}
