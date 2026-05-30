using UnityEngine;
using System.Collections;

public class ProyectilRalentizante : proyectilBase
{
    [Header("Configuración del Efecto")]
    [SerializeField] private float duracionEfecto = 4f;
    [SerializeField] private float factorRalentizacion = 0.4f;

    protected override void AplicarEfecto(Collision collision)
    {
        EnemyAI enemigoNormal = collision.gameObject.GetComponent<EnemyAI>();

        if (enemigoNormal != null)
        {
            enemigoNormal.StartCoroutine(RutinaRalentizarEnemyAI(enemigoNormal));
            return;
        }

        EnemySoundOnly enemigoSonido = collision.gameObject.GetComponent<EnemySoundOnly>();

        if (enemigoSonido != null)
        {
            enemigoSonido.StartCoroutine(RutinaRalentizarEnemySound(enemigoSonido));
        }
    }



    private IEnumerator RutinaRalentizarEnemyAI(EnemyAI enemigo)
    {

        enemigo.MultiplicadorVelocidad = factorRalentizacion;
        Debug.Log("¡Enemigo ralentizado!");


        yield return new WaitForSeconds(duracionEfecto);


        enemigo.MultiplicadorVelocidad = 1f;
        Debug.Log("El enemigo recuperó su velocidad.");
    }

    private IEnumerator RutinaRalentizarEnemySound(EnemySoundOnly enemigo)
    {
        enemigo.MultiplicadorVelocidad = factorRalentizacion;

        Debug.Log("¡EnemySoundOnly ralentizado!");

        yield return new WaitForSeconds(duracionEfecto);

        enemigo.MultiplicadorVelocidad = 1f;

        Debug.Log("EnemySoundOnly recuperó velocidad.");

    }
}
