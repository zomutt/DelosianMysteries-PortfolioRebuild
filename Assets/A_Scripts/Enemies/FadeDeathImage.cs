using System.Collections;
using UnityEngine;

public class FadeDeathImage : MonoBehaviour
{
    [SerializeField] private float fadeDuration = .5f;
    [SerializeField] private float timeBeforeFade = .5f; 
    private SpriteRenderer sr;
    private Color startingColor;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        startingColor = sr.color;
    }
    private void OnEnable()
    {
        StartCoroutine(FadeSequence());
    }

    private void OnDisable()
    {
        sr.color = startingColor;
    }
    private IEnumerator FadeSequence()
    {
        yield return new WaitForSeconds(timeBeforeFade);
        Color color = sr.color;
        float elapsedTime = 0;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration; 
            color.a = Mathf.Lerp(1, 0, t);
            sr.color = color;
            yield return null;
        }
        gameObject.SetActive(false);
    }
}
