using UnityEngine;

public class GomeraUnlock : MonoBehaviour
{
    [Header("Scripts de disparo a activar")]
    [SerializeField] private MonoBehaviour[] scriptsDisparo;

    [Header("Opcional: objetos de la gomera")]
    [SerializeField] private GameObject gomeraMano;

    private bool desbloqueada;

    public void DesbloquearGomera()
    {
        if (desbloqueada) return;

        desbloqueada = true;

        // Activar scripts de disparo
        foreach (var s in scriptsDisparo)
        {
            s.enabled = true;
        }

        // Activar modelo de la mano si existe
        if (gomeraMano != null)
            gomeraMano.SetActive(true);
    }
}