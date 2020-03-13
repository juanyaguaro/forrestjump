using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneratorController : MonoBehaviour
{
    public GameObject enemyPrefab;
    [Range (0f, 3f)]
    public float generatorTimer;

    //Createenemy: rea instancias del prefab de enemigos y se invoca a si misma recursivamente en intervalos de
    // 1 a generatorTimer segundos. Se utiliza en estado Playing.
    private void CreateEnemy() {
        Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        Invoke("CreateEnemy", Random.Range(1f, generatorTimer));
    }

    //StartGenerator: comienza a invocar la creacion de enemigos, se utiliza al comenzar el juego (estado Playing).
    public void StartGenerator() {
        Invoke("CreateEnemy", 0f);
    }

    //StopGenerator: cancela la generacion de enemigos, si la bandera clean es verdadera, destruye todos los
    // enemigos que fueron generados y se encuentran actualmente en juego. Esto sucede cuando el player entra en contacto
    // con algun enemigo.
    public void StopGenerator(bool clean = false) {
        CancelInvoke("CreateEnemy");
        if (clean) {
            Object[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach(GameObject enemy in enemies) {
                Destroy(enemy);
            }
        }
    }
}
