using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Audio;

public class CandadoPuzzle : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject panelCandado;

    [SerializeField] private TMP_Text numero1Text;
    [SerializeField] private TMP_Text numero2Text;
    [SerializeField] private TMP_Text numero3Text;

    [SerializeField] private RectTransform selector;

    [Header("Locker")]
    [SerializeField] private GameObject candado;
    [SerializeField] private Animator lockerAnimator;

    [SerializeField] private PlayerStealth currentPlayer;
    [SerializeField] private PickupGomera pickupGomera;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip sonidoCorrecto;
    [SerializeField] private AudioClip sonidoIncorrecto;


    private int numero1;
    private int numero2;
    private int numero3;

    private int seleccionActual = 0;

    private bool jugadorCerca;
    private bool abierto;

    private void Start()
    {
        panelCandado.SetActive(false);

        ActualizarUI();
    }

    private void Update()
    {
        if (abierto)
            return;

        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            AbrirPanel();
        }

        if (!panelCandado.activeSelf)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            panelCandado.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            currentPlayer.SetMovementLock(false);

            return;
        }

        // Cambiar columna
        if (Input.GetKeyDown(KeyCode.A))
        {
            seleccionActual--;

            if (seleccionActual < 0)
                seleccionActual = 2;

            MoverSelector();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            seleccionActual++;

            if (seleccionActual > 2)
                seleccionActual = 0;

            MoverSelector();
        }

        // Subir número
        if (Input.GetKeyDown(KeyCode.W))
        {
            CambiarNumero(1);
        }

        // Bajar número
        if (Input.GetKeyDown(KeyCode.S))
        {
            CambiarNumero(-1);
        }

        // Confirmar
        if (Input.GetKeyDown(KeyCode.Return))
        {
            VerificarCodigo();
        }
    }

    private void AbrirPanel()
    {
        panelCandado.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        currentPlayer.SetMovementLock(true);
    }

    private void CambiarNumero(int valor)
    {
        switch (seleccionActual)
        {
            case 0:
                numero1 += valor;

                if (numero1 > 9) numero1 = 0;
                if (numero1 < 0) numero1 = 9;
                break;

            case 1:
                numero2 += valor;

                if (numero2 > 9) numero2 = 0;
                if (numero2 < 0) numero2 = 9;
                break;

            case 2:
                numero3 += valor;

                if (numero3 > 9) numero3 = 0;
                if (numero3 < 0) numero3 = 9;
                break;
        }

        ActualizarUI();
    }

    private void ActualizarUI()
    {
        numero1Text.text = numero1.ToString();
        numero2Text.text = numero2.ToString();
        numero3Text.text = numero3.ToString();
        numero1Text.color = Color.white;
        numero2Text.color = Color.white;
        numero3Text.color = Color.white;
    }

    private void MoverSelector()
    {
        Vector2 nuevaPos = selector.anchoredPosition;

        if (seleccionActual == 0)
            nuevaPos.x = -120;

        if (seleccionActual == 1)
            nuevaPos.x = 0;

        if (seleccionActual == 2)
            nuevaPos.x = 120;

        selector.anchoredPosition = nuevaPos;
    }

    private void VerificarCodigo()
    {
        if (numero1 == 2 &&
            numero2 == 1 &&
            numero3 == 3)
        {
            abierto = true;

            numero1Text.color = Color.green;
            numero2Text.color = Color.green;
            numero3Text.color = Color.green;

            audioSource.PlayOneShot(sonidoCorrecto);

            StartCoroutine(AbrirCorrecto());

            Debug.Log("Candado abierto");
        }
        else
        {
            Debug.Log("Codigo incorrecto");

            numero1Text.color = Color.red;
            numero2Text.color = Color.red;
            numero3Text.color = Color.red;

            audioSource.PlayOneShot(sonidoIncorrecto);
        }
    }

    private IEnumerator AbrirCorrecto()
    {
        yield return new WaitForSeconds(1f);

        panelCandado.SetActive(false);

        currentPlayer.SetMovementLock(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (lockerAnimator != null)
        {
            lockerAnimator.SetTrigger("Abrir");
        }

        yield return new WaitForSeconds(1f); // o el tiempo real de la animación

        if (pickupGomera != null)
        {
            pickupGomera.ActivarCollider();
        }

        if (candado != null)
        {
            candado.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = true;

            currentPlayer = other.GetComponent<PlayerStealth>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            jugadorCerca = false;

            panelCandado.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

}