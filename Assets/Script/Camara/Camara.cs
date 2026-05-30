using UnityEngine;

public class Camara : MonoBehaviour
{
    [Header("Referencias")]
    public Transform jugador;

    [Header("Camara")]
    public float distancia = 4f;
    public float altura = 1.6f;
    public float sensibilidad = 200f;

    private float yaw;
    private float pitch = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Arrancar alineado con el jugador (desde atrás)
        yaw = jugador.eulerAngles.y;

        Vector3 offset = Quaternion.Euler(0f, yaw, 0f) * new Vector3(0f, 0f, -distancia);
        transform.position = jugador.position + offset + Vector3.up * altura;

        transform.LookAt(jugador.position + Vector3.up * altura);
    }

    void LateUpdate()
    {
        if (jugador == null) return;

        float mouseX = Input.GetAxis("Mouse X") * sensibilidad * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidad * Time.deltaTime;

        yaw += mouseX;
        pitch -= mouseY;
        pitch = Mathf.Clamp(pitch, -10f, 30f);

        // Rotación cámara
        transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Posición deseada detrás del jugador
        Vector3 offset = Quaternion.Euler(0f, yaw, 0f) * new Vector3(0f, 0f, -distancia);

        Vector3 origen = jugador.position + Vector3.up * altura;
        Vector3 destino = origen + offset;

        Vector3 direction = (destino - origen).normalized;

        // Raycast hacia atrás
        if (Physics.Raycast(origen, direction, out RaycastHit hit, distancia * distancia))
        {
            transform.position = hit.point - direction * 2f;
        }
        else
        {
            transform.position = destino;
        }

        // Debug
        Debug.DrawRay(origen, direction * distancia * distancia, Color.red);
    }

}