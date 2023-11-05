using UnityEngine;
using System.Collections;


public class PlayerAbility : MonoBehaviour
{
    // Tiempo entre activaciones de la habilidad.
    public float damage;
    public float cooldown;

    protected virtual void Start()
    {
        // Llama al m�todo Activate cada "cooldown" segundos y lo repite continuamente.
        StartCoroutine(RepeatActivation());
    }

    public virtual void SetAbilityStats(float d, float cd, GameObject obj = null)
    {
        damage = d;
        cooldown = cd;
    }

    private IEnumerator RepeatActivation()
    {
        while (true) // Repetir indefinidamente
        {
            yield return new WaitForSeconds(cooldown);
            Activate(); // Activa la habilidad
        }
    }

    // M�todo que define el comportamiento de la habilidad.
    protected virtual IEnumerator ActivateAbility()
    {
        // Implementa el comportamiento espec�fico de la habilidad en las clases derivadas.
        yield return null;
    }

    // M�todo para activar la habilidad.
    public void Activate()
    {
        if (!IsOnCooldown)
        {
            StartCoroutine(ActivateAbility());
            StartCoroutine(Cooldown());
        }
    }

    // M�todo para controlar el tiempo de recuperaci�n.
    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(cooldown);
    }

    // Propiedad para verificar si la habilidad est� en tiempo de recuperaci�n.
    public bool IsOnCooldown
    {
        get { return IsInvoking("Cooldown"); }
    }
}