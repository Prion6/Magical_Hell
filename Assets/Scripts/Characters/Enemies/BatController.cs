using System.Collections;
using UnityEngine;

public class BatController : Enemy
{
    // ...

    [SerializeField]
    private float moveRadius = 5f; // Radio dentro del cual se mover� el enemigo.
    [SerializeField]
    private float moveInterval = 2f; // Tiempo entre movimientos.
    [SerializeField]
    private float maxMoveTime = 3f; // M�ximo tiempo permitido para llegar al objetivo.

    private int movesMade = 0;
    float elapsedMoveTime = 0f;

    private void Start()
    {
        // Llama a la funci�n para iniciar el movimiento aleatorio.
        StartCoroutine(MoveToRandomPoints());
    }

    private IEnumerator MoveToRandomPoints()
    {
        while (true)
        {
            if (state != State.Injured && state != State.Death)
            {
                // Genera un punto aleatorio dentro del radio de movimiento.
                Vector3 randomPoint = transform.position + Random.insideUnitSphere * moveRadius;

                // Mueve el enemigo hacia el punto aleatorio.
                StartCoroutine(MoveToPosition(randomPoint));
            }

            yield return new WaitForSeconds(moveInterval);

            movesMade++;

            // Si se han realizado 3 movimientos, ajusta el intervalo a 5 segundos y reinicia el contador de movimientos.
            if (movesMade >= 3)
            {
                moveInterval = 5f;
                movesMade = 0;
            }
            else
            {
                // De lo contrario, vuelve al intervalo de 2 segundos.
                moveInterval = 2f;
            }
        }
    }

    private IEnumerator MoveToPosition(Vector3 targetPosition)
    {
        float moveStartTime = Time.time;
        ChangeState(State.Move);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            // Mueve gradualmente al enemigo hacia el punto aleatorio.
            elapsedMoveTime = Time.time - moveStartTime;
            if (elapsedMoveTime > maxMoveTime)
            {
                // Genera un nuevo punto aleatorio y cambia la direcci�n del movimiento.
                targetPosition = transform.position + Random.insideUnitSphere * moveRadius;
                targetPosition.z = transform.position.z;
                moveStartTime = Time.time;
            }

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, movementSpeed * Time.deltaTime);

            yield return null;
        }

        ChangeState(State.Idle); // Indica que el movimiento ha terminado.
    }
    // ...
}