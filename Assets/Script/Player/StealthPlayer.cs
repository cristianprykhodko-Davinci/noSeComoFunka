using UnityEngine;

public class PlayerStealth : MonoBehaviour
{
    [Header("Visibility")]
    [Range(0f, 1f)]
    [SerializeField] private float darknessMultiplier = 0.5f;

    public bool IsHidden { get; private set; }

    public bool IsMovementLocked { get; private set; }

    public float CurrentVisibility
    {
        get
        {
            if (IsHidden)
                return 0f;

            return darknessMultiplier;
        }
    }

    public void SetHidden(bool hidden)
    {
        IsHidden = hidden;
    }

    public void SetDarkness(float value)
    {
        darknessMultiplier = Mathf.Clamp01(value);
    }

    public void SetMovementLock(bool locked)
    {
        IsMovementLocked = locked;
    }
}