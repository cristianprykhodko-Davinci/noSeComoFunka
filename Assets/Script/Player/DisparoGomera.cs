using UnityEngine;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(LineRenderer))]
public class DisparoGomera : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Animator animador;
    [SerializeField] private Transform puntoDisparo;
    [SerializeField] private GameObject[] listaMunicion;
    private int muncionSeleccionadaIndex = 0;

    [Header("Disparo")]
    [SerializeField] private float fuerzaDisparo = 20f;
    [SerializeField] private Image municion1;
    [SerializeField] private Image municion2;
    [SerializeField] private bool listoParaDisparar = false;

    [Header("Trayectoria")]
    [SerializeField] private int pasosTrayectoria = 30;   // cantidad de segmentos
    [SerializeField] private float tiempoSimulacion = 2f; // segundos de simulación
    private LineRenderer lineRenderer;

    private void Start()
    {
        municion2.enabled = false;

        // Configurar LineRenderer
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = pasosTrayectoria + 1;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.yellow;
        lineRenderer.endColor = Color.yellow;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            CambiarMunicion(0);
            municion1.enabled = true;
            municion2.enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            CambiarMunicion(1);
            municion1.enabled = false;
            municion2.enabled = true;
        }

        if (Input.GetMouseButton(0))
        {
            animador.SetBool("Apuntando", true);
            DibujarTrayectoria(); // 🔹 dibujar la parábola mientras apunta
        }
        else
        {
            animador.SetBool("Apuntando", false);
            lineRenderer.positionCount = 0; // 🔹 ocultar línea cuando no apunta
        }

        if (Input.GetMouseButtonUp(0) && listoParaDisparar == true)
        {
            Disparar();
            listoParaDisparar = false;
        }
    }

    private void CambiarMunicion(int nuevoIndex)
    {
        if (listaMunicion == null || nuevoIndex >= listaMunicion.Length || listaMunicion[nuevoIndex] == null)
        {
            Debug.Log("No hay municion asignada");
        }
        muncionSeleccionadaIndex = nuevoIndex;
    }

    public void Listo()
    {
        listoParaDisparar = true;
    }

    public void Disparar()
    {
        if (listaMunicion == null || listaMunicion.Length == 0 || listaMunicion[muncionSeleccionadaIndex] == null)
        {
            Debug.LogError("No se puede disparar: No hay munición seleccionada o la lista está vacía.");
            return;
        }

        GameObject proyectil = Instantiate(
             listaMunicion[muncionSeleccionadaIndex],
             puntoDisparo.position,
             puntoDisparo.rotation
         );

        Rigidbody rb = proyectil.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.AddForce(puntoDisparo.forward * fuerzaDisparo, ForceMode.Impulse);
        }
    }

    // 🔹 Dibujar parábola con LineRenderer
    private void DibujarTrayectoria()
    {
        if (puntoDisparo == null || lineRenderer == null) return;

        lineRenderer.positionCount = pasosTrayectoria + 1;

        Vector3 origen = puntoDisparo.position;
        Vector3 velocidadInicial = puntoDisparo.forward * fuerzaDisparo;

        for (int i = 0; i <= pasosTrayectoria; i++)
        {
            float t = (tiempoSimulacion / pasosTrayectoria) * i;
            Vector3 desplazamiento = velocidadInicial * t + 0.5f * Physics.gravity * t * t;
            Vector3 puntoActual = origen + desplazamiento;

            lineRenderer.SetPosition(i, puntoActual);
        }
    }
}