using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthItem : MonoBehaviour
{
    Transform playerPosition;
    [SerializeField] private float healthPoint;
    [SerializeField] private float moveSpeed; // Velocidad de movimiento modificable

    private void Awake()
    {
        playerPosition = FindAnyObjectByType<PlayerController>().GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        // Calcula la direcci�n hacia el jugador
        Vector3 directionToPlayer = playerPosition.position - transform.position;

        // Normaliza la direcci�n para moverse a una velocidad constante
        directionToPlayer.Normalize();

        // Mueve el objeto hacia el jugador con la velocidad especificada
        transform.Translate(directionToPlayer * moveSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.layer == 6)
        {
            var player = collision.gameObject.GetComponent<PlayerController>();
            player.RecoverLife(healthPoint);
            Destroy(gameObject);
        }
    }
}
