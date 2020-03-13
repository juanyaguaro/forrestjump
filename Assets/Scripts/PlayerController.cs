using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject game;
    public GameObject enemyGenerator;
    public AudioClip jumpClip;
    public AudioClip dieClip;
    public AudioClip pointClip;
    private Animator animator;
    private AudioSource audioPlayer;
    private float startY;
    public ParticleSystem dust;

    //Start: obtiene la posicion inicial para la comprobacion de si el jugador esta en el suelo,
    // tambien obtiene el audioSource para reproducir los sonidos de las animaciones y obtiene el animator
    // para reproducir las animaciones mismas.
    private void Start() {
        animator = GetComponent<Animator>();
        audioPlayer = GetComponent<AudioSource>();
        startY = transform.position.y;
    }

    //Update: detecta si fue presionada alguna tecla y si el jugador se encuentra en estado de juego y en el suelo
    // para poder saltar. Se llama cada frame.
    private void Update() {
        bool userAction = Input.GetKeyDown(KeyCode.JoystickButton0) || Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0);
        bool gamePlaying = game.GetComponent<GameController>().gameState == GameState.Playing;
        bool inGround = transform.position.y == startY;

        if (inGround && gamePlaying && userAction && Time.timeScale != 0f) {
            UpdateState("PlayerJump");
            audioPlayer.clip = jumpClip;
            audioPlayer.Play();
        }
    }

    //UpdateState: recibe un nombre de animacion y la reproduce, se usa en todas las reproducciones de animaciones
    // del player.
    public void UpdateState(string state = null) {
        if (state != null)
            animator.Play(state);
    }

    //OnTriggerEnter2D: detecta si existe colision del player con un enemigo o con la zona de puntuacion
    // incrementa los puntos o la partida termina llamando a todas las instancias necesarias.
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Enemy") {
            UpdateState("PlayerDie");
            game.GetComponent<GameController>().gameState = GameState.Ended;
            enemyGenerator.SendMessage("StopGenerator", true);
            game.SendMessage("ResetTimeScale");
            
            game.GetComponent<AudioSource>().Stop();
            audioPlayer.clip = dieClip;
            audioPlayer.Play();

            DustStop();
        }
        else if (other.gameObject.tag == "Point") {
            game.SendMessage("IncreasePoints");
            audioPlayer.clip = pointClip;
            audioPlayer.Play();
        }
    }

    //GameReady: cambia el estado de juego del GameController a Ready para poder llamar a RestartGame().
    private void GameReady() {
        game.GetComponent<GameController>().gameState = GameState.Ready;
    }

    //DustPlay: reproduce el efecto de particulas.
    private void DustPlay() {
        dust.Play();
    }

    //DustStop: detiene la reproduccion del efecto de particulas.
    private void DustStop() {
        dust.Stop();
    }
}
