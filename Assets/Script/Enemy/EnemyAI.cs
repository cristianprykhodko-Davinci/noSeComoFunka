using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NavMeshAgent))]

public class EnemyAI : MonoBehaviour
{
    public float MultiplicadorVelocidad  = 1f; // Modificado por Cris por tema de municiones
    [Header("Patrol")]
    [SerializeField] private Transform[] patrolPoints;
    [SerializeField] private float patrolWaitTime = 2f;

    [Header("Detection")]
    [SerializeField] private float chaseDistance = 20f;


    private Vector3 lastKnownPosition;


    private NavMeshAgent agent;
    private EnemyVision vision;

    private Transform player;
    private PlayerStealth playerStealth;

    private EnemyState currentState;

    private int currentPatrolIndex;
    private float waitTimer;

    private Vector3 investigatePosition;

    private void Awake()

    {
        agent = GetComponent<NavMeshAgent>();
        vision = GetComponent<EnemyVision>();

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent missing.");
        }

        if (vision == null)
        {
            Debug.LogError("EnemyVision missing.");
        }

    }

    private void Start()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        Debug.Log(playerObject.name);

        if (playerObject != null)
        {
            player = playerObject.transform;
            playerStealth = playerObject.GetComponent<PlayerStealth>();
        }

        currentState = EnemyState.Patrol;

        MoveToNextPatrolPoint();

        agent.updateRotation = false;
    }

    private void Update()
    {

        Debug.Log("UPDATE ENEMY");

        if (player == null ||
            vision == null ||
            agent == null)
        {
            return;
        }


        float distanceToPlayer =
            Vector3.Distance(
                transform.position,
                player.position);


        bool canSeePlayer =
            vision.CanSeePlayer(player);
        Debug.Log("LLEGO A CANSEEPLAYER");

        if (canSeePlayer)
        {


            lastKnownPosition = player.position;




            investigatePosition = player.position;


            currentState = EnemyState.Chase;
        }


        if (currentState == EnemyState.Chase &&
            distanceToPlayer > chaseDistance)
        {
            StartInvestigating();
        }


        switch (currentState)
        {
            case EnemyState.Patrol:
                HandlePatrol();
                break;

            case EnemyState.Investigate:
                HandleInvestigate();
                break;

            case EnemyState.Chase:
                HandleChase(canSeePlayer);
                break;
        }
    }

    private void HandlePatrol()
    {
        if (agent == null)
            return;

        agent.speed = 5f * MultiplicadorVelocidad; // Modificado por Cris, para aplciar el efecto de relentizar por municion;

        Vector3 moveDirection = agent.desiredVelocity;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            moveDirection.y = 0f;

            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }

        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        if (agent.pathPending)
            return;

        if (agent.remainingDistance <= agent.stoppingDistance)
        {
            waitTimer += Time.deltaTime;

            if (waitTimer >= patrolWaitTime)
            {
                waitTimer = 0f;
                MoveToNextPatrolPoint();
            }
        }
    }

    private void MoveToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        if (agent == null)
            return;

        Transform targetPoint = patrolPoints[currentPatrolIndex];

        if (targetPoint != null)
        {
            agent.SetDestination(targetPoint.position);
        }

        currentPatrolIndex++;

        if (currentPatrolIndex >= patrolPoints.Length)
        {
            currentPatrolIndex = 0;
        }
    }

    public void InvestigatePosition(Vector3 position)
    {
        if (agent == null)
            return;

        investigatePosition = position;

        currentState = EnemyState.Investigate;

        agent.SetDestination(position);
    }

    private void HandleInvestigate()
    {

        if (agent == null)
            return;

        agent.speed = 8f * MultiplicadorVelocidad; // Modificado por Cris, para aplciar el efecto de relentizar por municion;

        Vector3 moveDirection = agent.desiredVelocity;

        if (moveDirection.sqrMagnitude > 0.01f)
        {
            moveDirection.y = 0f;

            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }

        if (agent.pathPending)
            return;


        if (agent.remainingDistance <= agent.stoppingDistance)
        {

            currentState = EnemyState.Patrol;

            MoveToNextPatrolPoint();
        }
    }

    private void HandleChase(bool canSeePlayer)
    {

        if (agent == null || player == null)
            return;

        agent.speed = 10f * MultiplicadorVelocidad; // Modificado por Cris, para aplciar el efecto de relentizar por municion

        Vector3 targetPosition;


        if (canSeePlayer)
        {

            lastKnownPosition = player.position;

            targetPosition = player.position;
        }
        else
        {

            targetPosition = lastKnownPosition;
        }


        agent.SetDestination(targetPosition);


        Vector3 moveDirection = agent.desiredVelocity;


        if (moveDirection.sqrMagnitude > 0.01f)
        {
            moveDirection.y = 0f;

            Quaternion targetRotation =
                Quaternion.LookRotation(moveDirection);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                10f * Time.deltaTime
            );
        }
    }

    private void StartInvestigating()
    {
        currentState = EnemyState.Investigate;
        agent.SetDestination(lastKnownPosition);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            // Frenar enemigo
            agent.isStopped = true;

            // Frenar jugador
            PlayerStealth stealth =
                collision.collider.GetComponent<PlayerStealth>();

            if (stealth != null)
            {
                stealth.SetMovementLock(true);
            }

            // Fade muerte
            FindFirstObjectByType<FadeMuerte>().IniciarMuerte();
        }
    }
}
