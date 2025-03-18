using System.Collections;
using UnityEngine;

public class FadingPlatforms : MonoBehaviour
{
    [Header("refrenaces")]
    private Material objectToFade;
    private Collider platformCollider;

    [Header("Fading")]
    [SerializeField] private float fadeTime = 5;
    [SerializeField] private bool isFading;

    private void Start()
    {
        objectToFade = GetComponent<Renderer>().material;
        platformCollider = GetComponent<Collider>();
        coroutineControl();
    }


    /// <summary>
    /// Takes the material color of the object its on and pulls down the alpha over time and set its back over a period of time
    /// </summary>
    #region Fadin logic
    private void coroutineControl()
    {
        if (isFading)
        {
            StartCoroutine(FadeOut());
            platformCollider.enabled = true;
        }
        else
        {
            StartCoroutine(FadeIn());
            platformCollider.enabled = false;
        }
    }

    public IEnumerator FadeIn()
    {
        objectToFade.color = new Color(objectToFade.color.r, objectToFade.color.g, objectToFade.color.b, 0);
        while (objectToFade.color.a < 1f)
        {
            objectToFade.color = new Color(objectToFade.color.r, objectToFade.color.g, objectToFade.color.b, objectToFade.color.a + (Time.deltaTime / fadeTime));
            yield return null;
        }

        isFading = true;
        coroutineControl();
    }

    public IEnumerator FadeOut()
    {
        objectToFade.color = new Color(objectToFade.color.r, objectToFade.color.g, objectToFade.color.b, 1);
        while (objectToFade.color.a > 0)
        {
            objectToFade.color = new Color(objectToFade.color.r, objectToFade.color.g, objectToFade.color.b, objectToFade.color.a - (Time.deltaTime / fadeTime));
            yield return null;
        }
        isFading = false;
        coroutineControl();
    }
    #endregion
}
