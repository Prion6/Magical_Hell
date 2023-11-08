using System.Collections;
using UnityEngine;
using UnityEngine.Analytics;

public class FireWormController : Enemy
{
    [Header("Combat Stats (Child)")]
    [SerializeField] protected float attackDistance;
    [SerializeField] protected float chaseDistance;

    [Header("Projectile Stats")]    
    [SerializeField] private GameObject fireballPrefab; // Prefab de la bola de fuego.
    [SerializeField] private Transform fireballSpawnPoint; // Punto de origen para las bolas de fuego.
    [SerializeField] private float projectileDamage;
    [SerializeField] private int numFireballs; // Cantidad de bolas de fuego a disparar.

    // Inicializa las probabilidades de los ataques.
    private float shortAttackProbability = 0f;
    private float midAttackProbability = 0.5f; // Misma probabilidad para midattack y longattack.

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        // Llama a la funci�n para iniciar el comportamiento aleatorio.
        StartCoroutine(RandomBehavior());
    }

    private IEnumerator RandomBehavior()
    {
        if (state != State.Injured && state != State.Death) {
            while (true)
            {
                
                direction = (playerTransform.position - transform.position).normalized;
                // Calcula la distancia entre el jugador y el enemigo.
                float distanceToPlayer = Vector3.Distance(playerTransform.position, transform.position);

                // Si no est� a distancia para shortattack, iguala las probabilidades de midattack y longattack.
                if (distanceToPlayer > attackDistance)
                {
                    shortAttackProbability = 0f;
                    midAttackProbability = 0.5f;
                }
                else
                {
                    // Calcula las probabilidades en funci�n de la distancia (por ejemplo, a menor distancia, mayor probabilidad).
                    shortAttackProbability = Mathf.Clamp01(1.0f - distanceToPlayer / attackDistance);
                    midAttackProbability = 0.5f; // Restablece la probabilidad para midattack.
                }

                // Genera un n�mero aleatorio para determinar qu� comportamiento realizar en funci�n de las probabilidades.
                float randomValue = Random.value;

                if (randomValue <= shortAttackProbability)
                {
                    // Mueve al Fire Worm a un punto aleatorio dentro del radio especificado.
                    SetState(State.ShortAttack);
                }
                else if (randomValue <= shortAttackProbability + midAttackProbability)
                {
                    // Realiza un ataque de carga hacia el jugador.
                    SetState(State.MidAttack);
                }
                else
                {
                    // Dispara 3 bolas de fuego en forma de abanico.
                    SetState(State.LongAttack);
                }

                // Espera un tiempo antes de realizar el pr�ximo comportamiento.
                yield return new WaitForSeconds(AttackDelay);
            }
        }
    }

    protected override void MidRangeAttack()
    { 
        // Calcula la direcci�n hacia el jugador.
        direction = (playerTransform.position - transform.position).normalized;
        rb2d.drag = 5;
        MovementSpeed = 35;

        // Impulsa al Fire Worm hacia el jugador.
        rb2d.AddForce(direction * MovementSpeed, ForceMode2D.Impulse);

        // Espera un tiempo antes de detener el ataque.
        StartCoroutine(StopAttack());
    }

    protected override void LongRangeAttack()
    { 
        // Dispara un abanico de bolas de fuego.
        for (int i = 0; i < numFireballs; i++)
        {
            // Calcula un �ngulo para la direcci�n de disparo.
            float angle = (i - (numFireballs - 1) / 2.0f) * 15f;

            // Calcula la direcci�n de disparo basada en el �ngulo.
            Vector2 projectileDirection = Quaternion.Euler(0, 0, angle) * direction;

            // Instancia una bola de fuego en la direcci�n calculada.
            GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
            fireball.GetComponent<WormFireballLogic>().direction = projectileDirection;
            fireball.GetComponent<WormFireballLogic>().damage = projectileDamage;
        }
    }
    private IEnumerator StopAttack()
    {
        // Espera un tiempo antes de detener el ataque.
        yield return new WaitForSeconds(2f);

        // Detiene el ataque y reinicia la velocidad.
        SetState(State.Idle);
        MovementSpeed = 6;
        rb2d.drag = 10;
    }
}
