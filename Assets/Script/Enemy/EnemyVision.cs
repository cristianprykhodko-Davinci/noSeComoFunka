using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    [Header("Vision Settings")]


    [SerializeField] private float viewDistance = 50f;


    [SerializeField] private float viewAngle = 180f;

    [Header("Layers")]


    [SerializeField] private LayerMask obstacleMask;


    [SerializeField] private LayerMask playerMask;


    public bool CanSeePlayer(Transform player)
    {

        if (player == null)
            return false;


        PlayerStealth stealth =
            player.GetComponent<PlayerStealth>();

        if (stealth == null)
            return false;


        if (stealth.IsHidden)
            return false;


        Vector3 directionToPlayer =
            (player.position - transform.position).normalized;


        float distanceToPlayer =
            Vector3.Distance(transform.position, player.position);


        if (distanceToPlayer > viewDistance)
            return false;


        //float angle =
        //    Vector3.Angle(transform.forward, directionToPlayer);


        //if (angle > viewAngle * 0.5f)
        //    return false;


        Vector3 rayOrigin =
            transform.position + Vector3.up * 1.6f;

        RaycastHit hit;


        LayerMask combinedMask =
            obstacleMask | playerMask;

        bool hasHit = Physics.Raycast(
            rayOrigin,
            directionToPlayer,
            out hit,
            viewDistance,
            combinedMask
        );


        if (!hasHit)
            return false;


        if (hit.collider == null)
            return false;


        if (hit.collider.CompareTag("Player"))
        {

            float visibility =
                stealth.CurrentVisibility;

            float effectiveDistance =
                viewDistance * visibility;


            return distanceToPlayer <= effectiveDistance;
        }


        return false;
    }


    private void OnDrawGizmosSelected()
    {

        if (!enabled)
            return;


        Gizmos.color = Color.yellow;


        Gizmos.DrawWireSphere(
            transform.position,
            viewDistance
        );


        Vector3 leftBoundary =
            Quaternion.Euler(0, -viewAngle * 0.5f, 0)
            * transform.forward;


        Vector3 rightBoundary =
            Quaternion.Euler(0, viewAngle * 0.5f, 0)
            * transform.forward;


        Gizmos.color = Color.red;


        Gizmos.DrawRay(
            transform.position,
            leftBoundary * viewDistance
        );

        Gizmos.DrawRay(
            transform.position,
            rightBoundary * viewDistance
        );
    }
}