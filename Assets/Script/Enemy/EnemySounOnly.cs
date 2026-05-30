using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System;

public class EnemySoundOnly : MonoBehaviour
{
    [Header("AI")]
    public NavMeshAgent agent;

    [Header("Patrol")]
    public Transform[] patrolPoints;
    public float patrolWaitTime = 2f;

    [Header("Chase")]
    public float chaseDuration = 5f;

    [Header("Velocidad")]
    [SerializeField] private float velocidadBase = 3.5f;

    public float MultiplicadorVelocidad = 1f;

    [Header("Kill")]
    public string deathSceneName = "GameOver";

    private int currentPatrolIndex;
    private float waitTimer;

    private bool chasingSound;
    private float chaseTimer;

    private Vector3 soundPosition;

    private void Start()
    {
        if (agent == null)
            agent = GetComponent<NavMeshAgent>();

        agent.speed = velocidadBase;

        //GoToNextPoint();


    }

    private void OnEnable()
    {
        AudioManager.OnSoundGenerated += DetectSound;
    }

    private void OnDisable()
    {
        AudioManager.OnSoundGenerated -= DetectSound;
    }

    private void Update()
    {
        agent.speed = velocidadBase * MultiplicadorVelocidad;

        if (chasingSound)
        {
            HandleChase();
        }
        //else
        //{
        //    HandlePatrol();
        //}
    }
    public void HearSound(Vector3 position)
    {
        Debug.Log("Escuche sonido");

        chasingSound = true;
        chaseTimer = chaseDuration;

        soundPosition = position;

        agent.SetDestination(soundPosition);
    }

    private void HandleChase()
    {
        chaseTimer -= Time.deltaTime;

        agent.SetDestination(soundPosition);

        if (chaseTimer <= 0f)
        {
            chasingSound = false;
            //GoToNextPoint();
        }
    }

    //private void HandlePatrol()
    //{
    //    if (patrolPoints.Length == 0)
    //        return;

    //    if (!agent.pathPending && agent.remainingDistance <= 0.3f)
    //    {
    //        waitTimer += Time.deltaTime;

    //        if (waitTimer >= patrolWaitTime)
    //        {
    //            GoToNextPoint();
    //        }
    //    }
    //}

    //private void GoToNextPoint()
    //{
    //    if (patrolPoints.Length == 0)
    //        return;

    //    waitTimer = 0f;

    //    agent.SetDestination(patrolPoints[currentPatrolIndex].position);

    //    currentPatrolIndex++;

    //    if (currentPatrolIndex >= patrolPoints.Length)
    //    {
    //        currentPatrolIndex = 0;
    //    }
    //}
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


    private void DetectSound(SoundStimulus stimulus)
    {
        float distancia = Vector3.Distance(transform.position, stimulus.Position);

        if (distancia <= stimulus.Radius)
        {
            Debug.Log(gameObject.name + " escuchó un sonido");

            HearSound(stimulus.Position);
        }
    }
}