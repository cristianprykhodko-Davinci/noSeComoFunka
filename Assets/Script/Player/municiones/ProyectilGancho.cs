using UnityEngine;
using System.Collections;
public class ProyectilGancho : proyectilBase
{
    private GameObject player;
    private bool enganchado = false;

    protected override void AplicarEfecto(Collision collision)
    {
        if (enganchado) return;

        player = GameObject.FindGameObjectWithTag("Player");
        enganchado = true;

        
        StartCoroutine(ArrastrarJugador());
    }

    IEnumerator ArrastrarJugador()
    {
        float speed = 15f;
       
        while (Vector3.Distance(player.transform.position, transform.position) > 1.5f)
        {
            player.transform.position = Vector3.MoveTowards(
                player.transform.position,
                transform.position,
                speed * Time.deltaTime
            );
            yield return null;
        }
        Destroy(gameObject);
    }
}
