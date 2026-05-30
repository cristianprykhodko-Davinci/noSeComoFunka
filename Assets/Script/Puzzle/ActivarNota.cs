using System.Security.Cryptography;
using UnityEngine;

public class ActivarNota : MonoBehaviour
{

    [SerializeField]public GameObject notaVisual;
    [SerializeField] private bool activa; 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && activa == true)
            {
               notaVisual.SetActive(true);
            }

        if (Input.GetKeyDown(KeyCode.Escape) && activa == true)
        {
            notaVisual.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            activa = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            activa = false;
            notaVisual.SetActive(false);
        }
    }
}
