using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Range (0, 5f)]
    public float velocity;
    private Rigidbody2D EnemyRigidBody2d;

    //Start: obtiene el RigidBody2D y aisgna una velocidad de desplazamiento hacia la izquierda.
    private void Start() {
        EnemyRigidBody2d = GetComponent<Rigidbody2D>();
        EnemyRigidBody2d.velocity = Vector2.left * velocity;   
    }

    //OnTriggerEnter2D: se destruye si el objeto con el que colisiona es el EnemyDestroyer.
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == "Destroyer")
            Destroy(gameObject);
    }
}
