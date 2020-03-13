using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum GameState {Idle, Playing, Ended, Ready};

public class GameController : MonoBehaviour
{
    public GameState gameState = GameState.Idle;
    [Range (0f, 0.06f)]
    public float parallaxSpeed;
    [Range (0f, 6f)]
    public float scaleTime;
    [Range (0f, 0.300f)]
    public float scaleInc;
    private GameObject[] backgrounds;
    private GameObject[] platforms;
    public GameObject UiIdle;
    public GameObject UiScore;
    public GameObject player;
    public GameObject enemyGenerator;
    public AudioSource musicPlayer;
    public int points { get; private set; }
    public Text pointsText;
    public Text recordText;
    public bool userAction = false;
    public GameObject backgroundGenerator;

    //Start: se llama antes de la primera llamada a Update().
    private void Start() {
        points = 0;
        musicPlayer = GetComponent<AudioSource>();
        recordText.text = "BEST: " + GetMaxScore().ToString();
        backgroundGenerator.SendMessage("StartBG");
        backgrounds = GameObject.FindGameObjectsWithTag("Background");
        platforms = GameObject.FindGameObjectsWithTag("Platform");
    }

    // Update: se llama una vez por cada frame para verificar el estado de juego y las acciones a realizar.
    private void Update() {
        // comienzo del juego
        switch (gameState) {
            case GameState.Idle:
                StartPlaying();
                break;
            case GameState.Playing:
                ExecuteParallaxEffect();
                break;
            case GameState.Ready:
                RestartGame();
                break;
        }
    }

    //StartPlaying: prepara todos los componentes (Player, EnemyGenerator) para comenzar el juego
    // repetidamente llama al timeScale para aumentar la velocidad del juego.
    private void StartPlaying() {
        if (userAction) {
            userAction = false;
            gameState = GameState.Playing;
            UiIdle.SetActive(false);
            UiScore.SetActive(true);
            player.SendMessage("UpdateState", "PlayerRun");
            player.SendMessage("DustPlay");
            enemyGenerator.SendMessage("StartGenerator");
            musicPlayer.Play();
            InvokeRepeating("GameTimeScale", scaleTime, scaleTime);
        }
    }

    //ExecuteParallaxEffect: sobre cada capa de Background y Platform del Game canvas
    // las texturas se desplazan a una velocidad constante hacia la izquierda.
    private void ExecuteParallaxEffect() {
        float BGfinalSpeed = parallaxSpeed * Time.deltaTime;
        float PFfinalSpeed = BGfinalSpeed * 2.5f;

        foreach (var bgLayer in backgrounds) {
            RawImage imageBgLayer = bgLayer.GetComponent<RawImage>();
            imageBgLayer.uvRect = new Rect(imageBgLayer.uvRect.x + BGfinalSpeed, 0f, 1f, 1f);
        }

        foreach (var pfLayer in platforms) {
            RawImage imagePfLayer = pfLayer.GetComponent<RawImage>();
            imagePfLayer.uvRect = new Rect(imagePfLayer.uvRect.x + PFfinalSpeed, 0f, 1f, 1f);
        }
    }

    //RestartGame: regresa el juego a la escena principal.
    public void RestartGame() {
        if (userAction)
            SceneManager.LoadScene("Principal");
    }

    //GameTimeScale: incrementa el timeScale en scaleInc unidades, esta medida es arbitaria.
    private void GameTimeScale() {
        Time.timeScale += scaleInc;
        Debug.Log("Current TimeScale: " + Time.timeScale.ToString());
    }

    //ResetTimeScale: devuelve el timeScale al tiempo original. Deberia cambiarlo a tener un parametro por default 1
    //para que, resetee el timeScale al que se tenia antes de pausar el juego.
    public void ResetTimeScale() {
        CancelInvoke("GameTimeScale");
        Time.timeScale = 1f;
        Debug.Log("Resetted TimeScale");
    }

    //IncreasePoints: incrementa la cantidad de puntos actuales y los muestra en el recordText.
    public void IncreasePoints() {
        points++;
        pointsText.text = points.ToString();
        if (points > GetMaxScore()) {
            recordText.text = "BEST: " + points.ToString();
            SetMaxScore(points);
        }
    }

    //GetMaxScore: devuelve la maxima puntuacion guardada.
    public int GetMaxScore() {
        return PlayerPrefs.GetInt("Max Points", 0);
    }

    //SetMaxScore: guarda la maxima puntuacion para ser instanciada en todas las partidas.
    public void SetMaxScore(int currentPoints) {
        PlayerPrefs.SetInt("Max Points", currentPoints);
    }

    //MainButtonPlayPressed: cambia el userAction a true para que cuando se realice el proximo Update(),
    // pueda entrar en el caso correspondiente y ejecutarse. se usa para empezar el juego y volver al menu principal.
    public void MainButtonPlayPressed() {
        userAction = true;
    }

    //MainButtonQuitPressed: se sale de la aplicacion.
    public void MainButtonQuitPressed() {
        Application.Quit();
    }

    //EndButtonRestartPressed: se utiliza para resetear la partida y no tener que pasar por el menu antes
    // cambia userAction con el mismo proposito que MainButtonPlayPressed.
    public void EndButtonRestartPressed() {
        points = 0;
        pointsText.text = points.ToString();
        userAction = true;
        gameState = GameState.Idle;
        UiScore.SendMessage("RestartUiScore");
    }
}
