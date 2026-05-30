using UnityEngine;

public class PickupGomera : MonoBehaviour
{
    [SerializeField] private GameObject gomeraMano;
    [SerializeField] private BoxCollider boxCol;
    [SerializeField] private GomeraUnlock gomeraUnlock;

    private bool jugadorCerca;

    private void Start()
    {
        if (boxCol != null)
            boxCol.enabled = false;
    }

    public void ActivarCollider()
    {
        if (boxCol != null)
            boxCol.enabled = true;
    }

    private void Update()
    {
        if (jugadorCerca && Input.GetKeyDown(KeyCode.E))
        {
            gomeraUnlock.DesbloquearGomera();

            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
            jugadorCerca = false;
    }
}