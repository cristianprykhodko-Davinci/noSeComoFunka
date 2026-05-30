using UnityEngine;

public class PlayerNoiseEmitter : MonoBehaviour
{
    [Header("Noise")]
    public float walkNoiseRadius = 8f;
    public float crouchNoiseRadius = 3f;
    public float emitInterval = 0.4f;

    [Header("Movement")]
    public float movementThreshold = 0.1f;

    private CharacterController controller;
    private float timer;

    public bool IsCrouching { get; private set; }

    private void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        HandleCrouch();
        EmitNoise();
    }

    private void HandleCrouch()
    {
        IsCrouching = Input.GetKey(KeyCode.LeftShift);
    }

    private void EmitNoise()
    {
        timer += Time.deltaTime;

        if (timer < emitInterval)
            return;

        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector2 movementInput = new Vector2(horizontal, vertical);

        if (movementInput.sqrMagnitude <= 0.01f)
            return;

        timer = 0f;

        float radius = IsCrouching
            ? crouchNoiseRadius
            : walkNoiseRadius;

        Collider[] hits =
            Physics.OverlapSphere(transform.position, radius);

        foreach (Collider hit in hits)
        {
            EnemySoundOnly enemy =
                hit.GetComponent<EnemySoundOnly>();

            if (enemy != null)
            {
                Debug.Log("Enemigo escuchó sonido");

                enemy.HearSound(transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, walkNoiseRadius);

        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, crouchNoiseRadius);
    }
}
