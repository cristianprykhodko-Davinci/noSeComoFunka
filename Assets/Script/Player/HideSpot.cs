using UnityEngine;
using UnityEngine.InputSystem;

public class HideSpot : MonoBehaviour
{
    [Header("Hide Spot")]
    [SerializeField] private bool requireKeyPress = true;

   
    private PlayerStealth currentPlayer;

   
    private bool playerInside;

    private void Update()
    {
        // Si el escondite no requiere tecla
        if (!requireKeyPress)
            return;

        // Si no hay jugador dentro
        if (!playerInside)
            return;

        // Chequeo Jugador
        if (currentPlayer == null)
            return;

        // Chequeo teclado
        if (Keyboard.current == null)
            return;

        // Presionar tecla E
        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            // IMPORTANTE: para que no abra la puerta a la vez que se esconde, aunque da risa cuando pasa
            // usa la interacción solo para el sigilo
            // evitando activar otros interactuables

            // Cambia el estado oculto
            bool hidden =
                !currentPlayer.IsHidden;

            // Activa/desactiva sigilo
            currentPlayer.SetHidden(hidden);

            // Bloquea/desbloquea movimiento
            currentPlayer.SetMovementLock(hidden);

            Debug.Log(hidden
                ? "Player Hidden"
                : "Player Revealed");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Chequeo
        if (other == null)
            return;

        // Busca PlayerStealth
        PlayerStealth stealth =
            other.GetComponent<PlayerStealth>();

        // Si no existe
        if (stealth == null)
            return;

        // Guarda referencia
        currentPlayer = stealth;

        // Marca jugador dentro
        playerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        // Seguridad
        if (other == null)
            return;

        // Busca stealth
        PlayerStealth stealth =
            other.GetComponent<PlayerStealth>();

        // Seguridad
        if (stealth == null)
            return;

        // SOLO permitir salir del trigger
        // si NO está escondido

        if (!stealth.IsHidden)
        {
            currentPlayer = null;
            playerInside = false;
        }
    }
}
