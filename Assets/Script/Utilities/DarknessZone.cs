using UnityEngine;

public class DarknessZone : MonoBehaviour
{
    [Range(0f, 1f)]
    [SerializeField] private float darknessValue = 0.2f;

    private void OnTriggerEnter(Collider other)
    {
        if (other == null)
            return;

        PlayerStealth stealth = other.GetComponent<PlayerStealth>();

        if (stealth == null)
            return;

        stealth.SetDarkness(darknessValue);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == null)
            return;

        PlayerStealth stealth = other.GetComponent<PlayerStealth>();

        if (stealth == null)
            return;

        stealth.SetDarkness(1f);
    }
}
