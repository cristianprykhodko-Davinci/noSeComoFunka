using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Transform camara;
    private CharacterController controlador;

    [Header("Movimiento")]
    [SerializeField] private bool usarGetAxisRaw = true;

    [Header("Crouch")]
    [SerializeField] private float normalSpeed = 5f;
    [SerializeField] private float crouchSpeed = 2f;

    private PlayerNoiseEmitter noiseEmitter;

    [Header("Gravedad")]
    [SerializeField] private float gravedad = -9f;
    private Vector3 velocidadVertical;

    public GameObject Notif;
    public Animator Puerta;
    //public Animator Puerta1;
    [SerializeField] private Animator animador;

    private void Awake()
    {
        controlador = GetComponent<CharacterController>();

        if (camara == null && Camera.main != null)
            camara = Camera.main.transform;

        noiseEmitter = GetComponent<PlayerNoiseEmitter>();
    }

    void Update()
    {

        PlayerStealth stealth =
     GetComponent<PlayerStealth>();
       
        // ¿Está bloqueado?
        bool movementLocked =
            stealth != null &&
            stealth.IsMovementLocked;

        // SOLO permitir interactuar
        // si NO está escondido
        if (!movementLocked &&
       Keyboard.current != null &&
       Keyboard.current.eKey.wasPressedThisFrame &&
       Notif.activeInHierarchy)
        {
            Puerta.SetTrigger("Abrir");
            Notif.SetActive(false);
        }

        //if (Input.GetMouseButton(0))
        //{
        //    animador.SetTrigger("Disparar");
        //} ESTA REPETIDO EN DISPAROGOMERA

        // Si está escondido
        if (movementLocked)
        {
            // Frenar velocidad vertical
            velocidadVertical = Vector3.zero;

            // Frenar animaciones
            if (animador != null)
            {
                animador.SetBool("Caminando", false);
            }

            // NO ejecutar movimiento
            return;
        }


        MoverJugadorPlano();
        Gravedad();
        
    }

    private void MoverJugadorPlano()
    {
        // BLoquea el movimiento del jugador
        PlayerStealth stealth =
    GetComponent<PlayerStealth>();

        if (stealth != null &&
            stealth.IsMovementLocked)
        {
            return;
        }

        //Capturamos las teclas (AWSD y Flechas)
        float ValorHorirontal = usarGetAxisRaw ? Input.GetAxisRaw("Horizontal") : Input.GetAxis("Horizontal");
        float ValorVertical = usarGetAxisRaw ? Input.GetAxisRaw("Vertical") : Input.GetAxis("Vertical");

        //Calculamos hacia donde mira la camara solo en eje (adelante y atras) y (derecha e izquierda)
        Vector3 adelanteCamara = camara.forward;
        Vector3 derechaCamara = camara.right;


        //bloqueo movimiento de camara eje Y (elimina eje y porque no necesitamos)
        adelanteCamara.y = 0f;
        derechaCamara.y = 0f;

        //Normaliza para no tener valores diferentes
        adelanteCamara.Normalize();
        derechaCamara.Normalize();

        //Combina para tener flechas diagonales
        Vector3 direccionPlano = (derechaCamara * ValorHorirontal + adelanteCamara * ValorVertical);


        //Hace que cuando precione dos direcciones en diagonal valla mas rapido
        if (direccionPlano.sqrMagnitude > 0.0001f)
        {
            direccionPlano.Normalize();

            // SOLO ROTAR SI NO ESTA APUNTANDO
            if (!Input.GetMouseButton(0))
            {
                Quaternion rotacionObjetivo =
                    Quaternion.LookRotation(direccionPlano);

                transform.rotation = Quaternion.Slerp(
                    transform.rotation,
                    rotacionObjetivo,
                    10f * Time.deltaTime
                );
            }
        }


        //Le da velocidad y que no dependa del tiempo de los FPS
        float velocidadActual = noiseEmitter != null && noiseEmitter.IsCrouching ? crouchSpeed :normalSpeed;

        Vector3 desplazamientoXZ =
            direccionPlano * (velocidadActual * Time.deltaTime);
        controlador.Move(desplazamientoXZ);

        //Animacion
        bool caminando = direccionPlano.sqrMagnitude > 0.0001f;
        animador.SetBool("Caminando", caminando);
    }

    private void Gravedad()
    {
        velocidadVertical.y += gravedad * Time.deltaTime;
        controlador.Move(velocidadVertical * Time.deltaTime);

        if(controlador.isGrounded && velocidadVertical.y <0)
        { 
            velocidadVertical.y = -2f;
        }
    }

    void OnTriggerEnter(Collider obj )
    {
        if (obj.tag == "Puerta")
        {
            Notif.SetActive(true);
        }
    }

    void OnTriggerExit(Collider obj)
    {
        if (obj.tag == "Puerta")
        {
            Notif.SetActive(false);
        }
    }
}
