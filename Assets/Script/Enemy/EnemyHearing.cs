using UnityEngine;

public class EnemyHearing : MonoBehaviour
{
    private EnemyAI enemyAI;

    private void Awake()
    {
        enemyAI = GetComponent<EnemyAI>();

        if (enemyAI == null)
        {
            Debug.LogError("EnemyAI missing.");
        }
    }

    private void OnEnable()
    {
        AudioManager.OnSoundGenerated += OnSoundHeard;
    }

    private void OnDisable()
    {
        AudioManager.OnSoundGenerated -= OnSoundHeard;
    }

    private void OnSoundHeard(SoundStimulus stimulus)
    {
        Debug.Log("Enemigo escuchó sonido en: " + stimulus.Position);
        if (enemyAI == null)
            return;

        float distance = Vector3.Distance(transform.position, stimulus.Position);

        if (distance <= stimulus.Radius)
        {
            enemyAI.InvestigatePosition(stimulus.Position);
        }
    }
}
