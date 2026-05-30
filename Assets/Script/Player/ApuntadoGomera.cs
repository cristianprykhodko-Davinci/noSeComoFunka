using UnityEngine;

public class ApuntadoGomera : MonoBehaviour
{
    [Header("Configuración de Apuntado")]
    [SerializeField] private float velocidadRotacion = 20f;
    [SerializeField] private Transform referenciaCamara; // arrastrá aquí tu objeto Camara en el inspector
    [SerializeField] private float offsetYaw = 0f;       // opcional: +90 si querés que el lateral sea el frente
    private bool apuntando = false;

    void Update()
    {
        // 🔹 Al presionar click → activar apuntado
        if (Input.GetMouseButtonDown(0))
        {
            apuntando = true;
        }

        // 🔹 Mientras apuntás, seguir la cámara
        if (apuntando && referenciaCamara != null)
        {
            SeguirCamara();
        }

        // 🔹 Si querés desactivar apuntado al soltar click
        if (Input.GetMouseButtonUp(0))
        {
            apuntando = false;
        }
    }

    private void SeguirCamara()
    {
        // Tomamos solo el yaw de la cámara
        float yawCamara = referenciaCamara.eulerAngles.y;

        // Construimos la rotación del personaje en Y + offset
        Quaternion rotacionObjetivo = Quaternion.Euler(0, yawCamara + offsetYaw, 0);

        // Copiamos suavemente
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            rotacionObjetivo,
            velocidadRotacion * Time.deltaTime
        );
    }
}