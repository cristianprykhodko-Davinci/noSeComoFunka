using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeMuerte : MonoBehaviour
{
    public Image fadeImage;

    public float fadeSpeed = 1f;

    public void IniciarMuerte()
    {
        StartCoroutine(FadeNegro());
    }

    private IEnumerator FadeNegro()
    {
        Color color = fadeImage.color;

        while (color.a < 1f)
        {
            color.a += Time.deltaTime * fadeSpeed;

            fadeImage.color = color;

            yield return null;
        }

        SceneManager.LoadScene("Muerte");
    }

}