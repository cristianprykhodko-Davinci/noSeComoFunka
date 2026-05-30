using UnityEngine;

public class ProyectilRuido : proyectilBase
{
    [Header("Configuración de Distracción")]
    [SerializeField] private float radioDelRuido = 15f;
    [SerializeField] private float intensidadDelRuido = 1f; 

    protected override void AplicarEfecto(Collision collision)
    {
       
        Vector3 puntoImpacto = collision.contacts[0].point;

        Debug.Log("¡La munición de ruido impactó en: " + puntoImpacto + "!");

       
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.GenerateSound(puntoImpacto, radioDelRuido, intensidadDelRuido);
        }
    }
}
