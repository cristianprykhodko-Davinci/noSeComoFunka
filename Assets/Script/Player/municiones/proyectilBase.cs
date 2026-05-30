using UnityEngine;

public abstract class proyectilBase : MonoBehaviour
{

    protected abstract void AplicarEfecto(Collision collision);

    private void OnCollisionEnter(Collision collision)
    {
        AplicarEfecto(collision);
        // La mayoría de las municiones se destruyen al chocar (excepto quizás el gancho)
        if (!(this is ProyectilGancho)) Destroy(gameObject);
    }
}
