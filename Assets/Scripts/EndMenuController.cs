using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Medal {Zero, One, Two, Three, Four, Five};

public class EndMenuController : MonoBehaviour
{
    public Text scoreText;
    public RawImage medal;

    //AddPlayerScore: obtiene la puntuacion del jugador y la muestra en el menu final, asi como carga el sprite de medalla
    // correspondiente dependiendo de la puntuacion obtenida.
    public void AddPlayerScore(int playerScore) {
        string medalImagePath = "medallas/medal" + GetMedal(playerScore).ToString();
        medal.texture = Resources.Load<Texture2D>(medalImagePath);
        scoreText.text = "Your Score: " + playerScore.ToString();
    }

    //GetMedal: devuelve el indice del sprite de la medalla a mostrar dependiendo de la puntuacion del jugador,
    // evaluando dicha puntuacion en intervalos, utiliza el enum Medal para mayor entendimiento.
    // REFACTORIZAR EVALUACION POR INTERVALOS.
    private int GetMedal(int score) {
        Medal medalByScore = Medal.Zero;

            if (10 <= score && score < 20) // 10 20
                medalByScore = Medal.One;
            else if (20 <= score && score < 30) // 20 30
                medalByScore = Medal.Two;
            else if (30 <= score && score < 40) // 30 40
                medalByScore = Medal.Three;
            else if (40 <= score && score < 50) // 40 50
                medalByScore = Medal.Four;
            else if (50 <= score) // 50 >
                medalByScore = Medal.Five;

        return (int) medalByScore;
    }
}
