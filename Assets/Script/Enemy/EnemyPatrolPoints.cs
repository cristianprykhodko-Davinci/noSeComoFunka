using UnityEngine;

public class EnemyPatrolPoint : MonoBehaviour
{
    [Header("Patrol Settings")]
    [SerializeField] private float waitTime = 2f;


    public float WaitTime => waitTime;

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.3f);
    }
}

